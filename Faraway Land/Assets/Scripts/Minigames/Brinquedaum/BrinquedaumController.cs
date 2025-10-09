using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrinquedaumController : MinigameController
{
    private Maze _theMaze;

    protected override void Start()
    {
        base.Start();
    }

    public override bool EnableMinigame(bool skipCondition = false)
    {
        if (!base.EnableMinigame()) return false;
        
        if (this._theMaze == null) this._theMaze = this._minigameObject.transform.GetChild(0).GetComponent<Maze>();
        this._theMaze.InitMaze();
        
        return true;
    }

    public override void DisableMinigame()
    {
        base.DisableMinigame();
    }

    public override void CompleteMinigame()
    {
        this._isComplete = true;
        this.DisableMinigame();
    }
}
