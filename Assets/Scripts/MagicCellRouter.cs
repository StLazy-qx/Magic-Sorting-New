using System;
using System.Collections.Generic;
using UnityEngine;

public class MagicCellRouter : MonoBehaviour
{
    [SerializeField] private WaitingPoint _waitingPoint;

    private IReadOnlyList<Vessel> _vessels;

    public void DeliverMagicCell(MagicCell cell)
    {
        if (cell == null)
            throw new ArgumentNullException(nameof(cell),
                "[DistributerMagicCell] ��������� ������ �� ����� ���� �������.");

        Color cellColor = cell.Color;
        Vessel targetVessel = FindVesselByColor(cellColor);

        if (targetVessel != null)
        {
            targetVessel.TakeMagic(cell);
        }
        else if (_waitingPoint.IsSeat)
        {
            _waitingPoint.HoldOverflowCell(cell);
        }
        else
        {
            Debug.LogWarning("���������� ��������� ������ - " +
                "��� ����������� ������ � ����� �������� ������");
        }
    }

    public void AcceptVesselsList(IReadOnlyList<Vessel> vessels)
    {
        if (vessels == null)
            throw new ArgumentNullException
                (nameof(vessels),
                "[DistributerMagicCell] ������ ����� �� ����� ���� ������.");

        _vessels = vessels;
    }

    public bool IsCheckCellColor(Color color)
    {
        if (FindVesselByColor(color) != null)
            return true;

        //return _waitingPoint.IsSeat;
        Debug.Log(_waitingPoint.IsSeat);

        return _waitingPoint.IsSeat;
    }

    private Vessel FindVesselByColor(Color color)
    {
        foreach (Vessel vessel in _vessels)
        {
            if (vessel.IsActive && vessel.Color == color)
            {
                return vessel;
            }
        }

        return null;
    }
}
