using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour, Interactable
{
    [Tooltip("ESSE TOCA A PRIMEIRA VEZ QUE O ASH INTERAJJIIIJJJ")]
    [SerializeField] private Dialog _uniqueDialog;
    [Tooltip("ESSE TOCA TODAS AS OUTRAS VEIZ; SE isRepeatable TIVER LIGADO")]
    [SerializeField] private Dialog _repeatDialog; // TODO: transformar num arrai pra poder fazer variacoes

    [Tooltip("Se verdadeiro, precisa apertar E pra iniciar o dialool")]
    [SerializeField] private bool _interactionRequired;
    [Tooltip("Se verdadeiro, repete, faz dnv, :3")]
    [SerializeField] private bool _isRepeatable;

    private bool _consumedFlag = false;

    private void Start()
    {
        FarueiUtils.AlignWithGrid(this.transform, AlignMode.Center);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this._interactionRequired)
        {
            GameManager.Instance.RegisterInteractable(this);
            return;
        }

        this.ActivateDialog();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this._interactionRequired)
        {
            GameManager.Instance.UnregisterInteractable(this);
            return;
        }
    }

    public void ActivateDialog()
    {
        if (!this._isRepeatable && this._consumedFlag) return;
        
        if (this._consumedFlag)
        {
            DialogSystem.Instance.DisplayDialog(this._repeatDialog);
            return;
        }

        DialogSystem.Instance.DisplayDialog(this._uniqueDialog);
        this._consumedFlag = true;
    }

    public void ActivateInteraction()
    {
        this.ActivateDialog();
    }
}
