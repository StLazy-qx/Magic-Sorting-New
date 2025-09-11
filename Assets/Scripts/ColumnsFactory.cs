using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ColumnsFactory : MonoBehaviour
{
    [Header("Prefabs & References")]
    [SerializeField] private MagicColumn _columnPrefab;
    [SerializeField] private MagicCell _magicCellPrefab; // потом за префаб €чейки будет отвечать фабрика €чеек
    [SerializeField] private MagicCellRouter _distributerMagicCell;
    [SerializeField] private BuildMagicColumn _buildMagicColumn;
    [SerializeField] private DifficultyDatabase _difficultyDatabase;

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] _spawnPoints;

    private DifficultyState _difficultyState;
    private DifficultySettings _currentSettings;
    private LevelDifficultySetterUI _levelDifficulty;
    private List<MagicColumn> _spawnedColumns = new List<MagicColumn>();

    public event Action<List<MagicColumn>> ColumnsListChanged;

    private void Start()
    {
        BuildColumns();
    }

    private void OnEnable()
    {
        if (_levelDifficulty != null)
            _levelDifficulty.LevelDifficultyChanged += OnDifficultyChanged;
    }

    private void OnDisable()
    {
        if (_levelDifficulty != null)
            _levelDifficulty.LevelDifficultyChanged -= OnDifficultyChanged;
    }

    [Inject]
    public void Construct(DifficultyState difficultyState)
    {
        _difficultyState = difficultyState;
        _currentSettings = _difficultyDatabase.GetSettings(_difficultyState.CurrentDifficulty);
    }

    public void BuildColumns()
    {
        ClearExistingColumns();

        int totalColors = _buildMagicColumn.TotalColors;
        int currentCountSpawnPoints = GetNumberOfPointsBasedOnDifficulty();
        int cellsPerColumn = Mathf.Max(1, totalColors / currentCountSpawnPoints);

        if (cellsPerColumn == 0)
            cellsPerColumn = 1;

        for (int i = 0; i < currentCountSpawnPoints; i++)
        {
            Transform point = _spawnPoints[i];
            MagicColumn columnInstance = Instantiate(
                _columnPrefab,
                point.position, 
                point.rotation);
            columnInstance.name = $"MagicColumn_{i}";

            columnInstance.Initialize(_magicCellPrefab, 
                _distributerMagicCell, 
                _buildMagicColumn, 
                cellsPerColumn);
            _spawnedColumns.Add(columnInstance);
        }

        ColumnsListChanged?.Invoke(_spawnedColumns);
    }

    //изменить дл€ безопасности
    public List<MagicColumn> GetSpawnedColumns()
    {
        return new List<MagicColumn>(_spawnedColumns);
    }

    //изменить название
    public void ResetFactory(DifficultyLevel level)
    {
        _difficultyState.SetDifficulty(level);
        _buildMagicColumn.Reset();
        BuildColumns();
    }

    private void OnDifficultyChanged(DifficultyLevel level)
    {
        BuildColumns();
    }

    private void ClearExistingColumns()
    {
        foreach (var column in _spawnedColumns)
        {
            if (column != null && column.gameObject != null)
                DestroyImmediate(column.gameObject);
        }

        _spawnedColumns.Clear();
    }

    //помен€ть название
    private int GetNumberOfPointsBasedOnDifficulty()
    {
        _currentSettings = _difficultyDatabase.GetSettings(_difficultyState.CurrentDifficulty);

        return Mathf.Min(_currentSettings.maxSpawnPoints, _spawnPoints.Length);
    }
}
