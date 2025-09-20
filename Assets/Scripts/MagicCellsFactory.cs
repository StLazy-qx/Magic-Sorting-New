using UnityEngine;

public class MagicCellsFactory : Factory<MagicCell>
{
    public MagicCell CreateCell(Transform parent, Vector3 localPosition, Color color)
    {
        MagicCell cell = Instantiate(Prefab, parent);
        cell.transform.localPosition = localPosition;
        cell.SetColor(color);

        Objects.Add(cell);
        NotifyInstancesChanged();

        return cell;
    }

    protected override void BuildInstances() {}
}
