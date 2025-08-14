using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem Instance;

    [SerializeField] private GameObject _box;
    private Image _boxImage;
    [SerializeField] private Transform _charLeft;
    private Image _charLeftPic;
    private Image _charLeftNamePlate;
    private TextMeshProUGUI _charLeftName;
    private GameObject _charLeftNamePlateOverlay;
    [SerializeField] private Transform _charRight;
    private Image _charRightPic;
    private Image _charRightNamePlate;
    private TextMeshProUGUI _charRightName;
    private GameObject _charRightNamePlateOverlay;
    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField] private RectTransform _nextArrow;
    private float _arrowTimer = 0f;
    private float _arrowSpeed = 3f;
    [SerializeField] private Transform _optionBox;
    private TextMeshProUGUI[] _optionButtons;
    private int _selectedOption = 0;

    private float _inactiveHeightOffset = 27;
    private float _inactiveColorAlpha = .6f;

    private Dialog _currentDialog = null;
    private int _dialogChainIndex = 0;

    private void InitDialogSystem()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(this.gameObject);
    }

    private void Start()
    {
        this.InitDialogSystem();

        this._boxImage = this._box.GetComponent<Image>();

        this._charLeftPic = this._charLeft.GetChild(0).GetComponent<Image>();
        this._charLeftNamePlate = this._charLeft.GetChild(1).GetComponent<Image>();
        this._charLeftName = this._charLeftNamePlate.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        this._charLeftNamePlateOverlay = this._charLeft.GetChild(2).gameObject;

        this._charRightPic = this._charRight.GetChild(0).GetComponent<Image>();
        this._charRightNamePlate = this._charRight.GetChild(1).GetComponent<Image>();
        this._charRightName = this._charRightNamePlate.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        this._charRightNamePlateOverlay = this._charRight.GetChild(2).gameObject;

        this._optionButtons = new TextMeshProUGUI[this._optionBox.childCount];
        for (int i = 0; i < this._optionButtons.Length; i++)
        {
            this._optionButtons[i] = this._optionBox.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    [SerializeField] private RectTransform _seletorzinhoBemPorco;

    private void Update()
    {
        if (GameManager.Instance.currentUserInterface != UserInterfaces.Dialog) return;

        Vector2 bufferPos = this._nextArrow.anchoredPosition;

        this._arrowTimer += Time.deltaTime * this._arrowSpeed;
        if (this._arrowTimer >= 1f) { this._arrowTimer = 1f; bufferPos.y = 9f; }
        if (this._arrowTimer <= 0f) { this._arrowTimer = 0f; bufferPos.y = 0f; }
        if (this._arrowTimer >= 1f || this._arrowTimer <= 0f) { this._arrowSpeed *= -1f; }

        this._nextArrow.anchoredPosition = bufferPos;

        if (Input.GetKeyDown(KeyCode.UpArrow)) { this._selectedOption -= 1; }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { this._selectedOption += 1; }

        AnswerOptions[] marcelo = this._currentDialog.dialogChain[this._dialogChainIndex].answerOptions; // marcelo eh um buffer

        if (this._selectedOption >= marcelo.Length) this._selectedOption = 0;
        if (this._selectedOption < 0) this._selectedOption = marcelo.Length - 1;

        this._seletorzinhoBemPorco.gameObject.SetActive(this._optionBox.gameObject.activeSelf);
        this._seletorzinhoBemPorco.position = this._optionButtons[this._selectedOption].transform.position;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (this._optionBox.gameObject.activeSelf)
            {
                // salvar o indice da opcao escolhida pra mudar this._currentChain = dialogInfo.answerOptions[0].dialogChain
                Dialog nextDialog = this._currentDialog.dialogChain[this._dialogChainIndex].answerOptions[this._selectedOption].nextDialog;
                this._currentDialog = nextDialog;
                this._dialogChainIndex = -1;
                this._optionBox.gameObject.SetActive(false);
            }

            this._dialogChainIndex++;

            if (this._dialogChainIndex >= this._currentDialog.dialogChain.Length)
            {
                // desliga a caixa de dialog
                GameManager.Instance.currentUserInterface = UserInterfaces.None;
                this._box.SetActive(false);
                return;
            }

            this.UpdateDialogBox();
        }
    }

    public void DisplayDialog(Dialog dialog)
    {
        this._currentDialog = dialog;
        GameManager.Instance.currentUserInterface = UserInterfaces.Dialog;

        this._charLeft.gameObject.SetActive(false);
        this._charRight.gameObject.SetActive(false);
        this._dialogChainIndex = 0;

        this.UpdateDialogBox();
        this._box.SetActive(true);
    }

    private void UpdateDialogBox() // TODO: os personagem consegue falar com si mesmo (eu sei o que isso significa, e eu sei como eu tenho que consertar dps, kaka)
    {
        DialogInfo dialogInfo = this._currentDialog.dialogChain[this._dialogChainIndex];
        DialogCharacter speakingCharacter = this._currentDialog.characters[dialogInfo.characterIndex];

        this._boxImage.color = speakingCharacter.frameColor;
        this._textBox.text = dialogInfo.text;
        this._textBox.color = speakingCharacter.textColor;
        
        if (dialogInfo.dialogSide == DialogSide.Left)
        {
            this._charLeftName.text = speakingCharacter.characterName;
            this._charLeftName.color = speakingCharacter.textColor;
            this._charLeftNamePlate.color = speakingCharacter.frameColor;
            this._charLeftPic.sprite = speakingCharacter.characterPic;
            this._charLeftPic.color = Color.white;
            this._charLeftPic.rectTransform.anchoredPosition = Vector2.zero;
            this._charLeft.gameObject.SetActive(true);
            this._charLeftNamePlateOverlay.SetActive(false);

            if (this._charRight.gameObject.activeSelf)
            {
                this._charRightPic.color = Color.white * this._inactiveColorAlpha;
                this._charRightPic.rectTransform.anchoredPosition = Vector2.up * -this._inactiveHeightOffset;
                this._charRightNamePlateOverlay.SetActive(true);
            }
        }

        if (dialogInfo.dialogSide == DialogSide.Right)
        {
            this._charRightName.text = speakingCharacter.characterName;
            this._charRightName.color = speakingCharacter.textColor;
            this._charRightNamePlate.color = speakingCharacter.frameColor;
            this._charRightPic.sprite = speakingCharacter.characterPic;
            this._charRightPic.color = Color.white;
            this._charRightPic.rectTransform.anchoredPosition = Vector2.zero;
            this._charRight.gameObject.SetActive(true);
            this._charRightNamePlateOverlay.SetActive(false);

            if (this._charLeft.gameObject.activeSelf)
            {
                this._charLeftPic.color = Color.white * this._inactiveColorAlpha;
                this._charLeftPic.rectTransform.anchoredPosition = Vector2.up * -this._inactiveHeightOffset;
                this._charLeftNamePlateOverlay.SetActive(true);
            }
        }

        if (!dialogInfo.promptOptions) return;

        // popular as opcoes com dialogInfo.answerOptions[i].option
        for (int i = 0; i < this._optionButtons.Length; i++)
        {
            if (i >= dialogInfo.answerOptions.Length)
            {
                this._optionButtons[i].transform.parent.gameObject.SetActive(false);
                continue;
            }

            this._optionButtons[i].text = dialogInfo.answerOptions[i].option;
            this._optionButtons[i].transform.parent.gameObject.SetActive(true);
        }
        // habilitar a caixa de opcoes
        this._optionBox.gameObject.SetActive(true);
        this._selectedOption = 0;
    }
}
