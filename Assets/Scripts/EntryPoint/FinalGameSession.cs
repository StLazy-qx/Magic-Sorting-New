using System;
using UnityEngine;

public class FinalGameSession : MonoBehaviour
{
    [SerializeField] private Panel _finalMatchPanel;

    public event Action<int> RoundChanged;

    public int CurrentRound { get; private set; } = 1;

    // �������� �������� ������
    public void ActivateFinalPanelAndPauseGame()
    {
        if (_finalMatchPanel == null)
        {
            Debug.LogError("FinalMatchPanel is not assigned in the inspector!", this);

            return;
        }

        _finalMatchPanel.Open();
        IncreaseRound();

        Time.timeScale = 0f; // ������ ����������
    }

    private void IncreaseRound()
    {
        CurrentRound++;
        RoundChanged?.Invoke(CurrentRound);
    }
}
