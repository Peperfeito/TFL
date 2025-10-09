using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocoDiLetraController : MinigameController
{
    [SerializeField] private GameObject fakBlocos;
    [SerializeField] private GameObject meiBlocos;
    [SerializeField] private GameObject[] bagulhoQTemQSumir;

    [SerializeField] private FuckingItemDataBaby _premiu;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (!this._isRunning) { return; }

        if (Input.GetKeyDown(KeyCode.Escape)) { this.DisableMinigame(); }
    }

    public override bool EnableMinigame(bool skipCondition = false)
    {
        if (!base.EnableMinigame()) return false;
        return true;
    }

    public override void DisableMinigame()
    {
        base.DisableMinigame();
    }

    public override void CompleteMinigame()
    {
        GameManager.Instance.AddToInventory(this._premiu);

        meiBlocos.SetActive(true);
        fakBlocos.SetActive(false);

        this._isComplete = true;
        this.DisableMinigame();
    }

    public void Verificar()
    {
        for (int i = 0; i < bagulhoQTemQSumir.Length; i++)
        {
            if (bagulhoQTemQSumir[i].activeSelf)
            {
                return;
            }
        }

        this.CompleteMinigame();
    }
}
