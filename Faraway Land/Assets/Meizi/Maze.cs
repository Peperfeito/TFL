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
    Tube, // BLUE;

    Triangles, // YELLOW
    Sacks, // CYAN
    Spinner, // MAGENTA

    Origin, // GREEN;
}

public struct MazeData // TODO: talvez tirar maze data, pq ta meio que inutil agr, mas vai que vamo precisar mais pra frente?
{
    public MazeStructure mazeStructure;

    public MazeData(MazeStructure mazeStructure)
    {
        this.mazeStructure = mazeStructure;
    }
}

public class Maze : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private Texture2D[] _mazeImages;

    [SerializeField] private GameObject _triangleTest;

    // Basic type
    private readonly Color WALL_COLOR = new Color(0f, 0f, 0f); // #000000
    private readonly Color PATH_COLOR = new Color(1f, 1f, 1f); // #FFFFFF
    // Stair type
    private readonly Color STAIRS_COLOR = new Color(1f, 0f, 0f); // #FF0000
    private readonly Color TUBE_COLOR = new Color(0f, 0f, 1f); // #0000FF
    // Structure type
    private readonly Color TRIANGLES_COLOR = new Color(1f, 1f, 0f); // #FFFF00
    private readonly Color SACKS_COLOR = new Color(0f, 1f, 1f); // #00FFFF
    private readonly Color SPINNER_COLOR = new Color(1f, 0f, 1f); // #FF00FF
    // Origin
    private readonly Color ORIGIN_COLOR = new Color(0f, 1f, 0f); // #00FF00

    private int floors;
    private int height;
    private int width;
    private MazeData[,,] _mazeData;

    private int maxLeft;
    private int maxRight;
    private int maxTop;
    private int maxBottom;

    private void Start()
    {
        this.floors = this._mazeImages.Length;
        this.height = 0;
        this.width = 0;

        this.maxLeft = 0;
        this.maxRight = 0;
        this.maxTop = 0;
        this.maxBottom = 0;

        // Find the matrix size
        for (int z = 0; z < this.floors; z++)
        {
            Color[] floorPixels = this._mazeImages[z].GetPixels();
            int pixelIndex = 0;

            for (int y = 0; y < this._mazeImages[z].height; y++)
            {
                bool found = false;
                for (int x = 0; x < this._mazeImages[z].width; x++)
                {
                    if (floorPixels[pixelIndex] == ORIGIN_COLOR)
                    {
                        this.maxLeft = Mathf.Max(this.maxLeft, x);
                        this.maxRight = Mathf.Max(this.maxRight, this._mazeImages[z].width - x);

                        this.maxBottom = Mathf.Max(this.maxBottom, y);
                        this.maxTop = Mathf.Max(this.maxTop, this._mazeImages[z].height - y);

                        found = true;
                        break;
                    }
                    pixelIndex++;
                }
                if (found) break;
            }
        }

        this.height = this.maxBottom + this.maxTop;
        this.width = this.maxLeft + this.maxRight;

        Debug.Log(this.height);
        Debug.Log(this.width);

        this._mazeData = new MazeData[this.floors, this.height, this.width];

        // Init Maze Data
        for (int z = 0; z < this.floors; z++)
        {
            Color[] floorPixels = this._mazeImages[z].GetPixels();
            int pixelIndex = 0;
            Vector2 floorOffset = Vector2.zero;

            // Find floor offset
            for (int y = 0; y < this._mazeImages[z].height; y++)
            {
                bool found = false;
                for (int x = 0; x < this._mazeImages[z].width; x++)
                {
                    if (floorPixels[pixelIndex] == ORIGIN_COLOR)
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

            int minY = 0 + this.maxBottom - (int)floorOffset.y;
            int maxY = (this._mazeImages[z].height - 1) + this.maxBottom - (int)floorOffset.y;
            int minX = 0 + this.maxLeft - (int)floorOffset.x;
            int maxX = (this._mazeImages[z].width - 1) + this.maxLeft - (int)floorOffset.x;

            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    if (y < minY || y > maxY || x < minX || x > maxX)
                    {
                        this._mazeData[z, y, x] = new MazeData(MazeStructure.Wall);
                        continue;
                    }

                    if (floorPixels[pixelIndex] == PATH_COLOR) { this._mazeData[z, y, x] = new MazeData(MazeStructure.Path); }
                    if (floorPixels[pixelIndex] == WALL_COLOR) { this._mazeData[z, y, x] = new MazeData(MazeStructure.Wall); }

                    if (floorPixels[pixelIndex] == STAIRS_COLOR) { this._mazeData[z, y, x] = new MazeData(MazeStructure.Stairs); }

                    if (floorPixels[pixelIndex] == TRIANGLES_COLOR) { this._mazeData[z, y, x] = new MazeData(MazeStructure.Triangles); }

                    if (floorPixels[pixelIndex] == ORIGIN_COLOR) { this._mazeData[z, y, x] = new MazeData(MazeStructure.Origin); }

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

                    float xPos = x * 2f;
                    //float xPos = x * 2f;
                    float yPos = y * 2f;
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

                            if (z > 0 && this._mazeData[z - 1, y, x].mazeStructure == MazeStructure.Stairs) { cFlag = false; }
                            if (z < this.floors - 1 && this._mazeData[z + 1, y, x].mazeStructure == MazeStructure.Stairs) { gFlag = false; }

                            break;

                        case MazeStructure.Triangles:

                            Quaternion rotation = Quaternion.identity; // olha em direcao ao Z (padrao)

                            if (
                                ((y > 0 && this._mazeData[z, y - 1, x].mazeStructure == MazeStructure.Wall) && (y < this.height - 1 && this._mazeData[z, y + 1, x].mazeStructure == MazeStructure.Wall))
                                || (y <= 0 && (y < this.height - 1 && this._mazeData[z, y + 1, x].mazeStructure == MazeStructure.Wall))
                                || (y >= this.height - 1 && (y > 0 && this._mazeData[z, y - 1, x].mazeStructure == MazeStructure.Wall))
                                )
                            {
                                // olha em direcao ao X
                                rotation = Quaternion.Euler(Vector3.up * 90f);
                            }

                            GameObject newPiece = GameObject.Instantiate(this._triangleTest, newBlock.transform.position, rotation);
                            newPiece.transform.parent = newBlock.transform;

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
                this._mazeData[this.floors - 1 - (int)this._targetPosition.y, (int)this._targetPosition.z, (int)this._targetPosition.x].mazeStructure == MazeStructure.Stairs
                && (this.floors - 1 - (int)this._targetPosition.y) > 0
                && this._mazeData[this.floors - 1 - (int)this._targetPosition.y - 1, (int)this._targetPosition.z, (int)this._targetPosition.x].mazeStructure == MazeStructure.Stairs
                )
            {
                this._targetPosition.y += 1;
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
                Physics.Raycast(this._player.transform.position, this._player.transform.forward, 2f)
                && this._mazeData[this.floors - 1 - (int)this._targetPosition.y, (int)this._targetPosition.z, (int)this._targetPosition.x].mazeStructure == MazeStructure.Stairs
                && (this.floors - 1 - (int)this._targetPosition.y) < this.floors - 1
                && this._mazeData[this.floors - 1 - (int)this._targetPosition.y + 1, (int)this._targetPosition.z, (int)this._targetPosition.x].mazeStructure == MazeStructure.Stairs
                )
            {
                this._targetPosition.y += -1;
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
