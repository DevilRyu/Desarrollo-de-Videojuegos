using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBoard : MonoBehaviour
{
    public Transform SpawnPoint;
    public TetrisBlock Prefab;

    public Transform[,] Grid;

    public bool IsCellAvailable(int row, int col) => Grid[row, col] == null;

    readonly int Rows = 20;
    readonly int Cols = 10;

    void Awake()
    {
        Grid = new Transform[Rows, Cols];
    }

    void Start()
    {
        InstantiateTetrisBlock();
    }

    public void InstantiateTetrisBlock()
    {
        var tetrisBlock = GameObject.Instantiate(Prefab,
                                                 SpawnPoint.position,
                                                 Quaternion.identity);
        tetrisBlock.Board = this;
    }

    public void AddBlockToGrid(TetrisBlock tetrisBlock)
    {
        var tetrisBlockTrans = tetrisBlock.transform;
        for (int i = 0; i < tetrisBlockTrans.childCount; i++)
        {
            var childTrans = tetrisBlockTrans.GetChild(i);
            int x = Mathf.RoundToInt(childTrans.position.x);
            int y = Mathf.RoundToInt(childTrans.position.y);
            var row = y;
            var col = x;
            Grid[row, col] = childTrans;
        }
    }

    public void RemoveBlockFromGrid(TetrisBlock tetrisBlock)
    {
        var tetrisBlockTrans = tetrisBlock.transform;
        for (int i = 0; i < tetrisBlockTrans.childCount; i++)
        {
            var childTrans = tetrisBlockTrans.GetChild(i);
            int x = Mathf.RoundToInt(childTrans.position.x);
            int y = Mathf.RoundToInt(childTrans.position.y);
            var row = y;
            var col = x;
            Grid[row, col] = null;
        }
    }

    public void ApplyClearanceRule()
    {
        for (int i = 0; i < Rows; i++)
        {
            var filled = true;
            for (int j = 0; j < Cols; j++)
            {
                if (Grid[i, j] == null)
                {
                    filled = false;
                    break;
                }
            }
            if (filled)
            {
                var removed = new TetrisBlock[Cols];
                for (int j = 0; j < Cols; j++)
                {
                    var child = Grid[i, j];
                    var tetrisBlock = child.GetComponentInParent<TetrisBlock>();
                    GameObject.Destroy(child.gameObject);
                    Grid[i, j] = null;
                    removed[j] = tetrisBlock;
                }
                for (int k = 0; k < Cols; k++)
                {
                    var tetrisBlock = removed[k];
                    RemoveBlockFromGrid(tetrisBlock);
                    tetrisBlock.enabled = true;
                }
            }
        }
    }
}
