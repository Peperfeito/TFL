using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : IPersistentSingleton<GameManager>
{
    private Minigame[] _minigames;

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
