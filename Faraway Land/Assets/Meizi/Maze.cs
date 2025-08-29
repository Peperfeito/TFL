using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [SerializeField] private GameObject _blockPrefab;

    private int[,] _maze = new int[10,10]; // TODO: ler isso de uma imagem

    private void Start()
    {
        this._maze = new int[,]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 1, 0, 0, 0, 1, 0, 0, 1, 0 },
            { 0, 1, 1, 1, 0, 1, 1, 1, 1, 0 },
            { 0, 1, 0, 1, 1, 1, 0, 0, 1, 0 },
            { 0, 1, 0, 1, 0, 1, 1, 1, 1, 0 },
            { 0, 1, 1, 1, 0, 0, 1, 0, 1, 0 },
            { 0, 1, 0, 0, 0, 0, 1, 0, 1, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        };


        for (int y = 9; y >= 0; y--)
        {
            for (int x = 0; x < 10; x++)
            {
                float xPos = x * 2f;
                float yPos = (9f - y) * 2f;

                GameObject newBlock = GameObject.Instantiate(this._blockPrefab, new Vector3(xPos, 0f, yPos), Quaternion.identity);
                newBlock.transform.parent = this.transform;

                if (this._maze[y, x] == 0) continue;

                // N
                if (y > 0 && this._maze[y - 1, x] == 1) newBlock.transform.GetChild(0).gameObject.SetActive(false);
                // S
                if (y < 9 && this._maze[y + 1, x] == 1) newBlock.transform.GetChild(1).gameObject.SetActive(false);
                // E
                if (x < 9 && this._maze[y, x + 1] == 1) newBlock.transform.GetChild(2).gameObject.SetActive(false);
                // W
                if (x > 0 && this._maze[y, x - 1] == 1) newBlock.transform.GetChild(3).gameObject.SetActive(false);
            }
        }
    }
}
