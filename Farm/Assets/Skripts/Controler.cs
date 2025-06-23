using UnityEngine;
using UnityEngine.UI;

public class Controler : MonoBehaviour
{
    [Header("Money & UI")]
    [SerializeField] private int money = 10;
    [SerializeField] private Text moneyText;

    [Header("Camera Drag Settings")]
    [Tooltip("0 = �����, 1 = ������, 2 = �������")]
    [SerializeField] private int dragMouseButton = 2;
    [Tooltip("�������� �������������� ������")]
    [SerializeField] private float dragSpeed = 1f;
    [Tooltip("����������� ����������� (px) ��� ������������� ��������������")]
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
            // ����������, �������� �� ��������������
            if (!isDraggingCamera &&
                Vector2.Distance(Input.mousePosition, pointerDownPos) > dragThreshold)
            {
                isDraggingCamera = true;
            }

            // ���� ������������� � ������� ������
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
        // ��� down
        if (Input.GetMouseButtonDown(0))
        {
            leftClickEligible = true;
            pointerDownPos = Input.mousePosition;
            // ���� �� �� ������ ������������ ��� �������������� ������ � �� ��� ������� ���� �
            // ����� �� �������
            if (0 == dragMouseButton && isDraggingCamera)
                leftClickEligible = false;

            pressedBuilding = GetBuildingUnderCursor(Input.mousePosition);
        }

        // ��� ��������� � ��� ��� �������, ���� ��������� �����
        if (Input.GetMouseButton(0) && 0 == dragMouseButton)
        {
            if (leftClickEligible &&
                Vector2.Distance(Input.mousePosition, pointerDownPos) > dragThreshold)
            {
                leftClickEligible = false;
            }
        }

        // ��� up
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

        // ��� � ����� ��������� ��������
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
