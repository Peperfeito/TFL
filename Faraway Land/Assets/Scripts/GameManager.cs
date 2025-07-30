using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : IPersistentSingleton<GameManager>
{
    private MinigameController[] _minigames;

    protected override void Awake()
    {
        this._minigames = new MinigameController[(int)Minigames._COUNT];
        this._inventory = new Inventory();

        base.Awake();
    }

    private void Start()
    {
        string levelName = SceneManager.GetActiveScene().name.Replace(' ', '_');

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

        for (int i = 0; i < _minigames.Length; i++)
        {
            this._minigames[i] = null;
        }

        switch (level)
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

    /* New Inventory */

    private Inventory _inventory;
    private FuckingItemPropBaby _itemInRange;

    public void RegisterItemInRange(FuckingItemPropBaby itemProp)
    {
        this._itemInRange = itemProp; // spawnar botao de interacao ou algo assim em cima do item pa nois sabe qual que eh que ta no alcance de interacao
    }

    public void UnregisterItemInRange(FuckingItemPropBaby itemProp)
    {
        if (itemProp != this._itemInRange) return;
        this._itemInRange = null;
    }

    public void TryItemInteraction()
    {
        if (this._itemInRange == null) return;
        this.AddToInventory(this._itemInRange.ItemData);
        Destroy(this._itemInRange.gameObject);
    }

    public void AddToInventory(FuckingItemDataBaby itemData)
    {
        this._inventory.AddItem(itemData);
    }

    public void RemoveFromInventory(FuckingItemDataBaby itemData)
    {
        this._inventory.RemoveItem(itemData);
    }

    public bool HasItemInInventory(FuckingItemDataBaby itemData)
    {
        return this._inventory.HasItem(itemData);
    }

    public InventoryContent[] GetInvenotryContents()
    {
        return this._inventory.GetContents();
    }

    // Bagulho

    public bool blockPlayerMovement = false;
}
