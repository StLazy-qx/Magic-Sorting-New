using System;
using System.Collections.Generic;
using UnityEngine;


//���������� ��� ������������� ���������� �� ������� � ���� �����
public class MagicCellsStackHandler : MonoBehaviour
{
    private readonly MagicCellsFactory _factory;
    private readonly MagicCellRouter _cellRouter;
    private readonly BuildMagicColumn _colorSource;
    private readonly Transform _parent;
    private readonly float _prefabHeight;

    private readonly Stack<MagicCell> _cellsStack = new();

    public event Action CellDisplacing;

    public MagicCellsStackHandler(
        MagicCellsFactory factory,
        MagicCellRouter cellRouter,
        BuildMagicColumn colorSource,
        Transform parent,
        float prefabHeight)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _cellRouter = cellRouter ?? throw new ArgumentNullException(nameof(cellRouter));
        _colorSource = colorSource ?? throw new ArgumentNullException(nameof(colorSource));
        _parent = parent ?? throw new ArgumentNullException(nameof(parent));
        _prefabHeight = prefabHeight;
    }

    public void CreateCells(int countCells)
    {
        float currentY = 0f;

        for (int i = 0; i < countCells; i++)
        {
            Color? pickedColor = _colorSource.GetRandomColor();

            if (!pickedColor.HasValue)
            {
                Debug.LogWarning($"[MagicCellsStackHandler] ������������ ������. ������� {i} �� {countCells}.");

                break;
            }

            MagicCell cell = _factory.CreateCell(
                parent: _parent,
                localPosition: new Vector3(0, currentY, 0),
                color: pickedColor.Value);

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
}
