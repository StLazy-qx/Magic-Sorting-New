using System;
using UnityEngine;

[RequireComponent(typeof(VolumeAggregator))]

public class Vessel : MonoBehaviour, IColorable
{
    [SerializeField] private Liquid _liquid;
    [SerializeField] private int _maxSize;
    [SerializeField] private int _points;

    private Color _mainColor;
    private VolumeAggregator _aggregator;

    public int Count => _maxSize;
    public bool IsActive => gameObject.activeSelf;
    public Color Color => _mainColor;
    public Liquid Liquid => _liquid;

    //изменить имена событий
    public event Action<Vector3> Completed;
    public event Action<int> PointCounted;

    private void Awake()
    {
        _aggregator = GetComponent<VolumeAggregator>();
    }

    public void TakeMagic(MagicCell cell)
    {
        if (cell == null)
            return;

        _aggregator.GrowUpVolume();

        if (_aggregator.IsFull)
        {
            PointCounted?.Invoke(_points);
            Completed?.Invoke(transform.position);
            gameObject.SetActive(false);
        }
    }

    public void SetColor(Color color)
    {
        _mainColor = color;

        //перенести в Awake
        _aggregator.InitParameters(_maxSize,_liquid);
    }
}
