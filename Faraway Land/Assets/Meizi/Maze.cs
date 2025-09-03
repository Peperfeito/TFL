using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public enum MazeStructure
{
    Wall, // BLACK;
    Path, // WHITE;
    Stairs, // RED;
    Tube,
}

public class Maze : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private Texture2D[] _mazeImages;

    private int floors;
    private int height;
    private int width;
    private MazeStructure[,,] _mazeData;

    private void Start()
    {
        this.floors = this._mazeImages.Length;
        this.height = 0;
        this.width = 0;

        for (int floorIndex = 0; floorIndex < this._mazeImages.Length; floorIndex++)
        {
            this.height = Mathf.Max(this.height, this._mazeImages[floorIndex].height);
            this.width = Mathf.Max(this.width, this._mazeImages[floorIndex].width);
        }

        this._mazeData = new MazeStructure[this.floors, this.height, this.width];

        for (int z = 0; z < this.floors; z++)
        {
            int pixelIndex = 0;
            Color[] pixels = this._mazeImages[z].GetPixels();
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    if (pixels[pixelIndex] == Color.white) { this._mazeData[z, y, x] = MazeStructure.Path; }
                    if (pixels[pixelIndex] == Color.black) { this._mazeData[z, y, x] = MazeStructure.Wall; }
                    if (pixels[pixelIndex] == Color.red) { this._mazeData[z, y, x] = MazeStructure.Stairs; }

                    pixelIndex++;
                }
            }
        }
        for (int z = 0; z < this.floors; z++)
        {
            GameObject floorParent = new GameObject($"Floor {this.floors - 1 - z}");
            floorParent.transform.parent = this.transform;
            for (int y = this.height - 1; y >= 0; y--)
            {
                for (int x = 0; x < this.width; x++)
                {
                    if (this._mazeData[z, y, x] == MazeStructure.Wall) continue;
                    
                    float xPos = x * 2f;
                    float yPos = y * 2f;
                    float zPos = (this.floors - 1 - z) * 2f;
                    
                    GameObject newBlock = GameObject.Instantiate(this._blockPrefab, new Vector3(xPos, zPos, yPos), Quaternion.identity);
                    newBlock.transform.parent = floorParent.transform;
                    
                    bool nFlag = y >= this.height - 1 || (y < this.height - 1 && this._mazeData[z, y + 1, x] == MazeStructure.Wall);
                    bool sFlag = y <= 0 || (y > 0 && this._mazeData[z, y - 1, x] == MazeStructure.Wall);
                    bool eFlag = x >= this.width - 1 || (x < this.height - 1 && this._mazeData[z, y, x + 1] == MazeStructure.Wall);
                    bool wFlag = x <= 0 || (x > 0 && this._mazeData[z, y, x - 1] == MazeStructure.Wall);
                    bool gFlag = true;
                    bool cFlag = true;

                    Transform nWall = newBlock.transform.GetChild(0);
                    Transform sWall = newBlock.transform.GetChild(1);
                    Transform eWall = newBlock.transform.GetChild(2);
                    Transform wWall = newBlock.transform.GetChild(3);
                    Transform ground = newBlock.transform.GetChild(4);
                    Transform cieling = newBlock.transform.GetChild(5);

                    nWall.gameObject.SetActive(nFlag);
                    sWall.gameObject.SetActive(sFlag);
                    eWall.gameObject.SetActive(eFlag);
                    wWall.gameObject.SetActive(wFlag);

                    switch (this._mazeData[z, y, x])
                    {
                        case MazeStructure.Stairs:

                            nWall.GetChild(0).gameObject.SetActive(nFlag);
                            sWall.GetChild(0).gameObject.SetActive(sFlag);
                            eWall.GetChild(0).gameObject.SetActive(eFlag);
                            wWall.GetChild(0).gameObject.SetActive(wFlag);

                            if (z > 0 && this._mazeData[z - 1, y, x] == MazeStructure.Stairs) cFlag = false;
                            if (z < this.floors - 1 && this._mazeData[z + 1, y, x] == MazeStructure.Stairs) gFlag = false;

                            break;
                    }

                    ground.gameObject.SetActive(gFlag);
                    cieling.gameObject.SetActive(cFlag);
                }
            }
        }
    }

    private float _positionTimer = 1f;
    private float _rotationTimer = 1f;

    Vector3 _targetPosition = Vector3.zero;
    Vector3 _facingDirection = Vector3.forward;
    Quaternion _targetRotation = Quaternion.identity;

    private void Update()
    {
        this._positionTimer += Time.deltaTime;
        if (this._positionTimer >= 1f) this._positionTimer = 1f;
        this._rotationTimer += Time.deltaTime;
        if (this._rotationTimer >= 1f) this._rotationTimer = 1f;

        // Movement
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!Physics.Raycast(this._player.transform.position, this._player.transform.forward, 2f))
            {
                this._targetPosition += this._facingDirection;
                this._positionTimer = 0f;
            }
            else if
                (
                this._mazeData[this.floors - 1 - (int)this._targetPosition.y, (int)this._targetPosition.z, (int)this._targetPosition.x] == MazeStructure.Stairs &&
                (this.floors - 1 - (int)this._targetPosition.y) > 0 &&
                this._mazeData[this.floors - 1 - (int)this._targetPosition.y - 1, (int)this._targetPosition.z, (int)this._targetPosition.x] == MazeStructure.Stairs
                )
            {
                this._targetPosition += Vector3.up;
                this._positionTimer = 0f;
            }
        }

        // Rotation
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.ChangeFacingDirection(0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            this.ChangeFacingDirection(1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if
                (
                Physics.Raycast(this._player.transform.position, this._player.transform.forward, 2f) &&
                this._mazeData[this.floors - 1 - (int)this._targetPosition.y, (int)this._targetPosition.z, (int)this._targetPosition.x] == MazeStructure.Stairs &&
                (this.floors - 1 - (int)this._targetPosition.y) < this.floors - 1 &&
                this._mazeData[this.floors - 1 - (int)this._targetPosition.y + 1, (int)this._targetPosition.z, (int)this._targetPosition.x] == MazeStructure.Stairs
                )
            {
                this._targetPosition -= Vector3.up;
                this._positionTimer = 0f;
            }
            else
            {
                this.ChangeFacingDirection(2);
            }
        }

        // Update
        this._player.transform.position = Vector3.Lerp(this._player.transform.position, Vector3.up + (this._targetPosition * 2f), this._positionTimer);
        this._player.transform.rotation = Quaternion.Lerp(this._player.transform.rotation, this._targetRotation, this._rotationTimer);
    }

    private void ChangeFacingDirection(int rotationDirection)
    {
        switch (rotationDirection)
        {
            case 0: // LEFT
                if (this._facingDirection.z != 0)
                {
                    this._facingDirection.x = -this._facingDirection.z;
                    this._facingDirection.z = 0;
                }
                else
                {
                    this._facingDirection.z = this._facingDirection.x;
                    this._facingDirection.x = 0;
                }
                break;

            case 1: // RIGHT
                if (this._facingDirection.z != 0)
                {
                    this._facingDirection.x = this._facingDirection.z;
                    this._facingDirection.z = 0;
                }
                else
                {
                    this._facingDirection.z = -this._facingDirection.x;
                    this._facingDirection.x = 0;
                }
                break;

            case 2: // BACK
                this._facingDirection *= -1;
                break;
        }

        this._targetRotation = Quaternion.Euler(Vector3.up * this.RotationAngle(this._facingDirection));
        this._rotationTimer = 0f;
    }

    private float RotationAngle(Vector3 lookDirection)
    {
        return ((Vector3.Angle(Vector3.forward, lookDirection) * Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(Vector3.forward, lookDirection)))) + 360) % 360;
    }
}
