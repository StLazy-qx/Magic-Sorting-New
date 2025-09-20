using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class VesselsFullingBehaviour : MonoBehaviour
{
    [SerializeField] private Panel _gamePanel;
    [SerializeField] private FinalGameSession _finalGame;

    private Wallet _wallet;
    private int _veselsCount;
    private IReadOnlyList<Vessel> _vessels;

    public void Init(IReadOnlyList<Vessel> vessels)
    {
        _vessels = vessels;

        foreach (Vessel vessel in _vessels)
            vessel.PointCounted += OnAddPoints;
    }

    private void OnDestroy()
    {
        foreach (Vessel vessel in _vessels)
            vessel.PointCounted -= OnAddPoints;
    }

    [Inject]
    private void Construct(Wallet wallet)
    {
        _wallet = wallet;
    }

    private void OnAddPoints(int value)
    {
        _wallet.AddPoints(value);

        _veselsCount++;

        if (_veselsCount == _vessels.Count)
        {
            _wallet.ConfirmPoints();
            _gamePanel.Close();
            _finalGame.ActivateFinalPanelAndPauseGame();
            _veselsCount = 0;
        }
    }
}
