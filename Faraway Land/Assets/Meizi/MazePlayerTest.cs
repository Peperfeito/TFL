using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePlayerTest : MonoBehaviour
{
    private float _positionTimer = 1f;
    private float _rotationTimer = 1f;

    Vector3 _targetPosition = Vector3.up;
    Quaternion _targetRotation = Quaternion.identity;

    private void Update()
    {
        this._positionTimer += Time.deltaTime;
        if (this._positionTimer >= 1f) this._positionTimer = 1f;
        this._rotationTimer += Time.deltaTime;
        if (this._rotationTimer >= 1f) this._rotationTimer = 1f;

        // Rigid Movement
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _targetPosition = this.transform.position + this.transform.forward * 2;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector3 rot = this.transform.rotation.eulerAngles;
            rot.y += 270f;
            this._targetRotation = Quaternion.Euler(rot);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3 rot = this.transform.rotation.eulerAngles;
            rot.y += 90f;
            this._targetRotation = Quaternion.Euler(rot);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector3 rot = this.transform.rotation.eulerAngles;
            rot.y += 180f;
            this._targetRotation = Quaternion.Euler(rot);
        }

        // Smooth Movement
        if (Input.GetKeyDown(KeyCode.W))
        {
            _targetPosition = this.transform.position + this.transform.forward * 2;
            this._positionTimer = 0f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 rot = this.transform.rotation.eulerAngles;
            rot.y += 270f;
            this._targetRotation = Quaternion.Euler(rot);
            this._rotationTimer = 0f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 rot = this.transform.rotation.eulerAngles;
            rot.y += 90f;
            this._targetRotation = Quaternion.Euler(rot);
            this._rotationTimer = 0f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 rot = this.transform.rotation.eulerAngles;
            rot.y += 180f;
            this._targetRotation = Quaternion.Euler(rot);
            this._rotationTimer = 0f;
        }

        // Movement Update
        this.transform.position = Vector3.Lerp(this.transform.position, this._targetPosition, this._positionTimer);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this._targetRotation, this._rotationTimer);
    }
}
