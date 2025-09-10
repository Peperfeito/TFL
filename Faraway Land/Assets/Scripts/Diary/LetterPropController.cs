using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPropController : MonoBehaviour, Interactable
{
    [SerializeField] private LetterData _letterData;

    [SerializeField] private CustomEvent OnReadLetter;

    private bool _letterRead = false;

    private void Start()
    {
        FarueiUtils.AlignWithGrid(this.transform, AlignMode.Center);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.RegisterInteractable(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameManager.Instance.UnregisterInteractable(this);
    }

    public void ActivateInteraction()
    {
        if (this._letterRead) return;
        this._letterRead = true;
        this.OnReadLetter.Trigger(this._letterData);
    }
}
