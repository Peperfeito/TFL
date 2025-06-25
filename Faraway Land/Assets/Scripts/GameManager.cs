using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : IPersistentSingleton<GameManager>
{
    private MinigameController[] _minigames;

    private Inventario _inventory;
    public Inventario Inventory
    {
        get
        {
            if (this._inventory == null) { this._inventory = GameObject.Find("Inventory").GetComponent<Inventario>(); }
            return this._inventory;
        }
    }

    protected override void Awake()
    {
        this._minigames = new MinigameController[(int)Minigames._COUNT];

        base.Awake();
    }

    private void Start()
    {
        string levelName = SceneManager.GetActiveScene().name.Replace(' ', '_');

        //for (int i = 0; i < _minigames.Length; i++)
        //{
        //    this._minigames[i] = null;
        //}

        switch (Enum.Parse(typeof(Levels), levelName))
        {
            case Levels.Level_01:
                this.InitMinigameArray((int)Minigames._LV1_START_, (int)Minigames._LV1_END_);
                break;
            case Levels.Level_02:
                this.InitMinigameArray((int)Minigames._LV2_START_, (int)Minigames._LV2_END_);
                break;
            default: break;
        }
    }

    public void LoadLevel(Levels level)
    {
        StartCoroutine(LoadLevelAsync(level));
        return;
    }

    IEnumerator LoadLevelAsync(Levels level)
    {
        string levelName = level.ToString().Replace('_', ' ');

        AsyncOperation loadedScene = SceneManager.LoadSceneAsync(levelName);

        if (!loadedScene.isDone) { yield return null; }

        for (int i = 0;  i < _minigames.Length; i++)
        {
            this._minigames[i] = null;
        }

        switch(level)
        {
            case Levels.Level_01:
                this.InitMinigameArray((int)Minigames._LV1_START_, (int)Minigames._LV1_END_);
                break;
            case Levels.Level_02:
                this.InitMinigameArray((int)Minigames._LV2_START_, (int)Minigames._LV2_END_);
                break;
            default: break;
        }
    }

    private void InitMinigameArray(int lowerBound, int upperBound)
    {
        for (int index = lowerBound + 1; index < upperBound; index++)
        {
            string currentMinigameName = ((Minigames)index).ToString();
            GameObject minigameObject = GameObject.Find(currentMinigameName);

            if (minigameObject == null || !minigameObject.TryGetComponent(out MinigameController currentMinigame)) continue;

            this._minigames[index] = currentMinigame;
        }
    }

    public void ChamaMinigame(Minigames minigame)
    {
        this._minigames[(int)minigame].EnableMinigame();
    }
}
