using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : IPersistentSingleton<GameManager>
{
    [SerializeField] private FuckingItemDataBaby _diary;

    private MinigameController[] _minigames;

    public double playerSeed;

    protected override void Awake()
    {
        base.Awake();

        this._minigames = new MinigameController[(int)Minigames._COUNT];
        this._inventory = new Inventory();
        this._inventory.AddItem(this._diary);

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

    // Levels

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

    // Minigames

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

    /* Interactable */

    private Interactable _interactableInRange;

    public void RegisterInteractable(Interactable interactable)
    {
        this._interactableInRange = interactable; // pop up do botao kaka, ou nao, sla, o moon decide dps, akakaka jiftsakujhf gdskajyhgf dsauhkj fdsj
    }

    public void UnregisterInteractable(Interactable interactable)
    {
        if (interactable != this._interactableInRange) return;
        this._interactableInRange = null;
    }

    public void TryInteractableInteraction()
    {
        if (this._interactableInRange == null) return;
        this._interactableInRange.ActivateInteraction();
    }

    /* New Inventory */

    private Inventory _inventory;

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

    public bool UseItem(FuckingItemDataBaby itemData)
    {
        return this._inventory.UseItem(itemData);
    }

    public bool LickItem(FuckingItemDataBaby itemData)
    {
        return this._inventory.LickItem(itemData);
    }

    public InventoryContent[] GetInvenotryContents()
    {
        return this._inventory.GetContents();
    }

    // Player


    private GameObject _movePoint;
    private GameObject _player;
    private GameObject _playerCamera;

    public void TeleportPlayer(Vector3 destination, PlayerLookDirection lookDirection = PlayerLookDirection.Down) // saber qual player ta ativado pra teleportar, caso necessario
    {
        if (this._movePoint == null) this._movePoint = GameObject.Find("MovePoint");
        this._movePoint.transform.position = destination;

        if (this._player == null) this._player = GameObject.Find("GridPlayer");
        this._player.transform.position = destination;
        this._player.GetComponent<PlayerGrid>().LookTowards(lookDirection);

        if (this._playerCamera == null) this._playerCamera = Camera.main.gameObject;
        this._playerCamera.transform.position = destination;

        FindObjectOfType<FadeEffect>().FadeOut();
    }

    /* Interafaces */

    public UserInterfaces currentUserInterface = UserInterfaces.None;
}
