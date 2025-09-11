using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelDifficultySetterUI : MonoBehaviour
{
    [SerializeField] private Button _easyLevelButton;
    [SerializeField] private Button _middleLevelButton;
    [SerializeField] private Button _hardLevelButton;
    [SerializeField] private Color _selectedColor;

    private DifficultyState _difficultyState;
    private Button _selectedButton;
    private Color _defaultColorButton;

    public event Action<DifficultyLevel> LevelDifficultyChanged;

    private void Awake()
    {
        if (_easyLevelButton != null)
            _defaultColorButton = _easyLevelButton.image.color;
    }

    private void OnEnable()
    {
        //_easyLevelButton.onClick.AddListener(() =>
        //{
        //    if (_difficultyState != null)
        //        SelectDifficulty(DifficultyLevel.Easy);
        //});

        _easyLevelButton.onClick.AddListener(() =>
        SelectDifficulty(DifficultyLevel.Easy));
        _middleLevelButton.onClick.AddListener(() => 
        SelectDifficulty(DifficultyLevel.Medium));
        _hardLevelButton.onClick.AddListener(() => 
        SelectDifficulty(DifficultyLevel.Hard));

        //HighlightButton(_easyLevelButton);
        SelectDifficulty(_difficultyState.CurrentDifficulty);
    }

    private void OnDisable()
    {
        _easyLevelButton.onClick.RemoveListener(() => 
        SelectDifficulty(DifficultyLevel.Easy));
        _middleLevelButton.onClick.RemoveListener(() => 
        SelectDifficulty(DifficultyLevel.Medium));
        _hardLevelButton.onClick.RemoveListener(() => 
        SelectDifficulty(DifficultyLevel.Hard));
    }

    [Inject]
    public void Construct(DifficultyState state)
    {
        _difficultyState = state;
        //_difficultyState.SetDifficulty(DifficultyLevel.Easy);
    }

    public DifficultyLevel Increase(DifficultyLevel currentLevel)
    {
        DifficultyLevel newLevel = currentLevel;

        switch (currentLevel)
        {
            case DifficultyLevel.Easy:
                newLevel = DifficultyLevel.Medium;
                break;
            case DifficultyLevel.Medium:
                newLevel = DifficultyLevel.Hard;
                break;
            case DifficultyLevel.Hard:
                newLevel = DifficultyLevel.Hard; // дальше некуда
                break;
        }

        if (newLevel != currentLevel)
            SelectDifficulty(newLevel);

        return newLevel;
    }

    private void SelectDifficulty(DifficultyLevel level)
    {
        _difficultyState.SetDifficulty(level);

        switch (level)
        {
            case DifficultyLevel.Easy:
                HighlightButton(_easyLevelButton);
                break;
            case DifficultyLevel.Medium:
                HighlightButton(_middleLevelButton);
                break;
            case DifficultyLevel.Hard:
                HighlightButton(_hardLevelButton);
                break;
        }
    }

    private void HighlightButton(Button button)
    {
        ResetButtonColors();

        button.image.color = _selectedColor;
        _selectedButton = button;
    }

    private void ResetButtonColors()
    {
        if (_easyLevelButton != null) 
            _easyLevelButton.image.color = _defaultColorButton;

        if (_middleLevelButton != null) 
            _middleLevelButton.image.color = _defaultColorButton;

        if (_hardLevelButton != null) 
            _hardLevelButton.image.color = _defaultColorButton;
    }
}