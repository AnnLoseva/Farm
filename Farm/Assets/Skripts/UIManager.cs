using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager I;
    public GameObject recipePopupUI;
    public Vector3 extraOffset = Vector3.zero;

    private RectTransform uiRect;
    private Camera cam;
    private SpriteRenderer targetSr;
    Building myBuilding;


    void Awake()
    {
        I = this;
        cam = Camera.main;
        uiRect = recipePopupUI.GetComponent<RectTransform>();
        recipePopupUI.SetActive(false);
    }

    public void Show(Building building)
    {
        targetSr = building.GetComponent<SpriteRenderer>();
        recipePopupUI.SetActive(true);
        myBuilding = building;
        UpdatePosition();
    }

    public void Work(Recipe recipe)
    {
        if (myBuilding != null)
        {
            myBuilding.StartWork(recipe);
            myBuilding = null;
        }
    }

    public void Hide()
    {
        recipePopupUI.SetActive(false);
        targetSr = null;

    }

    void Update()
    {
        if (recipePopupUI.activeSelf && targetSr != null)
            UpdatePosition();
    }

    void UpdatePosition()
    {
        Bounds b = targetSr.bounds;
        Vector3 topRight = new Vector3(b.max.x, b.max.y, 0);
        Vector3 screenPos = cam.WorldToScreenPoint(topRight);
        uiRect.position = screenPos + extraOffset;
    }

    
}