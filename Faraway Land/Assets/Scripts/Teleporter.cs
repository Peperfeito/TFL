using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Teleporter _destination;
    [SerializeField] private PlayerLookDirection _lookDirection;

    [HideInInspector] public bool teleportEnabled = true;

    private void Start()
    {
        // Garantir que o teleporter ta alinhado na grid pq eu nao confio nos devs desse jogo kaka
        FarueiUtils.AlignWithGrid(this.transform, AlignMode.Center);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this._destination == null || !this.teleportEnabled) return;

        if (collision.CompareTag("Player"))
        {
            this._destination.teleportEnabled = false;
            GameManager.Instance.TeleportPlayer(this._destination.transform.position, this._lookDirection);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        this.teleportEnabled = true;
    }
}
