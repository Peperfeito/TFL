using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [Header("Janaoseimais")]
    [SerializeField] private Transform _diary;

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
    }

    private void Update()
    {
        if (GameManager.Instance.currentUserInterface != UserInterfaces.Diary) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.currentUserInterface = UserInterfaces.None;
            this._diary.gameObject.SetActive(false);
        }
    }

    private void OnDiaryOpenCallback(object[] args)
    {
        GameManager.Instance.currentUserInterface = UserInterfaces.Diary;
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
