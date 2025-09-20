using System;
using UnityEngine;

public class MagicColumn : MonoBehaviour, IInteractable
{
    [SerializeField] private MagicCellRouter _cellRouter;
    [SerializeField] private MagicCellsFactory _factory;

    private int _countCells;
    private float _prefabHeight;
    private float _distanceBetweenCells = 0.05f;
    private MagicCell _cellPrefab;
    private BuildMagicColumn _colorSource;

    private MagicCellsStackHandler _stackHandler;

    public event Action CellDisplacing;
    public event Action Interacted;

    public void Initialize(
        MagicCell magicCell,
        MagicCellRouter distributerMagicCell,
        BuildMagicColumn colorSource,
        int countCells)
    {
        if (countCells <= 0 || colorSource == null)
            return;

        _cellPrefab = magicCell;
        _cellRouter = distributerMagicCell;
        _colorSource = colorSource;
        _countCells = countCells;
        _prefabHeight = GetPrefabHeight();

        _stackHandler = new MagicCellsStackHandler(
            _factory,
            _cellRouter,
            _colorSource,
            transform,
            _prefabHeight);

        _stackHandler.CellDisplacing += () => CellDisplacing?.Invoke();

        _stackHandler.CreateCells(_countCells);
    }

    private float GetPrefabHeight()
    {
        Renderer renderer = _cellPrefab.GetComponentInChildren<Renderer>();
        return renderer.bounds.size.y + _distanceBetweenCells;
    }

    public void OnClick()
    {
        Interacted?.Invoke();
    }

    //[SerializeField] private MagicCellRouter _cellRouter;
    //[SerializeField] private MagicCellsFactory _factory;

    //private int _countCells;
    //private float _prefabHeight;
    //private float _distanceBetweenCells = 0.05f;
    //private MagicCell _cellPrefab;
    //private Stack<MagicCell> _cellsStack;
    //private BuildMagicColumn _colorSource;

    //public event Action CellDisplacing;
    //public event Action Interacted;

    //private void Awake()
    //{
    //    _cellsStack = new();
    //}

    //public void Initialize(
    //    MagicCell magicCell, 
    //    MagicCellRouter distributerMagicCell, 
    //    BuildMagicColumn colorSource, 
    //    int countCells)
    //{
    //    if (countCells <= 0 || colorSource == null)
    //        return;

    //    _cellPrefab = magicCell;
    //    _cellRouter = distributerMagicCell;
    //    _colorSource = colorSource;
    //    _countCells = countCells;
    //    _prefabHeight = GetPrefabHeight();

    //    CreateCells();
    //}

    //public void CreateCells()
    //{
    //    if (_cellPrefab == null || _colorSource == null)
    //    {
    //        Debug.LogError($"[MagicColumn] ����������� prefab ��� �������� ������ � {name}.");

    //        return;
    //    }

    //    float currentY = transform.position.y;

    //    for (int i = 0; i < _countCells; i++)
    //    {
    //        Color? pickedColor = _colorSource.GetRandomColor();

    //        if (pickedColor.HasValue == false)
    //        {
    //            Debug.LogWarning($"[MagicColumn] " +
    //                $"������������ ������ ��� {name}. ������� {i} �� {_countCells}.");

    //            break;
    //        }

    //        MagicCell cell = _factory.CreateCell(
    //            parent: transform,
    //            localPosition: new Vector3(0, currentY, 0),
    //            color: pickedColor.Value);

    //        //MagicCell cell = Instantiate(_cellPrefab, transform);

    //        //cell.transform.localPosition = new Vector3(0, currentY, 0);
    //        //cell.SetColor(pickedColor.Value);

    //        ClickHandler clickHandler = cell.GetComponent<ClickHandler>();

    //        if (clickHandler != null)
    //            clickHandler.OnClicked += OnCellClicked;

    //        _cellsStack.Push(cell);

    //        currentY += _prefabHeight;
    //    }
    //}

    //private void OnCellClicked()
    //{
    //    if (_cellsStack.Count == 0)
    //        return;

    //    MagicCell topCell = _cellsStack.Peek();

    //    if (_cellRouter.IsCheckCellColor(topCell.Color) == false)
    //        return;

    //    MagicCell newTopCell = _cellsStack.Pop();

    //    _cellRouter.DeliverMagicCell(newTopCell);
    //    newTopCell.Disable();
    //    CellDisplacing?.Invoke();
    //}

    //private float GetPrefabHeight()
    //{
    //    Renderer renderer = _cellPrefab.GetComponentInChildren<Renderer>();

    //    return renderer.bounds.size.y + _distanceBetweenCells;
    //}

    //public void OnClick()
    //{
    //    Interacted?.Invoke();
    //}
}
