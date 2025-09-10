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

    public virtual void EnableMinigame() { }
    public virtual void DisableMinigame() { }
    public virtual void CompleteMinigame() { }
}
