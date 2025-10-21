using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameController : MonoBehaviour
{
    [SerializeField] protected Camera _minigameCamera;
    protected Camera _playerCamera;
    protected bool _isRunning = false;
    protected bool _isComplete = false;
    
    protected GameObject _minigameObject;

    protected virtual void Start()
    {
        this._minigameObject = this.transform.GetChild(0).gameObject;
    }

    public virtual bool EnableMinigame(bool skipCondition = false)
    {
        if (this._isComplete || skipCondition) return false;

        GameManager.Instance.currentUserInterface = UserInterfaces.Minigame;

        this._minigameObject.SetActive(true);

        this._playerCamera = Camera.main;
        this._playerCamera.gameObject.SetActive(false);

        FindObjectOfType<FadeEffect>().FadeOut();

        this._isRunning = true;

        return true;
    }

    public virtual void DisableMinigame()
    {
        GameManager.Instance.currentUserInterface = UserInterfaces.None;

        this._playerCamera.gameObject.SetActive(true);
        this._minigameObject.SetActive(false);

        this._isRunning = false;
    }

    public virtual void CompleteMinigame()
    {

    }
}
