using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager I;

    [Header("Popup � ������")]
    public GameObject recipePopupUI;            // ���� ��������
    public List<Button> recipeButtons;          // ������ �� ��� ������-����� � ����������
    public Vector3 extraOffset = Vector3.zero;

    private RectTransform uiRect;
    private Camera cam;
    private Building currentBuilding;
    private Field currentField;

    private Inventory inventory;

    private void Awake()
    {
         I = this;
        cam = Camera.main;
        uiRect = recipePopupUI.GetComponent<RectTransform>();
        recipePopupUI.SetActive(false);
        inventory = Object.FindFirstObjectByType<Inventory>();
    }


    /// <summary>
    /// ����� ��� ������
    /// </summary>
    public void ShowBuildingPopUp(Building building)
    {
        currentBuilding = building;
        currentField = null;

        recipePopupUI.SetActive(true);
        List<Recipe> recipes = building.BuildingRecipes();

        FillRecipeButtons(recipes, (recipe) =>
        {
            Work(recipe);
            HidePopUps();
        });

        UpdatePopUpPosition(building.transform);
    }

    public void ShowAnimalPopUp(Animal animal)
    {
        currentBuilding = null;
        currentField = null;

        recipePopupUI.SetActive(true);
        List<Recipe> recipes = animal.AnimalRecipes();

        FillRecipeButtons(recipes, (recipe) =>
        {
            Work(recipe);
            HidePopUps();
        });

        UpdatePopUpPosition(animal.transform);
    }

    /// <summary>
    /// ����� ��� ���� (������)
    /// </summary>
    public void ShowFieldPopUp(Field field)
    {
        currentField = field;
        currentBuilding = null;

        recipePopupUI.SetActive(true);

        List<Seed> seeds = field.AllSeeds();

        // ���������� ������ ��, ������� ���� � ���������
        List<Seed> available = new List<Seed>();
        foreach (var s in seeds)
        {
            if (inventory.HasSeed(s))
                available.Add(s);
        }

        FillSeedButtons(available, (seed) =>
        {
            field.Plant(seed);
            HidePopUps();
        });

        UpdatePopUpPosition(field.transform);
    }

    public void Work(Recipe recipe)
    {
        if (currentBuilding != null)
        {
            currentBuilding.StartWork(recipe);
            currentBuilding = null;
        }
    }

    public void HidePopUps()
    {
        recipePopupUI.SetActive(false);
        currentBuilding = null;
        currentField = null;
    }

    void Update()
    {
        if (recipePopupUI.activeSelf)
        {
            if (currentBuilding != null)
                UpdatePopUpPosition(currentBuilding.transform);
            else if (currentField != null)
                UpdatePopUpPosition(currentField.transform);
        }
    }

    private void FillRecipeButtons(List<Recipe> recipes, System.Action<Recipe> onClickAction)
    {
        for (int i = 0; i < recipeButtons.Count; i++)
        {
            var btn = recipeButtons[i];

            if (i < recipes.Count)
            {
                btn.gameObject.SetActive(true);
                btn.onClick.RemoveAllListeners();

                Recipe r = recipes[i];

                btn.GetComponent<Image>().sprite = r.result.icon;
                btn.GetComponentInChildren<Text>().text = "";

                btn.onClick.AddListener(() => onClickAction(r));
            }
            else
            {
                btn.gameObject.SetActive(false);
            }
        }
    }

    private void FillSeedButtons(List<Seed> seeds, System.Action<Seed> onClickAction)
    {
        for (int i = 0; i < recipeButtons.Count; i++)
        {
            var btn = recipeButtons[i];

            if (i < seeds.Count)
            {
                btn.gameObject.SetActive(true);
                btn.onClick.RemoveAllListeners();

                Seed s = seeds[i];

                btn.GetComponent<Image>().sprite = s.icon;
                btn.GetComponentInChildren<Text>().text = inventory.GetSeedAmount(s).ToString();

                btn.onClick.AddListener(() => onClickAction(s));
            }
            else
            {
                btn.gameObject.SetActive(false);
            }
        }
    }

    private void UpdatePopUpPosition(Transform target)
    {
        Bounds b = target.GetComponent<SpriteRenderer>().bounds;
        Vector3 topRight = new Vector3(b.max.x, b.max.y, 0);
        Vector3 screenPos = cam.WorldToScreenPoint(topRight);
        uiRect.position = screenPos + extraOffset;
    }
}
