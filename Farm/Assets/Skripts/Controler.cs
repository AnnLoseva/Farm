using UnityEngine;
using UnityEngine.UI;

public class Controler : MonoBehaviour
{
    [Header("Money & UI")]
    [SerializeField] private int money = 10;
    [SerializeField] private Text moneyText;

    [Header("Camera Drag Settings")]
    [Tooltip("0 = левая, 1 = правая, 2 = средняя")]
    [SerializeField] private int dragMouseButton = 2;
    [Tooltip("Скорость перетаскивания камеры")]
    [SerializeField] private float dragSpeed = 1f;
    [Tooltip("Минимальное перемещение (px) для распознавания перетаскивания")]
    [SerializeField] private float dragThreshold = 5f;

    private Camera cam;
    private Vector3 dragOrigin;
    private Vector2 pointerDownPos;
    private bool isDraggingCamera;
    private bool leftClickEligible;
    private Building pressedBuilding;
    private UIManager uiManager;

    private void Start()
    {
        cam = Camera.main;
        moneyText.text = money.ToString();
        uiManager = GetComponent<UIManager>();
    }

    private void Update()
    {
        HandleCameraDrag();
        HandleMouseClicks();
    }

    private void HandleCameraDrag()
    {
        if (Input.GetMouseButtonDown(dragMouseButton))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            pointerDownPos = Input.mousePosition;
            isDraggingCamera = false;
        }

        if (Input.GetMouseButton(dragMouseButton))
        {
            // Определяем, началось ли перетаскивание
            if (!isDraggingCamera &&
                Vector2.Distance(Input.mousePosition, pointerDownPos) > dragThreshold)
            {
                isDraggingCamera = true;
            }

            // Если перетаскиваем — двигаем камеру
            if (isDraggingCamera)
            {
                Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position += (dragOrigin - currentPoint) * dragSpeed;
                dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }
    }

    private void HandleMouseClicks()
    {
        // ЛКМ down
        if (Input.GetMouseButtonDown(0))
        {
            leftClickEligible = true;
            pointerDownPos = Input.mousePosition;
            // Если та же кнопка используется для перетаскивания камеры и мы уже двинули мышь —
            // клики не годятся
            if (0 == dragMouseButton && isDraggingCamera)
                leftClickEligible = false;

            pressedBuilding = GetBuildingUnderCursor(Input.mousePosition);
        }

        // ЛКМ удержание — ещё раз сбросим, если превысили порог
        if (Input.GetMouseButton(0) && 0 == dragMouseButton)
        {
            if (leftClickEligible &&
                Vector2.Distance(Input.mousePosition, pointerDownPos) > dragThreshold)
            {
                leftClickEligible = false;
            }
        }

        // ЛКМ up
        if (Input.GetMouseButtonUp(0))
        {
            if (leftClickEligible)
            {
                Building released = GetBuildingUnderCursor(Input.mousePosition);
                if (released != null && released == pressedBuilding)
                {
                    released.Click();
                }
                else
                {
                    uiManager.Hide();
                }
            }

            leftClickEligible = false;
            pressedBuilding = null;
        }

        // ПКМ — сразу выполняем действие
        if (Input.GetMouseButtonDown(1))
        {
            Building building = GetBuildingUnderCursor(Input.mousePosition);
            if (building != null)
            {
                building.RightClick();
            }
        }
    }

    private Building GetBuildingUnderCursor(Vector2 screenPos)
    {
        Vector2 worldPoint = cam.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Building"))
            return hit.collider.GetComponent<Building>();
        return null;
    }
}
