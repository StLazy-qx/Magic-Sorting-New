using UnityEngine;
using Zenject;

public class DifficultyState : IInitializable
{
    public DifficultyLevel CurrentDifficulty { get; private set; } =
        DifficultyLevel.Easy;

    public void SetDifficulty(DifficultyLevel level)
    {
        CurrentDifficulty  = level;
    }

    public void Initialize() {}
}
