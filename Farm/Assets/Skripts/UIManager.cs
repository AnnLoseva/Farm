using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager I;
    [Header("Popup � ������")]
    public GameObject recipePopupUI;          // ���� ��������
    public List<Button> recipeButtons;        // ������ �� ��� ������-����� � ����������
    public Vector3 extraOffset = Vector3.zero;

    private RectTransform uiRect;
    private Camera cam;
    private Building currentBuilding;


    void Awake()
    {
        I = this;
        cam = Camera.main;
        uiRect = recipePopupUI.GetComponent<RectTransform>();
        recipePopupUI.SetActive(false);
    }

    /// <summary>
    /// ������� ���-�� � ����������� ������ ���������
    /// </summary>
    public void ShowPopUp(Building building)
    {
        currentBuilding = building;
        recipePopupUI.SetActive(true);

        // �������� ������ �������� �� ������
        List<Recipe> recipes = building.availableRecipes;

        // �������� �� ���� ����� ������� ��������� �������
        for (int i = 0; i < recipeButtons.Count; i++)
        {
            var btn = recipeButtons[i];

            if (i < recipes.Count)
            {
                // 1) �������� ������ � ������� ������ ���������
                btn.gameObject.SetActive(true);
                btn.onClick.RemoveAllListeners();

                // 2) ����������� ��������� ����� recipe ��� ���������
                Recipe r = recipes[i];

                // 3) ������  ��������
                
                // ���� � ���� ���� ������ � �������:
                 btn.GetComponent<Image>().sprite = r.result.icon;

                // 4) ��������� ����� Work(r)
                btn.onClick.AddListener(() =>
                {
                    Work(r);
                    HidePopUp();
                });
            }
            else
            {
                // ��������� ��� ������� ������
                btn.gameObject.SetActive(false);
            }
        }

        UpdatePopUpPosition();
    }


    public void Work(Recipe recipe)
    {
        if (currentBuilding != null)
        {
            currentBuilding.StartWork(recipe);
            currentBuilding = null;
        }
    }

    public void HidePopUp()
    {
        recipePopupUI.SetActive(false);
        currentBuilding = null;
    }

    void Update()
    {
        if (recipePopupUI.activeSelf && currentBuilding != null)
            UpdatePopUpPosition();
    }

    private void UpdatePopUpPosition()
    {
        var sr = currentBuilding.GetComponent<SpriteRenderer>();
        Bounds b = sr.bounds;
        Vector3 topRight = new Vector3(b.max.x, b.max.y, 0);
        Vector3 screenPos = cam.WorldToScreenPoint(topRight);
        uiRect.position = screenPos + extraOffset;
    }


}