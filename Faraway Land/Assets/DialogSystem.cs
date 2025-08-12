using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem Instance;

    [SerializeField] private RectTransform _nextArrow;
    private float _arrowTimer = 0f;
    private float _arrowSpeed = 3f;

    private void InitDialogSystem()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(this.gameObject);
    }

    private void Start()
    {
        this.InitDialogSystem();
    }

    private void Update()
    {
        Vector2 bufferPos = this._nextArrow.anchoredPosition;

        this._arrowTimer += Time.deltaTime * this._arrowSpeed;
        if (this._arrowTimer >= 1f) { this._arrowTimer = 1f; bufferPos.y = 9f; }
        if (this._arrowTimer <= 0f) { this._arrowTimer = 0f; bufferPos.y = 0f; }
        if (this._arrowTimer >= 1f || this._arrowTimer <= 0f) { this._arrowSpeed *= -1f; }

        this._nextArrow.anchoredPosition = bufferPos;
    }
}
