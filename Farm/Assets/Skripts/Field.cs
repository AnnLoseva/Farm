// FieldController.cs
// Этот скрипт отвечает за логику поля: показ popup, посадку, рост и сбор урожая
// Повесьте этот скрипт на GameObject поля, у которого есть Collider2D и Animator

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D), typeof(Animator))]
public class FieldController : MonoBehaviour
{
    [Header("Field Settings")]
    [Tooltip("Список рецептов (семян), которые можно посадить на этом поле")]
    public List<Recipe> availableSeeds;            // В инспекторе: перетащить нужные Recipe-ассеты

    [Header("UI Popup")]
    [Tooltip("Ссылка на панель с кнопками для посадки семян")]
    public GameObject popupUI;                     // В инспекторе: перетащить Panel пула Popup

    [Tooltip("Список кнопок внутри Popup, по одной на каждое семя")]
    public List<Button> seedButtons;               // В инспекторе: проставить Button-ы в том же порядке, что и availableSeeds

    [Tooltip("Смещение позиции UI над полем в мировых координатах")]
    public Vector3 uiOffset = new Vector3(0, 1f, 0); // Настройка высоты появления popup

    // Ссылки на компоненты и менеджеры (инициализируются в Start):
    private Animator animator;                      // Аниматор поля (клипы роста, готовности, сбора)
    private Collider2D col;                         // Коллайдер поля для кликов
    private UIManager uiManager;                    // Менеджер UI (singleton)
    private Inventory inventory;                    // Менеджер инвентаря

    // Состояние поля:
    private bool isSeeded = false;                 // Семена посажены и идут в рост
    private bool isReady = false;                 // Урожай созрел и готов к сбору
    private Recipe currentSeed;                     // Текущий выбранный рецепт для роста

    void Start()
    {
        // Получаем ссылки на компоненты
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        uiManager = UIManager.I;                   // Убедитесь, что UIManager.I настроен
        inventory = FindObjectOfType<Inventory>(); // Должен быть в сцене один Inventory

        // Скрываем popup при старте
        if (popupUI) popupUI.SetActive(false);

        // Подписываем каждую кнопку на посадку конкретного семени
        // Кнопки и availableSeeds должны совпадать по порядку
        for (int i = 0; i < seedButtons.Count; i++)
        {
            int idx = i;
            seedButtons[idx].onClick.RemoveAllListeners();
            seedButtons[idx].onClick.AddListener(() => Plant(availableSeeds[idx]));
        }
    }

    /// <summary>
    /// Вызывается из Controler при правом клике по полю
    /// Показывает popup, если поле пустое
    /// </summary>
    public void RightClick()
    {
        if (!isSeeded && !isReady)
        {
            
        }
    }

    /// <summary>
    /// Вызывается из Controler при левом клике по полю
    /// Собирает урожай, если поле готово
    /// </summary>
    public void Click()
    {
        if (isReady)
        {
            Harvest();
        }
    }

    /// <summary>
    /// Сажает семена выбранного рецепта
    /// Снимает ингредиенты и запускает анимацию роста
    /// </summary>
    private void Plant(Recipe seed)
    {
        // Проверка наличия семян в инвентаре
        if (!inventory.CheckRecipe(seed))
        {
            Debug.LogWarning("Не хватает семян для посадки");
            return;
        }

        // Списание семян из инвентаря
        foreach (var ing in seed.ingredients)
            inventory.RemoveItem(ing.item, ing.amount);

        // Устанавливаем текущее состояние
        currentSeed = seed;
        isSeeded = true;
        isReady = false;

        // Скрываем popup
        popupUI.SetActive(false);

        // Запускаем анимацию роста
        animator.SetTrigger("StartGrowing");
    }

    /// <summary>
    /// Вызывается через Animation Event по окончании клипа роста
    /// Переводит поле в состояние «готово к сбору» и играет готовый стейт
    /// </summary>
    public void OnGrowthComplete()
    {
        isReady = true;
        animator.SetTrigger("Ready");
    }

    /// <summary>
    /// Сбор урожая: выдаёт результат в инвентарь, сбрасывает состояние поля и запускает анимацию сбора
    /// </summary>
    private void Harvest()
    {
        // Выдача урожая
        for (int i = 0; i < currentSeed.resultCount; i++)
            inventory.AddItem(currentSeed.result);

        // Сброс состояния поля
        isSeeded = false;
        isReady = false;

        // Анимация сбора
        animator.SetTrigger("Harvest");
    }

    /// <summary>
    /// Скрыть popup (вызывается из UIManager)
    /// </summary>
    public void HidePopup()
    {
        popupUI.SetActive(false);
    }

    /// <summary>
    /// Обновить состояние кнопок перед показом popup
    /// Включает/отключает кнопки в зависимости от наличия семян в инвентаре
    /// </summary>
    public void UpdatePopupButtons()
    {
        for (int i = 0; i < seedButtons.Count; i++)
        {
            if (i < availableSeeds.Count)
            {
                seedButtons[i].gameObject.SetActive(true);
                bool canPlant = inventory.CheckRecipe(availableSeeds[i]);
                seedButtons[i].interactable = canPlant;
            }
            else
            {
                seedButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
