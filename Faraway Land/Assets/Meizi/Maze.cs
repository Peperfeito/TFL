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

    Origin, // GREEN;
}

public struct MazeData
{
    public MazeStructure mazeStructure;
    public Vector2 offset;

    public MazeData(MazeStructure mazeStructure, Vector2 offset)
    {
        this.mazeStructure = mazeStructure;
        this.offset = offset;
    }
}

public class Maze : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private Texture2D[] _mazeImages;

    private int floors;
    private int height;
    private int width;
    private MazeData[,,] _mazeData;

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

        this._mazeData = new MazeData[this.floors, this.height, this.width];

        // Init Maze Data
        for (int z = 0; z < this.floors; z++)
        {
            Color[] floorPixels = this._mazeImages[z].GetPixels();
            
            // Find offset
            int pixelIndex = 0;
            Vector2 floorOffset = Vector2.zero;
            for (int y = 0; y < this._mazeImages[z].height; y++)
            {
                bool found = false;
                for (int x = 0; x < this._mazeImages[z].width; x++)
                {
                    if (floorPixels[pixelIndex] == Color.green)
                    {
                        floorOffset = new Vector2(x, y);
                        found = true;
                        break;
                    }
                    pixelIndex++;
                }
                if (found) break;
            }

            // Populate others
            pixelIndex = 0;
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    if (y >= this._mazeImages[z].height || x >= this._mazeImages[z].width)
                    {
                        this._mazeData[z, y, x] = new MazeData(MazeStructure.Wall, floorOffset);
                        continue;
                    }

                    if (floorPixels[pixelIndex] == Color.white) { this._mazeData[z, y, x] = new MazeData(MazeStructure.Path, floorOffset); }
                    if (floorPixels[pixelIndex] == Color.black) { this._mazeData[z, y, x] = new MazeData(MazeStructure.Wall, floorOffset); }
                    if (floorPixels[pixelIndex] == Color.red) { this._mazeData[z, y, x] = new MazeData(MazeStructure.Stairs, floorOffset); }
                    if (floorPixels[pixelIndex] == Color.green) { this._mazeData[z, y, x] = new MazeData(MazeStructure.Origin, floorOffset); }

                    pixelIndex++;
                }
            }
        }

        // Spawn Maze Blocks
        for (int z = 0; z < this.floors; z++)
        {
            GameObject floorParent = new GameObject($"Floor {this.floors - 1 - z}");
            floorParent.transform.parent = this.transform;
            for (int y = this.height - 1; y >= 0; y--)
            {
                for (int x = 0; x < this.width; x++)
                {
                    if (this._mazeData[z, y, x].mazeStructure == MazeStructure.Wall) continue;

                    Vector2 curOffset = this._mazeData[z, y, x].offset;

                    float xPos = (x - curOffset.x) * 2f;
                    //float xPos = x * 2f;
                    float yPos = (y - curOffset.y) * 2f;
                    //float yPos = y * 2f;
                    float zPos = (this.floors - 1 - z) * 2f;
                    
                    GameObject newBlock = GameObject.Instantiate(this._blockPrefab, new Vector3(xPos, zPos, yPos), Quaternion.identity);
                    newBlock.transform.parent = floorParent.transform;
                    
                    bool nFlag = y >= this.height - 1 || (y < this.height - 1 && this._mazeData[z, y + 1, x].mazeStructure == MazeStructure.Wall);
                    bool sFlag = y <= 0 || (y > 0 && this._mazeData[z, y - 1, x].mazeStructure == MazeStructure.Wall);
                    bool eFlag = x >= this.width - 1 || (x < this.height - 1 && this._mazeData[z, y, x + 1].mazeStructure == MazeStructure.Wall);
                    bool wFlag = x <= 0 || (x > 0 && this._mazeData[z, y, x - 1].mazeStructure == MazeStructure.Wall);
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

                    switch (this._mazeData[z, y, x].mazeStructure)
                    {
                        case MazeStructure.Stairs:

                            nWall.GetChild(0).gameObject.SetActive(nFlag);
                            sWall.GetChild(0).gameObject.SetActive(sFlag);
                            eWall.GetChild(0).gameObject.SetActive(eFlag);
                            wWall.GetChild(0).gameObject.SetActive(wFlag);

                            if (z > 0)
                            {
                                Vector2 upperOffset = this._mazeData[z - 1, 0, 0].offset;
                                int upperOffsetY = y - (int)curOffset.y + (int)upperOffset.y;
                                int upperOffsetX = x - (int)curOffset.x + (int)upperOffset.x;

                                bool inRange = upperOffsetY >= 0 && upperOffsetX >= 0 && upperOffsetY <= this.height && upperOffsetX <= this.width;
                                if (inRange && this._mazeData[z - 1, upperOffsetY, upperOffsetX].mazeStructure == MazeStructure.Stairs) cFlag = false;
                            }

                            if (z < this.floors - 1)
                            {
                                Vector2 lowerOffset = this._mazeData[z + 1, 0, 0].offset;
                                int lowerOffsetY = y - (int)curOffset.y + (int)lowerOffset.y;
                                int lowerOffsetX = x - (int)curOffset.x + (int)lowerOffset.x;

                                bool inRange = lowerOffsetY >= 0 && lowerOffsetX >= 0 && lowerOffsetY <= this.height && lowerOffsetX <= this.width;
                                if (this._mazeData[z + 1, lowerOffsetY, lowerOffsetX].mazeStructure == MazeStructure.Stairs) gFlag = false;
                            }

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
            else if (this._mazeData[this.floors - 1 - (int)this._targetPosition.y, (int)this._targetPosition.z, (int)this._targetPosition.x].mazeStructure == MazeStructure.Stairs && (this.floors - 1 - (int)this._targetPosition.y) > 0)
            {
                Vector2 curOffset = this._mazeData[this.floors - 1 - (int)this._targetPosition.y, 0, 0].offset;
                Vector2 upperOffset = this._mazeData[(this.floors - 1 - (int)this._targetPosition.y) - 1, 0, 0].offset;
                int upperOffsetY = (int)this._targetPosition.z - (int)curOffset.y + (int)upperOffset.y;
                int upperOffsetX = (int)this._targetPosition.x - (int)curOffset.x + (int)upperOffset.x;

                bool inRange = upperOffsetY >= 0 && upperOffsetX >= 0 && upperOffsetY <= this.height && upperOffsetX <= this.width;
                if ( inRange && this._mazeData[this.floors - 1 - (int)this._targetPosition.y - 1, upperOffsetY, upperOffsetX].mazeStructure == MazeStructure.Stairs )
                {
                    this._targetPosition.x += -curOffset.x + upperOffset.x;
                    this._targetPosition.y += 1;
                    this._targetPosition.z += -curOffset.y + upperOffset.y;
                    this._positionTimer = 0f;
                }
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
                Physics.Raycast(this._player.transform.position, this._player.transform.forward, 2f)
                && this._mazeData[this.floors - 1 - (int)this._targetPosition.y, (int)this._targetPosition.z, (int)this._targetPosition.x].mazeStructure == MazeStructure.Stairs
                && (this.floors - 1 - (int)this._targetPosition.y) < this.floors - 1
                )
            {
                Vector2 curOffset = this._mazeData[this.floors - 1 - (int)this._targetPosition.y, 0, 0].offset;
                Vector2 lowerOffset = this._mazeData[(this.floors - 1 - (int)this._targetPosition.y) + 1, 0, 0].offset;
                int lowerOffsetY = (int)this._targetPosition.z - (int)curOffset.y + (int)lowerOffset.y;
                int lowerOffsetX = (int)this._targetPosition.x - (int)curOffset.x + (int)lowerOffset.x;

                bool inRange = lowerOffsetY >= 0 && lowerOffsetX >= 0 && lowerOffsetY <= this.height && lowerOffsetX <= this.width;
                if (inRange && this._mazeData[this.floors - 1 - (int)this._targetPosition.y + 1, lowerOffsetY, lowerOffsetX].mazeStructure == MazeStructure.Stairs)
                {
                    this._targetPosition.x += -curOffset.x + lowerOffset.x;
                    this._targetPosition.y += -1;
                    this._targetPosition.z += -curOffset.y + lowerOffset.y;
                    this._positionTimer = 0f;
                }
            }
            else
            {
                this.ChangeFacingDirection(2);
            }
        }

        // Update
        Vector3 worldPosition = this._targetPosition;
        worldPosition.x -= this._mazeData[this.floors - 1 - (int)this._targetPosition.y, 0, 0].offset.x;
        worldPosition.z -= this._mazeData[this.floors - 1 - (int)this._targetPosition.y, 0, 0].offset.y;
        this._player.transform.position = Vector3.Lerp(this._player.transform.position, Vector3.up + (worldPosition * 2f), this._positionTimer);
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
