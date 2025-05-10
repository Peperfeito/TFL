using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuScreen;
    [SerializeField] private GameObject _savesScreen;
    [SerializeField] private GameObject _creditsScreen;

    private void Start() => Cursor.visible = false;

    public void OnPlayButtonPress()
    {
        GameManager.Instance.LoadLevel("Level 01");
    }

    public void OnLoadButtonPress()
    {
        this._savesScreen.SetActive(true);
        this._mainMenuScreen.SetActive(false);
    }

    public void OnCreditsButtonPress()
    {
        this._creditsScreen.SetActive(true);
        this._mainMenuScreen.SetActive(false);
    }

    public void OnBackButtonPress()
    {
        this._mainMenuScreen.SetActive(true);
        this._creditsScreen.SetActive(false);
        this._savesScreen.SetActive(false);
    }

    public void OnQuitButtonPress() => Application.Quit();
}
