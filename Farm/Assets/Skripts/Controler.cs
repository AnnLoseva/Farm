using UnityEngine;

public class Controller : MonoBehaviour
{
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
    private bool isUsable = true;

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventoryUI;  // Сюда закинь окно инвентаря

    private void Start()
    {
        cam = Camera.main;
        uiManager = GetComponent<UIManager>();
    }

    private void Update()
    {
        if (isUsable)
        {
            HandleCameraDrag();
            HandleMouseClicks();
        }
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
            if (!isDraggingCamera &&
                Vector2.Distance(Input.mousePosition, pointerDownPos) > dragThreshold)
            {
                isDraggingCamera = true;
            }

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
        if (Input.GetMouseButtonDown(0))
        {
            leftClickEligible = true;
            pointerDownPos = Input.mousePosition;

            if (0 == dragMouseButton && isDraggingCamera)
                leftClickEligible = false;

            pressedBuilding = GetBuildingUnderCursor(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) && 0 == dragMouseButton)
        {
            if (leftClickEligible &&
                Vector2.Distance(Input.mousePosition, pointerDownPos) > dragThreshold)
            {
                leftClickEligible = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (leftClickEligible)
            {
                // Открытие инвентаря
                if (TryOpenInventory(Input.mousePosition))
                    return;

                // Поля
                Field field = GetFieldUnderCursor(Input.mousePosition);
                if (field != null)
                {
                    field.Click();
                    return;
                }

                // Здания
                Building released = GetBuildingUnderCursor(Input.mousePosition);
                if (released != null && released == pressedBuilding)
                {
                    released.Click();
                }
                else
                {
                    uiManager.HidePopUps();
                }
            }

            leftClickEligible = false;
            pressedBuilding = null;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Field field = GetFieldUnderCursor(Input.mousePosition);
            if (field != null)
            {
                field.RightClick();
                return;
            }

            Building building = GetBuildingUnderCursor(Input.mousePosition);
            if (building != null)
            {
                building.RightClick();
            }
        }
    }


    private bool TryOpenInventory(Vector2 screenPos)
    {
        Vector2 worldPoint = cam.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Inventory"))
        {
            inventoryUI.SetActive(true);
            IsUsable(false); // выключаем остальное взаимодействие
            return true;
        }
        return false;
    }

    private Building GetBuildingUnderCursor(Vector2 screenPos)
    {
        Vector2 worldPoint = cam.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Building"))
            return hit.collider.GetComponent<Building>();
        return null;
    }

    private Field GetFieldUnderCursor(Vector2 screenPos)
    {
        Vector2 worldPoint = cam.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Field"))
            return hit.collider.GetComponent<Field>();
        return null;
    }



    public void IsUsable(bool enable)
    {
        isUsable = enable;
    }
}
