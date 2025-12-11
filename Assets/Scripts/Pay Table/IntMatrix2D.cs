using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntMatrix2D
{
    public int rows = 3;
    public int columns = 3;
    public List<int> data = new List<int>();

    public void Initialize()
    {
        data = new List<int>(new int[rows * columns]);
    }

    public int Get(int row, int col)
    {
        return data[row * columns + col];
    }

    public void Set(int row, int col, int value)
    {
        data[row * columns + col] = value;
    }
}
