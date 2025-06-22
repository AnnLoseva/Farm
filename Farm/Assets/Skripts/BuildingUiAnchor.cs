using UnityEngine;

public class BuildingUIAnchor : MonoBehaviour
{
    public GameObject recipePopupUI; // ������ �� ���� ������ UI
    public Vector3 offset = new Vector3(0, 100, 0); // �������� �����

    private RectTransform uiRect;
    private Camera cam;
    private Building building;

    private void Start()
    {
        cam = Camera.main;
        uiRect = recipePopupUI.GetComponent<RectTransform>();
        recipePopupUI.SetActive(false); // �������� ����������
        building = GetComponent<Building>();
    }

    public void ShowPopup()
    {
        recipePopupUI.SetActive(true);
        UpdateUIPosition();
    }

    public void HidePopup()
    {
        building.StartWork();
        recipePopupUI.SetActive(false);
    }

    private void Update()
    {
        if (recipePopupUI.activeSelf)
        {
            UpdateUIPosition();
        }
    }

    void UpdateUIPosition()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
        uiRect.position = screenPos + offset;
    }
}