using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryUIMode
{
    BoxesMode,
    EquipBoxMode,
    ItemBoxMode,
    DialogBoxMode,
}

public class InventoryUIController : MonoBehaviour
{
    private InventoryContent[] _currentInventoryContents; // copia dos VALORES do inventario (modificar nao afeta diretamente o inventario)
    private InventoryUIMode _currentInventoryMode;

    /* Selector */
    private readonly Vector2 REST_SELECTOR_SIZE = new Vector2(300f, 300f);
    private readonly Vector2 EQUIPBOX_SELECTOR_SIZE = new Vector2(54f, 74f);
    private readonly Vector2 ITEMBOX_SELECTOR_SIZE = new Vector2(98f, 74f);
    private readonly Vector2 ITEM_SELECTOR_SIZE = new Vector2(15f, 15f);
    private readonly Vector2 BUTTON_SELECTOR_SIZE = new Vector2(29f, 8f);
    private const float SELECTOR_SPEED = 2f;
    [SerializeField] private RectTransform _selector;
    private Vector3 _selectorTargetPosition;
    private Vector2 _selectorTargetSize;
    private float _selectorTimer;

    private float _bobbingTimer = 0f;
    private float _bobbingSpeed = 2f;

    [Header("Events")]
    [SerializeField] private CustomEvent OnDiaryOpen;

    /* Box */
    [Header("Boxes")]
    [SerializeField] private Transform[] _boxes;
    private int _selectedBox;

    /* Item Box */
    [Header("Item Box")]
    // Pages
    [SerializeField] private Transform _pageIndicator;
    private List<Image> _pageDots = new List<Image>();
    private bool _forcePageChange = false;
    private int _selectedPage;
    // Item Slots
    [SerializeField] private Transform _itemSlotContainer;
    private Transform[] _itemSlots;
    private int _selectedItemSlot;

    /* Dialog Box */
    [Header("Dialog Box")]

    [SerializeField] private Sprite _noItemSprite;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemText;
    // Buttons
    [SerializeField] private Transform _buttonContainer;
    private Transform[] _buttons;
    private TextMeshProUGUI[] _buttonsText;
    private int _selectedButton;

    /* Equip Slots */
    [Header("Equipment Slots")]

    [SerializeField] private Image _sla;

    private void Start()
    {
        new CustomEventListener(OnDiaryOpen, OnDiaryOpenCallback);

        // Init page indicator
        for (int i = 0; i < this._pageIndicator.childCount; i++)
        {
            this._pageDots.Add(this._pageIndicator.GetChild(i).GetComponent<Image>());
        }

        // Init item slots
        this._itemSlots = new Transform[this._itemSlotContainer.childCount];
        for (int i = 0; i < this._itemSlotContainer.childCount; i++)
        {
            this._itemSlots[i] = this._itemSlotContainer.GetChild(i);
        }

        // Init buttons
        this._buttons = new Transform[this._buttonContainer.childCount];
        this._buttonsText = new TextMeshProUGUI[this._buttonContainer.childCount];
        for (int i = 0; i < this._buttonContainer.childCount; i++)
        {
            this._buttons[i] = this._buttonContainer.GetChild(i);
            this._buttonsText[i] = this._buttons[i].GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        this.InitInventoryScreen();
    }

    private void Update()
    {
        this._selectorTimer += Time.deltaTime * SELECTOR_SPEED;
        if (this._selectorTimer > 1f) { this._selectorTimer = 1f; }

        switch (this._currentInventoryMode)
        {
            case InventoryUIMode.BoxesMode:

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    GameManager.Instance.currentUserInterface = UserInterfaces.None;
                    this.gameObject.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow)) { this.ChangeBox(-1); }
                if (Input.GetKeyDown(KeyCode.RightArrow)) { this.ChangeBox(+1); }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    this._currentInventoryMode = this._selectedBox == 0 ? InventoryUIMode.EquipBoxMode : InventoryUIMode.ItemBoxMode;
                    this.UpdateSelector();
                }
                break;

            case InventoryUIMode.EquipBoxMode:

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    this._currentInventoryMode = InventoryUIMode.BoxesMode;
                    this.UpdateSelector();
                }
                break;

            case InventoryUIMode.ItemBoxMode:

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    this._currentInventoryMode = InventoryUIMode.BoxesMode;
                    this.UpdateSelector();
                }

                if (Input.GetKeyDown(KeyCode.LeftShift)) { this._forcePageChange = true; }
                if (Input.GetKeyUp(KeyCode.LeftShift)) { this._forcePageChange = false; }

                if (Input.GetKeyDown(KeyCode.UpArrow)) { this.ChangeSelection(-5); }
                if (Input.GetKeyDown(KeyCode.DownArrow)) { this.ChangeSelection(+5); }
                if (Input.GetKeyDown(KeyCode.LeftArrow)) { this.ChangeSelection(-1); }
                if (Input.GetKeyDown(KeyCode.RightArrow)) { this.ChangeSelection(+1); }

                if (Input.GetKeyDown(KeyCode.Return)) { this.SelectItem(); }

                break;

            case InventoryUIMode.DialogBoxMode:

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    this._currentInventoryMode = InventoryUIMode.ItemBoxMode;
                    this.UpdateSelector();
                }

                if (Input.GetKeyDown(KeyCode.UpArrow)) { this.ChangeSelectedButton(-1); }
                if (Input.GetKeyDown(KeyCode.DownArrow)) { this.ChangeSelectedButton(+1); }

                if (Input.GetKeyDown(KeyCode.Return)) { this.SelectButton(); }

                break;
        }

        this._selector.position = Vector3.Lerp(this._selector.position, this._selectorTargetPosition, this._selectorTimer);
        this._selector.sizeDelta = Vector2.Lerp(this._selector.sizeDelta, this._selectorTargetSize, this._selectorTimer);

        this._bobbingTimer += Time.deltaTime * this._bobbingSpeed;
        if (this._bobbingTimer >= 1f) { this._bobbingTimer = 1f;}
        if (this._bobbingTimer <= 0f) { this._bobbingTimer = 0f;}
        if (this._bobbingTimer >= 1f || this._bobbingTimer <= 0f) { this._bobbingSpeed *= -1; }
        this._selector.sizeDelta += Vector2.one * Mathf.Lerp(-4.5f, +4.5f, this._bobbingTimer) * this._selectorTimer;
    }

    // Chamado toda vez que o inventario abre
    public void InitInventoryScreen()
    {
        this._selector.position = this._selector.parent.position;
        this._selector.sizeDelta = this.REST_SELECTOR_SIZE * 9;

        this._currentInventoryMode = InventoryUIMode.BoxesMode;
        
        this._selectedBox = 0;
        this._selectedPage = 0;
        this._selectedButton = 0;
        this._selectedItemSlot = 0;

        for (int i = 0; i < this._pageDots.Count; i++)
        {
            this._pageDots[i].color = new Color(1f, 1f, 1f, i == 0 ? 1f : .125f);
        }

        this.LoadInventory();
    }

    private void LoadInventory() // TODO: fazer mais
    {
        this._currentInventoryContents = GameManager.Instance.GetInvenotryContents();
        
        this.UpdateSelector();
        this.UpdateSlots();
    }

    private void ChangeSelection(int increment)
    {
        if (this._forcePageChange && Mathf.Abs(increment) == 1)
        {
            this.ChangePage(increment);
            return;
        }

        this._selectedItemSlot += increment;

        // vertical wrap around
        if (increment == 1 && this._selectedItemSlot % 5 == 0) { this._selectedItemSlot -= 5; this.ChangePage(+1); }
        if (increment == -1 && (this._selectedItemSlot + 1) % 5 == 0) { this._selectedItemSlot += 5; this.ChangePage(-1); }

        // horizontal wrap around
        if (this._selectedItemSlot < 0) { this._selectedItemSlot += this._itemSlots.Length; }
        if (this._selectedItemSlot >= this._itemSlots.Length) { this._selectedItemSlot -= this._itemSlots.Length; }

        this._selectorTargetPosition = this._itemSlots[this._selectedItemSlot].position;
        this._selectorTimer = 0f;

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

        if (this._itemSlots == null) return;

        while (slotIndex < this._itemSlots.Length)
        {
            Image itemImage = this._itemSlots[slotIndex].GetChild(0).GetComponent<Image>();
            TextMeshProUGUI itemAmount = this._itemSlots[slotIndex].GetChild(1).GetComponent<TextMeshProUGUI>();

            bool isIndexInRange = itemIndex < this._currentInventoryContents.Length;
            
            if (isIndexInRange)
            {
                itemImage.sprite = this._currentInventoryContents[itemIndex].data.itemIcon;
                itemAmount.text = this._currentInventoryContents[itemIndex].amount > 1 ? this._currentInventoryContents[itemIndex].amount.ToString() : string.Empty;
            }
            
            itemImage.gameObject.SetActive(isIndexInRange);

            itemIndex += 1;
            slotIndex += 1;
        }

        this.UpdateDialogBox();
    }

    private ItemType _selectedItemType = ItemType.None; // buffer

    private void UpdateDialogBox()
    {
        int itemIndex = this.GetItemIndex();
        if (itemIndex >= this._currentInventoryContents.Length)
        {
            this._itemImage.sprite = this._noItemSprite;
            this._itemText.text = "-- no item selected --";
            
            for (int i = 0; i < this._buttonsText.Length; i++)
            {
                this._buttonsText[i].text = " --- ";
                this._buttonsText[i].color = Color.white * .5f;
            }

            return;
        }

        InventoryContent highlightedItem = this._currentInventoryContents[itemIndex];
        this._itemImage.sprite = highlightedItem.data.itemIcon;
        this._itemText.text = $"{highlightedItem.data.itemName}\n{highlightedItem.data.itemDescription}";

        switch (highlightedItem.data.itemType)
        {
            case ItemType.None:
                this._buttonsText[0].text = " --- ";
                this._buttonsText[0].color = Color.white * .5f;
                break;

            case ItemType.Activatable: case ItemType.Consumable:
                this._buttonsText[0].text = "USE";
                this._buttonsText[0].color = Color.white;
                break;

            case ItemType.Headwear: case ItemType.Holdable: case ItemType.Footwear:
                this._buttonsText[0].text = "EQUIP";
                this._buttonsText[0].color = Color.white;
                break;
        }

        this._buttonsText[1].text = " OPINGON ";
        this._buttonsText[1].color = Color.white;

        this._buttonsText[2].text = " LICK ";
        this._buttonsText[2].color = Color.white;

        this._selectedItemType = highlightedItem.data.itemType;
    }

    private void SelectItem()
    {
        int itemIndex = this.GetItemIndex();

        if (this._selectedItemSlot >= this._itemSlots.Length || itemIndex >= this._currentInventoryContents.Length) return;

        this._selectedButton = this._selectedItemType == ItemType.None ? 1 : 0;

        this._currentInventoryMode = InventoryUIMode.DialogBoxMode;

        this.UpdateSelector();
    }

    private void ChangeSelectedButton(int increment)
    {
        this._selectedButton += increment;

        // wrap
        if (this._selectedButton < 0) { this._selectedButton = this._buttons.Length - 1; }
        if (this._selectedButton >= this._buttons.Length) { this._selectedButton = 0; }

        if (this._selectedItemType == ItemType.None && this._selectedButton == 0) { this.ChangeSelectedButton(increment); }

        this.UpdateSelector();
    }

    private void SelectButton()
    {
        switch (this._selectedButton)
        {
            case 0: // USE/EQUIP
                if (GameManager.Instance.UseItem(this._currentInventoryContents[this.GetItemIndex()].data))
                {
                    // update screen
                    this._currentInventoryMode = InventoryUIMode.ItemBoxMode;
                    this.LoadInventory();
                }
                break;

            case 1: // OPINION
                break;

            case 2: // LICK
                if (GameManager.Instance.LickItem(this._currentInventoryContents[this.GetItemIndex()].data))
                {
                    if (this._currentInventoryContents[this.GetItemIndex()].amount <= 0) { this._currentInventoryMode = InventoryUIMode.ItemBoxMode; }
                    this.LoadInventory();
                }
                break;

            default:
                break;
        }
    }

    private void UpdateSelector()
    {
        Vector3 targetPos = this._selectorTargetPosition;
        Vector2 targetSize = this._selectorTargetSize;

        switch (this._currentInventoryMode)
        {
            case InventoryUIMode.BoxesMode:
                targetPos = this._boxes[this._selectedBox].position;
                targetSize = this._selectedBox == 0 ? EQUIPBOX_SELECTOR_SIZE : ITEMBOX_SELECTOR_SIZE;
                break;

            case InventoryUIMode.EquipBoxMode: // TODO: fazer
                targetPos = this._selectorTargetPosition;
                targetSize = this._selectorTargetSize;
                break;

            case InventoryUIMode.ItemBoxMode:
                targetPos = this._itemSlots[this._selectedItemSlot].position;
                targetSize = ITEM_SELECTOR_SIZE;
                break;

            case InventoryUIMode.DialogBoxMode:
                targetPos = this._buttons[this._selectedButton].position;
                targetSize = BUTTON_SELECTOR_SIZE;
                break;
        }

        this._selectorTargetPosition = targetPos;
        this._selectorTargetSize = targetSize * 9;
        this._selectorTimer = 0f;
    }

    private void ChangeBox(int increment)
    {
        this._selectedBox += increment;

        // wrap
        if (this._selectedBox < 0) { this._selectedBox = this._boxes.Length - 1; }
        if (this._selectedBox >= this._boxes.Length) { this._selectedBox = 0; }

        this.UpdateSelector();
    }

    private int GetItemIndex()
    {
        return this._selectedItemSlot + (20 * this._selectedPage);
    }

    private void OnDiaryOpenCallback(object[] args)
    {
        this.gameObject.SetActive(false);
    }
}
