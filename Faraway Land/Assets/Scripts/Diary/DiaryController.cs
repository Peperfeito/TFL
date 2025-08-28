using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DiaryController : MonoBehaviour
{
    [System.Serializable]
    public struct DiaryLetter
    {
        public bool obtained;
        public LetterData letterData;

        public DiaryLetter(LetterData letterData)
        {
            this.obtained = false;
            this.letterData = letterData;
        }
    }

    [System.Serializable]
    public struct DiaryEntry
    {
        public string entry;
        public Sprite drawing;

        public DiaryEntry(string entry, Sprite drawing)
        {
            this.entry = entry;
            this.drawing = drawing;
        }
    }

    [SerializeField] private List<DiaryEntry> _diaryEntries;

    [Header("Events")]
    [SerializeField] private CustomEvent OnDiaryOpen;
    [SerializeField] private CustomEvent OnReadLetter;
    [SerializeField] private CustomEvent OnDialogLog;

    [Header("Objeej")]
    [SerializeField] private Transform _diaryBackground;
    [SerializeField] private Transform _diary;
    [SerializeField] private Image _leftPage;
    private Image _lpLetter;
    private Image _lpTape;
    private TextMeshProUGUI _lpText;
    private Image _lpDrawing;
    [SerializeField] private Image _rightPage;
    private Image _rpLetter;
    private Image _rpTape;
    private TextMeshProUGUI _rpText;
    private Image _rpDrawing;
    [SerializeField] private Image _leftAnimPage;
    private Image _lapLetter;
    private Image _lapTape;
    private TextMeshProUGUI _lapText;
    private Image _lapDrawing;
    [SerializeField] private Image _rightAnimPage;
    private Image _rapLetter;
    private Image _rapTape;
    private TextMeshProUGUI _rapText;
    private Image _rapDrawing;

    // 0 - L; 1 - R; 2 - AL; 3 - AR
    private Image[] _pageLetter;
    private Image[] _pageTape;
    private TextMeshProUGUI[] _pageText;
    private Image[] _pageDrawing;

    [Header("Atributes")]
    [SerializeField] private DiaryLetter[] _diaryLetters;

    private void Start()
    {
        new CustomEventListener(OnDiaryOpen, OnDiaryOpenCallback);
        new CustomEventListener(OnReadLetter, OnReadLetterCallbackB);
        new CustomEventListener(OnDialogLog, OnDialogLogCallback);

        LetterData[] letters = Resources.LoadAll<LetterData>("LetterData");
        this._diaryLetters = new DiaryLetter[letters.Length];
        for (int i = 0; i < letters.Length; i++)
        {
            this._diaryLetters[i] = new DiaryLetter(letters[i]);
        }

        //this._diaryEntries = new List<DiaryEntry>();

        this._lpLetter = this._leftPage.transform.GetChild(0).GetComponent<Image>();
        this._lpTape = this._leftPage.transform.GetChild(1).GetComponent<Image>();
        this._lpText = this._leftPage.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        this._lpDrawing = this._leftPage.transform.GetChild(3).GetComponent<Image>();

        this._rpLetter = this._rightPage.transform.GetChild(0).GetComponent<Image>();
        this._rpTape = this._rightPage.transform.GetChild(1).GetComponent<Image>();
        this._rpText = this._rightPage.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        this._rpDrawing = this._rightPage.transform.GetChild(3).GetComponent<Image>();

        this._lapLetter = this._leftAnimPage.transform.GetChild(0).GetComponent<Image>();
        this._lapTape = this._leftAnimPage.transform.GetChild(1).GetComponent<Image>();
        this._lapText = this._leftAnimPage.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        this._lapDrawing = this._leftAnimPage.transform.GetChild(3).GetComponent<Image>();

        this._rapLetter = this._rightAnimPage.transform.GetChild(0).GetComponent<Image>();
        this._rapTape = this._rightAnimPage.transform.GetChild(1).GetComponent<Image>();
        this._rapText = this._rightAnimPage.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        this._rapDrawing = this._rightAnimPage.transform.GetChild(3).GetComponent<Image>();

        this._pageLetter = new[] { this._lpLetter, this._rpLetter, this._lapLetter, this._rapLetter };
        this._pageTape = new[] { this._lpTape, this._rpTape, this._lapTape, this._rapTape };
        this._pageText= new[] { this._lpText, this._rpText, this._lapText, this._rapText };
        this._pageDrawing = new[] { this._lpDrawing, this._rpDrawing, this._lapDrawing, this._rapDrawing };

        //this._leftPage.color = this._fakePages[this._selectedPage];
        this._lpLetter.sprite = this._diaryLetters[this._selectedPage].letterData.letterSprite;
        this._lpLetter.gameObject.SetActive(this._diaryLetters[this._selectedPage].obtained);
        this._lpTape.sprite = this._diaryLetters[this._selectedPage].letterData.tapeSprite;
        this._lpText.text = string.Empty;
        this._lpDrawing.gameObject.SetActive(false);
        
        //this._rightPage.color = this._fakePages[this._selectedPage + 1];
        this._rpLetter.sprite = this._diaryLetters[this._selectedPage + 1].letterData.letterSprite;
        this._rpLetter.gameObject.SetActive(this._diaryLetters[this._selectedPage + 1].obtained);
        this._rpTape.sprite = this._diaryLetters[this._selectedPage + 1].letterData.tapeSprite;
        this._rpText.text = string.Empty;
        this._rpDrawing.gameObject.SetActive(false);
    }

    Quaternion _targetLeftRotation = Quaternion.identity;
    Quaternion _targetRightRotation = Quaternion.identity;
    private float _pageRotationTimer = 0;

    private int _selectedPage = 0;

    private bool _pageFlip = false;

    private void Update()
    {
        this._pageRotationTimer += Time.deltaTime / 3f;
        if (this._pageRotationTimer >= 1f)
        {
            if (this._pageFlip)
            {
                this._pageFlip = false;

                if (this._selectedPage < this._diaryLetters.Length)
                {
                    this._pageLetter[0].sprite = this._diaryLetters[this._selectedPage].letterData.letterSprite;
                    this._pageTape[0].sprite = this._diaryLetters[this._selectedPage].letterData.tapeSprite;
                }
                else if (this._selectedPage - this._diaryLetters.Length < this._diaryEntries.Count)
                {
                    this._pageText[0].text = this._diaryEntries[this._selectedPage - this._diaryLetters.Length].entry;
                    this._pageDrawing[0].sprite = this._diaryEntries[this._selectedPage - this._diaryLetters.Length].drawing;
                }
                this._pageLetter[0].gameObject.SetActive(this._selectedPage < this._diaryLetters.Length && this._diaryLetters[this._selectedPage].obtained);
                this._pageTape[0].gameObject.SetActive(this._selectedPage < this._diaryLetters.Length);
                this._pageText[0].gameObject.SetActive(this._selectedPage >= this._diaryLetters.Length && this._selectedPage - this._diaryLetters.Length < this._diaryEntries.Count);
                this._pageDrawing[0].gameObject.SetActive(this._selectedPage >= this._diaryLetters.Length && this._selectedPage - this._diaryLetters.Length < this._diaryEntries.Count);

                if (this._selectedPage + 1 < this._diaryLetters.Length)
                {
                    this._pageLetter[1].sprite = this._diaryLetters[this._selectedPage + 1].letterData.letterSprite;
                    this._pageTape[1].sprite = this._diaryLetters[this._selectedPage + 1].letterData.tapeSprite;
                }
                else if (this._selectedPage + 1 - this._diaryLetters.Length < this._diaryEntries.Count)
                {
                    this._pageText[1].text = this._diaryEntries[this._selectedPage + 1 - this._diaryLetters.Length].entry;
                    this._pageDrawing[1].sprite = this._diaryEntries[this._selectedPage + 1 - this._diaryLetters.Length].drawing;
                }
                this._pageLetter[1].gameObject.SetActive(this._selectedPage + 1 < this._diaryLetters.Length && this._diaryLetters[this._selectedPage + 1].obtained);
                this._pageTape[1].gameObject.SetActive(this._selectedPage + 1 < this._diaryLetters.Length);
                this._pageText[1].gameObject.SetActive(this._selectedPage + 1 >= this._diaryLetters.Length && this._selectedPage + 1 - this._diaryLetters.Length < this._diaryEntries.Count);
                this._pageDrawing[1].gameObject.SetActive(this._selectedPage + 1 >= this._diaryLetters.Length && this._selectedPage + 1 - this._diaryLetters.Length < this._diaryEntries.Count);
            }

            this._pageRotationTimer = 1f;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && this._pageRotationTimer >= 1f)
        {
            this._selectedPage += 2;
            if (this._selectedPage >= this._diaryLetters.Length + this._diaryEntries.Count) this._selectedPage = 0;

            this.FlipPage(1);

            this._leftAnimPage.transform.localRotation = Quaternion.Euler(0f, 181f, 0f);
            this._targetLeftRotation = Quaternion.Euler(Vector3.up * 359f);

            this._rightAnimPage.transform.localRotation = Quaternion.Euler(0f, 1f, 0f);
            this._targetRightRotation = Quaternion.Euler(Vector3.up * 179f);

            this._pageRotationTimer = 0f;
            this._pageFlip = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && this._pageRotationTimer >= 1f)
        {
            this._selectedPage -= 2;
            if (this._selectedPage < 0) this._selectedPage = this._diaryLetters.Length + this._diaryEntries.Count - 2 + ((this._diaryLetters.Length + this._diaryEntries.Count) % 2);

            this.FlipPage(0);

            this._leftAnimPage.transform.localRotation = Quaternion.Euler(0f, 359f, 0f);
            this._targetLeftRotation = Quaternion.Euler(Vector3.up * 181f);

            this._rightAnimPage.transform.localRotation = Quaternion.Euler(0f, 179f, 0f);
            this._targetRightRotation = Quaternion.Euler(Vector3.up * 1f);

            this._pageRotationTimer = 0f;
            this._pageFlip = true;
        }

        this._leftAnimPage.transform.localRotation = Quaternion.Lerp(this._leftAnimPage.transform.localRotation, this._targetLeftRotation, this._pageRotationTimer);
        this._rightAnimPage.transform.localRotation = Quaternion.Lerp(this._rightAnimPage.transform.localRotation, this._targetRightRotation, this._pageRotationTimer);

        this._leftAnimPage.gameObject.SetActive(this._leftAnimPage.transform.localRotation.eulerAngles.y > 270f && this._pageRotationTimer < 1f);
        this._rightAnimPage.gameObject.SetActive(this._rightAnimPage.transform.localRotation.eulerAngles.y < 90f && this._pageRotationTimer < 1f);

        bool lFlag = Mathf.Abs(this._leftAnimPage.transform.localRotation.eulerAngles.y - this._targetLeftRotation.eulerAngles.y) < .5f;
        bool rFlag = Mathf.Abs(this._rightAnimPage.transform.localRotation.eulerAngles.y - this._targetRightRotation.eulerAngles.y) < .5f;

        if (lFlag && rFlag) this._pageRotationTimer = 1f;

        if (GameManager.Instance.currentUserInterface != UserInterfaces.Diary) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.currentUserInterface = UserInterfaces.None;
            this._diaryBackground.gameObject.SetActive(false);
            this._diary.gameObject.SetActive(false);
        }
    }

    private void OnDiaryOpenCallback(object[] args)
    {
        GameManager.Instance.currentUserInterface = UserInterfaces.Diary;
        this._diaryBackground.gameObject.SetActive(true);
        this._diary.gameObject.SetActive(true);
    }

    private void OnReadLetterCallbackB(object[] args)
    {
        if (args.Length <= 0) return;

        for (int i = 0; i < args.Length; i++)
        {
            LetterData letterData = (LetterData)args[i];
            if (letterData == null) continue;
            this.AddLetter(letterData);
            break;
        }
    }

    private void OnDialogLogCallback(object[] args)
    {
        if (args.Length <= 0) return;

        for (int i = 0; i < args.Length; i++)
        {
            Dialog dialogData = (Dialog)args[i];
            if (dialogData == null) continue;
            this.LogDialog(dialogData);
            break;
        }
    }

    private void AddLetter(LetterData letterData)
    {
        for (int i = 0; i < this._diaryLetters.Length; i++)
        {
            if (letterData != this._diaryLetters[i].letterData) continue;

            this._diaryLetters[i].obtained = true;
        }
    }

    private void LogDialog(Dialog dialogData)
    {
        DiaryEntry newEntry = new DiaryEntry(dialogData.logEntry, dialogData.logDrawing);
        this._diaryEntries.Add(newEntry);
    }

    private void FlipPage(int clonedPage) // 0 - left; 1 - right
    {
        int pageIndex = clonedPage;
        int otherIndex = 1 - clonedPage;
        int animIndex = clonedPage + 2;
        int otherAnimIndex = 3 - clonedPage;

        int selectedPageIndex = this._selectedPage + pageIndex;
        int otherSelectedPageIndex = this._selectedPage + otherIndex;

        // Save current CLONED into ANIM CLONED;
        this._pageLetter[animIndex].sprite = this._pageLetter[pageIndex].sprite;
        this._pageLetter[animIndex].gameObject.SetActive(this._pageLetter[pageIndex].gameObject.activeSelf);
        
        this._pageTape[animIndex].sprite = this._pageTape[pageIndex].sprite;
        this._pageTape[animIndex].gameObject.SetActive(this._pageTape[pageIndex].gameObject.activeSelf);

        this._pageText[animIndex].text = this._pageText[pageIndex].text;
        this._pageText[animIndex].gameObject.SetActive(this._pageText[pageIndex].gameObject.activeSelf);

        this._pageDrawing[animIndex].sprite = this._pageDrawing[pageIndex].sprite;
        this._pageDrawing[animIndex].gameObject.SetActive(this._pageDrawing[pageIndex].gameObject.activeSelf);

        // Load next OTHER into ANIM OTHER;
        if (otherSelectedPageIndex < this._diaryLetters.Length)
        {
            this._pageLetter[otherAnimIndex].sprite = this._diaryLetters[otherSelectedPageIndex].letterData.letterSprite;
            this._pageTape[otherAnimIndex].sprite = this._diaryLetters[otherSelectedPageIndex].letterData.tapeSprite;
        }
        else if (otherSelectedPageIndex - this._diaryLetters.Length < this._diaryEntries.Count)
        {
            this._pageText[otherAnimIndex].text = this._diaryEntries[otherSelectedPageIndex - this._diaryLetters.Length].entry;
            this._pageDrawing[otherAnimIndex].sprite = this._diaryEntries[otherSelectedPageIndex - this._diaryLetters.Length].drawing;
        }
        this._pageLetter[otherAnimIndex].gameObject.SetActive(otherSelectedPageIndex < this._diaryLetters.Length && this._diaryLetters[otherSelectedPageIndex].obtained);
        this._pageTape[otherAnimIndex].gameObject.SetActive(otherSelectedPageIndex < this._diaryLetters.Length);
        this._pageText[otherAnimIndex].gameObject.SetActive(otherSelectedPageIndex >= this._diaryLetters.Length && otherSelectedPageIndex - this._diaryLetters.Length < this._diaryEntries.Count);
        this._pageDrawing[otherAnimIndex].gameObject.SetActive(otherSelectedPageIndex >= this._diaryLetters.Length && otherSelectedPageIndex - this._diaryLetters.Length < this._diaryEntries.Count);
        
        // Load next CLONED into CLONED;
        if (selectedPageIndex < this._diaryLetters.Length)
        {
            this._pageLetter[pageIndex].sprite = this._diaryLetters[selectedPageIndex].letterData.letterSprite;
            this._pageTape[pageIndex].sprite = this._diaryLetters[selectedPageIndex].letterData.tapeSprite;
        }
        else if (selectedPageIndex - this._diaryLetters.Length < this._diaryEntries.Count)
        {
            this._pageText[pageIndex].text = this._diaryEntries[selectedPageIndex - this._diaryLetters.Length].entry;
            this._pageDrawing[pageIndex].sprite = this._diaryEntries[selectedPageIndex - this._diaryLetters.Length].drawing;
        }
        this._pageLetter[pageIndex].gameObject.SetActive(selectedPageIndex < this._diaryLetters.Length && this._diaryLetters[selectedPageIndex].obtained);
        this._pageTape[pageIndex].gameObject.SetActive(selectedPageIndex < this._diaryLetters.Length);
        this._pageText[pageIndex].gameObject.SetActive(selectedPageIndex >= this._diaryLetters.Length && selectedPageIndex - this._diaryLetters.Length < this._diaryEntries.Count);
        this._pageDrawing[pageIndex].gameObject.SetActive(selectedPageIndex >= this._diaryLetters.Length && selectedPageIndex - this._diaryLetters.Length < this._diaryEntries.Count);
    }
}
