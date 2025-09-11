using System;
using System.Collections.Generic;
using UnityEngine;

public class MagicColumn : MonoBehaviour, IInteractable
{
    [SerializeField] private MagicCellRouter _cellRouter;

    private int _countCells;
    private float _prefabHeight;
    private float _distanceBetweenCells = 0.05f;
    private MagicCell _cellPrefab;
    private Stack<MagicCell> _cellsStack;
    private BuildMagicColumn _colorSource;

    public event Action CellDisplacing;
    public event Action Interacted;

    private void Awake()
    {
        _cellsStack = new();
    }

    public void Initialize(
        MagicCell magicCell, 
        MagicCellRouter distributerMagicCell, 
        BuildMagicColumn colorSource, 
        int countCells)
    {
        if (countCells <= 0)
            return;

        if (colorSource == null)
        {
            Debug.LogError($"[MagicColumn] colorSource == null у {name}. " +
                $"Столб останется пустым.");
            return;
        }

        _cellPrefab = magicCell;
        _cellRouter = distributerMagicCell;
        _colorSource = colorSource;
        _countCells = countCells;
        _prefabHeight = GetPrefabHeight();

        CreateCells();
    }

    //заменить потом на фабрику Cell 
    public void CreateCells()
    {
        if (_cellPrefab == null || _colorSource == null)
        {
            Debug.LogError($"[MagicColumn] Отсутствует prefab или источник цветов у {name}.");
            return;
        }

        float currentY = transform.position.y;

        for (int i = 0; i < _countCells; i++)
        {
            Color? pickedColor = _colorSource.GetRandomColor();

            if (pickedColor.HasValue == false)
            {
                Debug.LogWarning($"[MagicColumn] " +
                    $"Недостаточно цветов для {name}. Создано {i} из {_countCells}.");

                break;
            }

            MagicCell cell = Instantiate(_cellPrefab, transform);

            cell.transform.localPosition = new Vector3(0, currentY, 0);
            cell.SetColor(pickedColor.Value);

            ClickHandler clickHandler = cell.GetComponent<ClickHandler>();

            if (clickHandler != null)
                clickHandler.OnClicked += OnCellClicked;

            _cellsStack.Push(cell);

            currentY += _prefabHeight;
        }
    }

    private void OnCellClicked()
    {
        if (_cellsStack.Count == 0)
            return;

        MagicCell topCell = _cellsStack.Peek();

        if (_cellRouter.IsCheckCellColor(topCell.Color) == false)
            return;

        MagicCell newTopCell = _cellsStack.Pop();
        
        _cellRouter.DeliverMagicCell(newTopCell);
        newTopCell.Disable();
        CellDisplacing?.Invoke();
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
}
