using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private GameHandler _gameHandler;
    [SerializeField] private Panel _panel;
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        if (_button != null)
        {
            _button.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnDisable()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {
        if (_gameHandler != null)
        {
            _panel.Close();
            _gameHandler.OpenMainMenu();
        }
    }
}
