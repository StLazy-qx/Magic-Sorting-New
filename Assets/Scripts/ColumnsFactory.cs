using UnityEngine;

public class ColumnsFactory : Factory<MagicColumn>
{
    [SerializeField] private MagicCell _magicCellPrefab;
    [SerializeField] private MagicCellRouter _distributerMagicCell;
    [SerializeField] private BuildMagicColumn _buildMagicColumn;

    protected override void BuildInstances()
    {
        ClearInstances();

        int totalColors = _buildMagicColumn.TotalColors;
        int currentCountSpawnPoints = GetNumberOfPointsBasedOnDifficulty();
        int cellsPerColumn = Mathf.Max(1, totalColors / currentCountSpawnPoints);

        for (int i = 0; i < currentCountSpawnPoints; i++)
        {
            Transform point = SpawnPoints[i];
            MagicColumn columnInstance = Instantiate(
                Prefab,
                point.position,
                point.rotation);

            columnInstance.name = $"MagicColumn_{i}";

            columnInstance.Initialize(
                _magicCellPrefab,
                _distributerMagicCell,
                _buildMagicColumn,
                cellsPerColumn);

            Objects.Add(columnInstance);
        }

        NotifyInstancesChanged();
    }

    private int GetNumberOfPointsBasedOnDifficulty()
    {
        if (CurrentSettings == null && DifficultyDatabase != null)
        {
            CurrentSettings = DifficultyDatabase.GetSettings(DifficultyState.CurrentDifficulty);
        }

        return Mathf.Min(CurrentSettings.maxSpawnPoints, SpawnPoints.Length);
    }

    //[Header("Prefabs & References")]
    //[SerializeField] private MagicColumn _columnPrefab;
    //[SerializeField] private MagicCell _magicCellPrefab;
    //[SerializeField] private MagicCellRouter _distributerMagicCell;
    //[SerializeField] private BuildMagicColumn _buildMagicColumn;
    //[SerializeField] private DifficultyDatabase _difficultyDatabase;

    //[Header("Spawn Settings")]
    //[SerializeField] private Transform[] _spawnPoints;

    //private DifficultyState _difficultyState;
    //private DifficultySettings _currentSettings;
    //private LevelDifficultySetterUI _levelDifficulty;
    //private List<MagicColumn> _spawnedColumns = new List<MagicColumn>();

    //public event Action<List<MagicColumn>> ColumnsListChanged;

    //private void Start()
    //{
    //    BuildColumns();
    //}

    //private void OnEnable()
    //{
    //    if (_levelDifficulty != null)
    //        _difficultyState.DifficultyChanged += OnDifficultyChanged;
    //}

    //private void OnDisable()
    //{
    //    if (_levelDifficulty != null)
    //        _difficultyState.DifficultyChanged -= OnDifficultyChanged;
    //}

    //[Inject]
    //public void Construct(DifficultyState difficultyState)
    //{
    //    _difficultyState = difficultyState;
    //    _currentSettings = _difficultyDatabase.GetSettings(_difficultyState.CurrentDifficulty);
    //}

    //public void BuildColumns()
    //{
    //    ClearExistingColumns();

    //    int totalColors = _buildMagicColumn.TotalColors;
    //    int currentCountSpawnPoints = GetNumberOfPointsBasedOnDifficulty();
    //    int cellsPerColumn = Mathf.Max(1, totalColors / currentCountSpawnPoints);

    //    if (cellsPerColumn == 0)
    //        cellsPerColumn = 1;

    //    for (int i = 0; i < currentCountSpawnPoints; i++)
    //    {
    //        Transform point = _spawnPoints[i];
    //        MagicColumn columnInstance = Instantiate(
    //            _columnPrefab,
    //            point.position, 
    //            point.rotation);
    //        columnInstance.name = $"MagicColumn_{i}";

    //        columnInstance.Initialize(_magicCellPrefab, 
    //            _distributerMagicCell, 
    //            _buildMagicColumn, 
    //            cellsPerColumn);
    //        _spawnedColumns.Add(columnInstance);
    //    }

    //    ColumnsListChanged?.Invoke(_spawnedColumns);
    //}

    ////изменить для безопасности
    //public List<MagicColumn> GetSpawnedColumns()
    //{
    //    return new List<MagicColumn>(_spawnedColumns);
    //}

    ////изменить название
    //public void ResetFactory(DifficultyLevel level)
    //{
    //    _difficultyState.SetDifficulty(level);
    //    _buildMagicColumn.Reset();
    //    BuildColumns();
    //}

    //private void OnDifficultyChanged(DifficultyLevel level)
    //{
    //    BuildColumns();
    //}

    //private void ClearExistingColumns()
    //{
    //    foreach (var column in _spawnedColumns)
    //    {
    //        if (column != null && column.gameObject != null)
    //            DestroyImmediate(column.gameObject);
    //    }

    //    _spawnedColumns.Clear();
    //}

    ////поменять название
    //private int GetNumberOfPointsBasedOnDifficulty()
    //{
    //    _currentSettings = _difficultyDatabase.GetSettings(_difficultyState.CurrentDifficulty);

    //    return Mathf.Min(_currentSettings.maxSpawnPoints, _spawnPoints.Length);
    //}
}
