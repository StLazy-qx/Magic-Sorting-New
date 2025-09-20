using System.Collections.Generic;
using UnityEngine;

public class VesselFactory : Factory<Vessel>
{
    [SerializeField] private VesselsFullingBehaviour _gameBehaviour;
    [SerializeField] private MagicCellRouter _distributerMagicCell;
    [SerializeField] private BuildMagicColumn _buildMagicColumn;
    [SerializeField] private ColorRandomizer _colorRandomizer;

    private Queue<Vessel> _vesselsQueue = new Queue<Vessel>();

    protected override void OnDestroy()
    {
        foreach (var vesell in Objects)
        {
            if (vesell != null)
                vesell.Completed -= OnReplaceVessel;
        }

        base.OnDestroy();
    }

    protected override void BuildInstances()
    {
        if (Prefab == null)
        {
            Debug.LogError($"{name}: Prefab �� �������� � ����������!");

            return;
        }

        if (SpawnPoints == null || SpawnPoints.Length == 0)
        {
            Debug.LogWarning($"{name}: SpawnPoints ������ � ������ ����� �������, �� �� ����������.");
        }

        Objects.Clear();
        _vesselsQueue.Clear();

        int count = 0;
        int colors = 0;

        if (CurrentSettings != null)
        {
            count = CurrentSettings.vesselsCount;
            colors = CurrentSettings.colorsCount;
        }
        else if (DifficultyDatabase != null && DifficultyState != null)
        {
            var difficultyState = DifficultyDatabase.GetSettings(DifficultyState.CurrentDifficulty);
            count = difficultyState.vesselsCount;
            colors = difficultyState.colorsCount;
        }

        for (int i = 0; i < count; i++)
        {
            Vessel vessel = Instantiate(Prefab);
            vessel.Completed += OnReplaceVessel;
            vessel.gameObject.SetActive(false);

            Objects.Add(vessel);
            _vesselsQueue.Enqueue(vessel);
        }

        AssignColorsToVessels(CurrentSettings.colorsCount);
        _distributerMagicCell.AcceptVesselsList(Objects);
        _buildMagicColumn.AcceptVesselsList(Objects);
        _gameBehaviour.Init(Objects);
        SpawnVessels();
        NotifyInstancesChanged();
    }

    private void OnReplaceVessel(Vector3 position)
    {
        if (_vesselsQueue.Count == 0)
            return;

        Vessel newVessel = _vesselsQueue.Dequeue();
        newVessel.transform.position = position;

        newVessel.gameObject.SetActive(true);
    }

    private void SpawnVessels()
    {
        if (_vesselsQueue.Count == 0)
            return;

        if (SpawnPoints == null || SpawnPoints.Length == 0)
            return;

        for (int i = 0; i < SpawnPoints.Length && _vesselsQueue.Count > 0; i++)
        {
            Vessel vessel = _vesselsQueue.Dequeue();
            vessel.transform.position = SpawnPoints[i].position;

            vessel.gameObject.SetActive(true);
        }
    }

    private void AssignColorsToVessels(int countColors)
    {
        if (Objects.Count == 0)
            return;

        if (Objects.Count > countColors)
        {
            Color[] pointColors = _colorRandomizer.CrateArrayColors(Mathf.Min(countColors, SpawnPoints.Length));

            for (int i = 0; i < Mathf.Min(SpawnPoints.Length, Objects.Count); i++)
            {
                Color colorToAssign = i < pointColors.Length
                    ? pointColors[i]
                    : _colorRandomizer.GenerateRandomColor();
                Objects[i].GetComponent<ColorMarker>().Init(colorToAssign);
            }

            for (int i = SpawnPoints.Length; i < Objects.Count; i++)
            {
                Objects[i].GetComponent<ColorMarker>()
                    .Init(_colorRandomizer.GenerateRandomColor());
            }
        }
        else
        {
            Color[] colors = _colorRandomizer.CrateArrayColors(countColors);

            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].GetComponent<ColorMarker>()
                    .Init(colors[i % colors.Length]);
            }
        }
    }

    //[SerializeField] private VesselsFullingBehaviour _gameBehaviour; // ����� ��� �����?
    //[SerializeField] private Vessel _prefab;
    //[SerializeField] private MagicCellRouter _distributerMagicCell;
    //[SerializeField] private BuildMagicColumn _buildMagicColumn;
    //[SerializeField] private DifficultyDatabase _difficultyDatabase;
    //[SerializeField] private Transform[] _points;

    //private int _countVessels;
    //private int _countColors;
    //private LevelDifficultySetterUI _levelDifficulty;
    //private DifficultyState _difficultyState;
    //private DifficultySettings _currentSettings;
    //private ColorRandomizer _colorRandomizer;
    //private List<Vessel> _vessels;
    //private Queue<Vessel> _vesselsQueue;

    //public IReadOnlyList<Vessel> Vessels => _vessels;


    //private void Awake()
    //{
    //    _colorRandomizer = GetComponent<ColorRandomizer>();
    //    _vessels = new List<Vessel>();
    //    _vesselsQueue = new Queue<Vessel>();

    //    OnDifficultyChanged(_difficultyState.CurrentDifficulty);
    //}

    //private void OnEnable()
    //{
    //    if (_difficultyState != null)
    //        _difficultyState.DifficultyChanged += OnDifficultyChanged;
    //}

    //private void OnDisable()
    //{
    //    if (_difficultyState != null)
    //        _difficultyState.DifficultyChanged -= OnDifficultyChanged;

    //    foreach (Vessel vessel in _vessels)
    //    {
    //        vessel.Completed -= OnReplaceVessel;
    //    }
    //}

    //[Inject]
    //public void Construct(DifficultyState difficultyState)
    //{
    //    _difficultyState = difficultyState;
    //    _currentSettings = _difficultyDatabase.GetSettings(_difficultyState.CurrentDifficulty);
    //}

    //public void ResetFactory(DifficultyLevel level)
    //{
    //    foreach (Vessel vessel in _vessels)
    //    {
    //        Destroy(vessel.gameObject);
    //    }

    //    _vessels.Clear();
    //    _vesselsQueue.Clear();

    //    switch (level)
    //    {
    //        case DifficultyLevel.Easy:
    //            _countVessels = 3;
    //            _countColors = 3;
    //            break;
    //        case DifficultyLevel.Medium:
    //            _countVessels = 5;
    //            _countColors = 4;
    //            break;
    //        case DifficultyLevel.Hard:
    //            _countVessels = 7;
    //            _countColors = 6;
    //            break;
    //    }

    //    Initialize();
    //    SpawnVessel();
    //}

    //private void OnDifficultyChanged(DifficultyLevel level)
    //{
    //    //����� ����������?
    //    foreach (Vessel vessel in _vessels)
    //    {
    //        Destroy(vessel.gameObject);
    //    }

    //    _vessels.Clear();
    //    _vesselsQueue.Clear();

    //    _currentSettings = _difficultyDatabase.GetSettings(level);

    //    _countVessels = _currentSettings.vesselsCount;
    //    _countColors = _currentSettings.colorsCount;

    //    Initialize();
    //    SpawnVessel();
    //}

    //private void OnReplaceVessel(Vector3 position)
    //{
    //    if (_vesselsQueue.Count == 0)
    //        return;

    //    Vessel newVessel = _vesselsQueue.Dequeue();
    //    newVessel.transform.position = position;

    //    newVessel.gameObject.SetActive(true);
    //}

    //private void SpawnVessel()
    //{
    //    if (_vesselsQueue.Count == 0)
    //        return;

    //    for (int i = 0; i < _points.Length; i++)
    //    {
    //        Vessel newVessel = _vesselsQueue.Dequeue();
    //        newVessel.transform.position = _points[i].position;

    //        newVessel.gameObject.SetActive(true);
    //    }
    //}

    //private void Initialize()
    //{
    //    if (_countVessels <= 0)
    //        return;

    //    for (int i = 0; i < _countVessels; i++)
    //    {
    //        Vessel vessel = Instantiate(
    //            _prefab);
    //        vessel.Completed += OnReplaceVessel;

    //        _vessels.Add(vessel);
    //        _vesselsQueue.Enqueue(vessel);
    //        vessel.gameObject.SetActive(false);
    //    }

    //    AssignColorsToVessels();
    //    _distributerMagicCell.AcceptVesselsList(Vessels);
    //    _buildMagicColumn.AcceptVesselsList(Vessels);
    //    _gameBehaviour.Init(Vessels);
    //}

    //private void AssignColorsToVessels()
    //{
    //    if (_countVessels > _countColors)
    //    {
    //        Color[] pointColors = _colorRandomizer.
    //            CrateArrayColors(Mathf.Min(_countColors, _points.Length));

    //        for (int i = 0; i < Mathf.Min(_points.Length, _vessels.Count); i++)
    //        {
    //            Color colorToAssign = i <
    //                pointColors.Length ?
    //                pointColors[i] :
    //                _colorRandomizer.GenerateRandomColor();
    //            _vessels[i].GetComponent<ColorMarker>().Init(colorToAssign);
    //        }

    //        for (int i = _points.Length; i < _vessels.Count; i++)
    //        {
    //            _vessels[i].GetComponent<ColorMarker>().
    //                Init(_colorRandomizer.GenerateRandomColor());
    //        }
    //    }
    //    else
    //    {
    //        Color[] colors = _colorRandomizer.CrateArrayColors(_countColors);

    //        for (int i = 0; i < _vessels.Count; i++)
    //        {
    //            _vessels[i].GetComponent<ColorMarker>().Init(colors[i % colors.Length]);
    //        }
    //    }
    //}
}
