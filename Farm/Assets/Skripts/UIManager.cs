using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager I;
    [Header("Popup и кнопки")]
    public GameObject recipePopupUI;          // сама панелька
    public List<Button> recipeButtons;        // ссылки на все кнопки-слоты в инспекторе
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
    /// Открыть поп-ап и «заполнить» кнопки рецептами
    /// </summary>
    public void ShowPopUp(Building building)
    {
        currentBuilding = building;
        recipePopupUI.SetActive(true);

        // Получаем список рецептов из здания
        List<Recipe> recipes = building.availableRecipes;

        // Проходим по всем нашим заранее созданным кнопкам
        for (int i = 0; i < recipeButtons.Count; i++)
        {
            var btn = recipeButtons[i];

            if (i < recipes.Count)
            {
                // 1) Включаем кнопку и очищаем старые слушатели
                btn.gameObject.SetActive(true);
                btn.onClick.RemoveAllListeners();

                // 2) Захватываем локальную копию recipe для замыкания
                Recipe r = recipes[i];

                // 3) Меняем  картинку
                
                // если у тебя есть иконка в рецепте:
                 btn.GetComponent<Image>().sprite = r.result.icon;

                // 4) Назначаем вызов Work(r)
                btn.onClick.AddListener(() =>
                {
                    Work(r);
                    HidePopUp();
                });
            }
            else
            {
                // Отключаем все «лишние» кнопки
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