using System;
using UnityEngine;

public class NewWallet
{
    private int _score;

    public int Score => _score;

    public event Action<int> ScoreChanged;

    public NewWallet()
    {
        ScoreChanged?.Invoke(_score);
    }

    public void AddPoints(int value)
    {
        if (value < 0)
            throw new ArgumentException("Value cannot be negative");

        _score += value;
        ScoreChanged?.Invoke(_score);
    }

    public void Reset()
    {
        _score = 0;
        ScoreChanged?.Invoke(_score);
    }
}
