using System.Collections;
using System.Collections.Generic;
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

    [Header("Events")]
    [SerializeField] private CustomEvent OnDiaryOpen;
    [SerializeField] private CustomEvent OnReadLetter;

    [Header("Objeej")]
    [SerializeField] private Transform _diaryBackground;
    [SerializeField] private Transform _diary;
    [SerializeField] private Image _leftPage;
    private Image _lpLetter;
    private Image _lpTape;
    [SerializeField] private Image _rightPage;
    private Image _rpLetter;
    private Image _rpTape;
    [SerializeField] private Image _leftAnimPage;
    private Image _lapLetter;
    private Image _lapTape;
    [SerializeField] private Image _rightAnimPage;
    private Image _rapLetter;
    private Image _rapTape;

    [Header("Atributes")]
    [SerializeField] private DiaryLetter[] _diaryLetters;

    private void Start()
    {
        new CustomEventListener(OnDiaryOpen, OnDiaryOpenCallback);
        new CustomEventListener(OnReadLetter, OnReadLetterCallbackB);

        LetterData[] letters = Resources.LoadAll<LetterData>("LetterData");
        this._diaryLetters = new DiaryLetter[letters.Length];
        for (int i = 0; i < letters.Length; i++)
        {
            this._diaryLetters[i] = new DiaryLetter(letters[i]);
        }

        this._lpLetter = this._leftPage.transform.GetChild(0).GetComponent<Image>();
        this._lpTape = this._leftPage.transform.GetChild(1).GetComponent<Image>();

        this._rpLetter = this._rightPage.transform.GetChild(0).GetComponent<Image>();
        this._rpTape = this._rightPage.transform.GetChild(1).GetComponent<Image>();

        this._lapLetter = this._leftAnimPage.transform.GetChild(0).GetComponent<Image>();
        this._lapTape = this._leftAnimPage.transform.GetChild(1).GetComponent<Image>();

        this._rapLetter = this._rightAnimPage.transform.GetChild(0).GetComponent<Image>();
        this._rapTape = this._rightAnimPage.transform.GetChild(1).GetComponent<Image>();

        //this._leftPage.color = this._fakePages[this._selectedPage];
        this._lpLetter.sprite = this._diaryLetters[this._selectedPage].letterData.letterSprite;
        this._lpLetter.gameObject.SetActive(this._diaryLetters[this._selectedPage].obtained);
        this._lpTape.sprite = this._diaryLetters[this._selectedPage].letterData.tapeSprite;
        //this._rightPage.color = this._fakePages[this._selectedPage + 1];
        this._rpLetter.sprite = this._diaryLetters[this._selectedPage + 1].letterData.letterSprite;
        this._rpLetter.gameObject.SetActive(this._diaryLetters[this._selectedPage + 1].obtained);
        this._rpTape.sprite = this._diaryLetters[this._selectedPage + 1].letterData.tapeSprite;
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
                this._lpLetter.sprite = this._diaryLetters[this._selectedPage].letterData.letterSprite;
                this._lpLetter.gameObject.SetActive(this._diaryLetters[this._selectedPage].obtained);
                this._lpTape.sprite = this._diaryLetters[this._selectedPage].letterData.tapeSprite;

                this._rpLetter.sprite = this._diaryLetters[this._selectedPage + 1].letterData.letterSprite;
                this._rpLetter.gameObject.SetActive(this._diaryLetters[this._selectedPage + 1].obtained);
                this._rpTape.sprite = this._diaryLetters[this._selectedPage + 1].letterData.tapeSprite;

                this._pageFlip = false;
            }

            this._pageRotationTimer = 1f;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && this._pageRotationTimer >= 1f)
        {
            this._selectedPage += 2;
            if (this._selectedPage >= this._diaryLetters.Length) this._selectedPage = 0;

            //this._rightAnimPage.color = this._rightPage.color;
            this._rapLetter.sprite = this._rpLetter.sprite;
            this._rapLetter.gameObject.SetActive(this._rpLetter.gameObject.activeSelf);
            this._rapTape.sprite = this._rpTape.sprite;
            //this._leftAnimPage.color = this._fakePages[this._selectedPage];
            this._lapLetter.sprite = this._diaryLetters[this._selectedPage].letterData.letterSprite;
            this._lapLetter.gameObject.SetActive(this._diaryLetters[this._selectedPage].obtained);
            this._lapTape.sprite = this._diaryLetters[this._selectedPage].letterData.tapeSprite;
            //this._rightPage.color = this._fakePages[this._selectedPage + 1];
            this._rpLetter.sprite = this._diaryLetters[this._selectedPage + 1].letterData.letterSprite;
            this._rpLetter.gameObject.SetActive(this._diaryLetters[this._selectedPage + 1].obtained);
            this._rpTape.sprite = this._diaryLetters[this._selectedPage + 1].letterData.tapeSprite;

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
            if (this._selectedPage < 0) this._selectedPage = this._diaryLetters.Length - 2;

            //this._leftAnimPage.color = this._leftPage.color;
            this._lapLetter.sprite = this._lpLetter.sprite;
            this._lapLetter.gameObject.SetActive(this._lpLetter.gameObject.activeSelf);
            this._lapTape.sprite = this._lpTape.sprite;
            //this._rightAnimPage.color = this._fakePages[this._selectedPage + 1];
            this._rapLetter.sprite = this._diaryLetters[this._selectedPage + 1].letterData.letterSprite;
            this._rapLetter.gameObject.SetActive(this._diaryLetters[this._selectedPage + 1].obtained);
            this._rapTape.sprite = this._diaryLetters[this._selectedPage + 1].letterData.tapeSprite;
            //this._leftPage.color = this._fakePages[this._selectedPage];
            this._lpLetter.sprite = this._diaryLetters[this._selectedPage].letterData.letterSprite;
            this._lpLetter.gameObject.SetActive(this._diaryLetters[this._selectedPage].obtained);
            this._lpTape.sprite = this._diaryLetters[this._selectedPage].letterData.tapeSprite;

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

    private void AddLetter(LetterData letterData)
    {
        for (int i = 0; i < this._diaryLetters.Length; i++)
        {
            if (letterData != this._diaryLetters[i].letterData) continue;

            this._diaryLetters[i].obtained = true;
        }
    }
}
