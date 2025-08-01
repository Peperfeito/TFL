using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocoDiLetraController : MinigameController
{
    [SerializeField] private GameObject fakBlocos;
    [SerializeField] private GameObject meiBlocos;
    [SerializeField] private GameObject[] bagulhoQTemQSumir;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (!this._isRunning) { return; }

        if (Input.GetKeyDown(KeyCode.Escape)) { this.DisableMinigame(); }
    }

    public override void EnableMinigame()
    {
        if (this._isComplete) { return; }

        this._minigameObject.SetActive(true);

        this._playerCamera = Camera.main;
        this._playerCamera.gameObject.SetActive(false);

        FindObjectOfType<FadeEffect>().FadeOut();

        this._isRunning = true;
    }

    public override void DisableMinigame()
    {
        this._playerCamera.gameObject.SetActive(true);
        this._minigameObject.SetActive(false);

        this._isRunning = false;
    }

    public override void CompleteMinigame()
    {
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
