// FieldController.cs
// ���� ������ �������� �� ������ ����: ����� popup, �������, ���� � ���� ������
// �������� ���� ������ �� GameObject ����, � �������� ���� Collider2D � Animator

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D), typeof(Animator))]
public class FieldController : MonoBehaviour
{
    [Header("Field Settings")]
    [Tooltip("������ �������� (�����), ������� ����� �������� �� ���� ����")]
    public List<Recipe> availableSeeds;            // � ����������: ���������� ������ Recipe-������

    [Header("UI Popup")]
    [Tooltip("������ �� ������ � �������� ��� ������� �����")]
    public GameObject popupUI;                     // � ����������: ���������� Panel ���� Popup

    [Tooltip("������ ������ ������ Popup, �� ����� �� ������ ����")]
    public List<Button> seedButtons;               // � ����������: ���������� Button-� � ��� �� �������, ��� � availableSeeds

    [Tooltip("�������� ������� UI ��� ����� � ������� �����������")]
    public Vector3 uiOffset = new Vector3(0, 1f, 0); // ��������� ������ ��������� popup

    // ������ �� ���������� � ��������� (���������������� � Start):
    private Animator animator;                      // �������� ���� (����� �����, ����������, �����)
    private Collider2D col;                         // ��������� ���� ��� ������
    private UIManager uiManager;                    // �������� UI (singleton)
    private Inventory inventory;                    // �������� ���������

    // ��������� ����:
    private bool isSeeded = false;                 // ������ �������� � ���� � ����
    private bool isReady = false;                 // ������ ������ � ����� � �����
    private Recipe currentSeed;                     // ������� ��������� ������ ��� �����

    void Start()
    {
        // �������� ������ �� ����������
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        uiManager = UIManager.I;                   // ���������, ��� UIManager.I ��������
        inventory = FindObjectOfType<Inventory>(); // ������ ���� � ����� ���� Inventory

        // �������� popup ��� ������
        if (popupUI) popupUI.SetActive(false);

        // ����������� ������ ������ �� ������� ����������� ������
        // ������ � availableSeeds ������ ��������� �� �������
        for (int i = 0; i < seedButtons.Count; i++)
        {
            int idx = i;
            seedButtons[idx].onClick.RemoveAllListeners();
            seedButtons[idx].onClick.AddListener(() => Plant(availableSeeds[idx]));
        }
    }

    /// <summary>
    /// ���������� �� Controler ��� ������ ����� �� ����
    /// ���������� popup, ���� ���� ������
    /// </summary>
    public void RightClick()
    {
        if (!isSeeded && !isReady)
        {
            
        }
    }

    /// <summary>
    /// ���������� �� Controler ��� ����� ����� �� ����
    /// �������� ������, ���� ���� ������
    /// </summary>
    public void Click()
    {
        if (isReady)
        {
            Harvest();
        }
    }

    /// <summary>
    /// ������ ������ ���������� �������
    /// ������� ����������� � ��������� �������� �����
    /// </summary>
    private void Plant(Recipe seed)
    {
        // �������� ������� ����� � ���������
        if (!inventory.CheckRecipe(seed))
        {
            Debug.LogWarning("�� ������� ����� ��� �������");
            return;
        }

        // �������� ����� �� ���������
        foreach (var ing in seed.ingredients)
            inventory.RemoveItem(ing.item, ing.amount);

        // ������������� ������� ���������
        currentSeed = seed;
        isSeeded = true;
        isReady = false;

        // �������� popup
        popupUI.SetActive(false);

        // ��������� �������� �����
        animator.SetTrigger("StartGrowing");
    }

    /// <summary>
    /// ���������� ����� Animation Event �� ��������� ����� �����
    /// ��������� ���� � ��������� ������� � ����� � ������ ������� �����
    /// </summary>
    public void OnGrowthComplete()
    {
        isReady = true;
        animator.SetTrigger("Ready");
    }

    /// <summary>
    /// ���� ������: ����� ��������� � ���������, ���������� ��������� ���� � ��������� �������� �����
    /// </summary>
    private void Harvest()
    {
        // ������ ������
        for (int i = 0; i < currentSeed.resultCount; i++)
            inventory.AddItem(currentSeed.result);

        // ����� ��������� ����
        isSeeded = false;
        isReady = false;

        // �������� �����
        animator.SetTrigger("Harvest");
    }

    /// <summary>
    /// ������ popup (���������� �� UIManager)
    /// </summary>
    public void HidePopup()
    {
        popupUI.SetActive(false);
    }

    /// <summary>
    /// �������� ��������� ������ ����� ������� popup
    /// ��������/��������� ������ � ����������� �� ������� ����� � ���������
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
