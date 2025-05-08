using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : IPersistentSingleton<GameManager>
{
    private Minigame[] _minigames;

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
        base.Awake();
    }

    public bool LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
        return true;
    }

}
