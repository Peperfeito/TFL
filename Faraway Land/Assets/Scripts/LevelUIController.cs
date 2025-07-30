using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUIController : MonoBehaviour
{
    private EventSystem _eventSystem;
    [Header("Dialog")]
    //[SerializeField] private GameObject _dialogBox;
    //[SerializeField] private Image _dialogProfilePic;
    //[SerializeField] private TextMeshProUGUI _dialogText;
    //[SerializeField] private Button _dialogPositiveButton;
    //[SerializeField] private Button _dialogNegativeButton;
    
    //public GameObject DialogBox { get { return _dialogBox; } } // PH

    //private TextMeshProUGUI _dialogPositiveButtonText;
    //private TextMeshProUGUI _dialogNegativeButtonText;

    //private Sprite _lastDialogSprite;

    //private DialogBoxMode _modeBuffer;

    [SerializeField] private InventoryUIController _inventoryUIController;

    private void Start()
    {
        this._eventSystem = EventSystem.current;

        //this._dialogPositiveButtonText = this._dialogPositiveButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //this._dialogNegativeButtonText = this._dialogNegativeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this._inventoryUIController.gameObject.SetActive(!this._inventoryUIController.gameObject.activeSelf);
            GameManager.Instance.blockPlayerMovement = this._inventoryUIController.gameObject.activeSelf;
            if (this._inventoryUIController.gameObject.activeSelf) { this._inventoryUIController.LoadItems(); }
        }
    }

    //private void SetButtonState(bool bothActive, string positiveText = "SIM", string negativeText = "NAO")
    //{
    //    this._dialogPositiveButtonText.text = positiveText;
    //    this._dialogNegativeButtonText.text = negativeText;

    //    this._dialogPositiveButton.gameObject.SetActive(true);
    //    this._dialogNegativeButton.gameObject.SetActive(bothActive);
    //}

    //public void UpdateDialogBox(DialogBoxMode mode, Sprite profilePic, string text = "")
    //{
    //    if (text == string.Empty)
    //    {
    //        this._dialogBox.SetActive(false);
    //        return;
    //    }

    //    switch (mode)
    //    {
    //        case DialogBoxMode.Default: this.SetButtonState(false, "PROXIMO"); break;
    //        case DialogBoxMode.ItemInteraction: this.SetButtonState(true, "PEGAR", "DEIXAR"); break;
    //        case DialogBoxMode.ObjectInteraction: this.SetButtonState(true, "USAR", "SAIR"); break;
    //    }

    //    this._dialogProfilePic.sprite = profilePic;
    //    this._dialogText.text = text;

    //    if (!this._dialogBox.activeSelf) { this._dialogBox.SetActive(true); }

    //    this._lastDialogSprite = profilePic;
    //    this._modeBuffer = mode;

    //    this._eventSystem.firstSelectedGameObject = this._dialogPositiveButton.gameObject;
    //}
}
