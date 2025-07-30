using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private Transform _inventoryContents;
    [SerializeField] private Transform _pageIndicator;
    [SerializeField] private Transform _selectionFrame;

    // Page
    private int _selectedPage;
    private List<Image> _pageDots = new List<Image>();
    private bool _forcePageChange = false;

    // Slot
    private int _selectedSlot;
    private Transform[] _slots;

    // Selector
    private float _timer;
    private Vector3 _targetPosition;
    private float _selectionFrameSpeed = 2f;

    // Dialog
    [Header("Dialog Box")]
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemText;

    private void Start()
    {
        // Pages
        for (int i = 0; i < this._pageIndicator.childCount; i++)
        {
            this._pageDots.Add(this._pageIndicator.GetChild(i).GetComponent<Image>());
        }
        this._selectedPage = 0;

        // Slots
        this._slots = new Transform[this._inventoryContents.childCount];

        for (int i = 0; i < this._inventoryContents.childCount; i++)
        {
            this._slots[i] = this._inventoryContents.GetChild(i);
        }

        this.LoadItems();
    }

    private void Update()
    {
        this._timer += Time.deltaTime * this._selectionFrameSpeed;
        if (this._timer > 1f) this._timer = 1f;

        if (Input.GetKeyDown(KeyCode.LeftShift)) { this._forcePageChange = true; }
        if (Input.GetKeyUp(KeyCode.LeftShift)) { this._forcePageChange = false; }

        if (Input.GetKeyDown(KeyCode.UpArrow)) { this.ChangeSelection(-5); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { this.ChangeSelection(+5); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { this.ChangeSelection(-1); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { this.ChangeSelection(+1); }

        this._selectionFrame.position = Vector3.Lerp(this._selectionFrame.position, this._targetPosition, this._timer);
    }

    private InventoryContent[] _currentInventoryContents;
    
    // Chamado toda vez que o inventario abre
    public void LoadItems() 
    {
        this._currentInventoryContents = GameManager.Instance.GetInvenotryContents();

        this._selectedSlot = 0;
        this._targetPosition = this._slots[this._selectedSlot].position;
        this._selectionFrame.position = this._slots[this._selectedSlot].position;
        this._timer = 1f;

        this._selectedPage = 0;
        for (int i = 0; i < this._pageDots.Count; i++)
        {
            this._pageDots[i].color = new Color(1f, 1f, 1f, i == 0 ? 1f : .125f);
        }

        this.UpdateSlots();
    }

    private void ChangeSelection(int increment)
    {
        if (this._forcePageChange && Mathf.Abs(increment) == 1)
        {
            this.ChangePage(increment);
            return;
        }

        this._selectedSlot += increment;

        // vertical wrap around
        if (increment == 1 && this._selectedSlot % 5 == 0) { this._selectedSlot -= 5; this.ChangePage(+1); }
        if (increment == -1 && (this._selectedSlot + 1) % 5 == 0) { this._selectedSlot += 5; this.ChangePage(-1); }

        // horizontal wrap around
        if (this._selectedSlot < 0) { this._selectedSlot += this._slots.Length; }
        if (this._selectedSlot >= this._slots.Length) { this._selectedSlot -= this._slots.Length; }

        this._targetPosition = this._slots[this._selectedSlot].position;
        this._timer = 0f;

        this.UpdateDialogBox();
    }

    private void ChangePage(int increment)
    {
        this._pageDots[this._selectedPage].color = new Color(1f, 1f, 1f, .125f);

        this._selectedPage += increment;

        if (this._selectedPage < 0f) { this._selectedPage = this._pageDots.Count - 1; }
        if (this._selectedPage >= this._pageDots.Count) { this._selectedPage = 0; }

        this._pageDots[this._selectedPage].color = new Color(1f, 1f, 1f, 1f);

        this.UpdateSlots();
    }

    private void UpdateSlots()
    {
        int slotIndex = 0;
        int itemIndex = 0 + (20 * this._selectedPage);

        while (slotIndex < this._slots.Length)
        {
            Image itemImage = this._slots[slotIndex].GetChild(0).GetComponent<Image>();

            Debug.Log(this._currentInventoryContents.Length);
            bool isIndexInRange = itemIndex < this._currentInventoryContents.Length;
            if (isIndexInRange) { itemImage.sprite = this._currentInventoryContents[itemIndex].data.itemIcon; }
            itemImage.gameObject.SetActive(isIndexInRange);

            itemIndex += 1;
            slotIndex += 1;
        }

        this.UpdateDialogBox();
    }

    private void UpdateDialogBox()
    {
        int index = this._selectedSlot + (20 * this._selectedPage);
        if (index >= this._currentInventoryContents.Length)
        {
            this._itemImage.sprite = this._selectionFrame.GetComponent<Image>().sprite;
            this._itemText.text = $"-- no item selected --";
            return;
        }

        InventoryContent stuff = this._currentInventoryContents[index];
        this._itemImage.sprite = stuff.data.itemIcon;
        this._itemText.text = $"{stuff.data.itemName}\n{stuff.data.itemDescription}";
    }
}
