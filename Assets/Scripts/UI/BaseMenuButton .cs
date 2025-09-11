using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]

public abstract class BaseMenuButton : MonoBehaviour
{
    [SerializeField] protected Panel TargetPanel;

    protected GameHandler GameHandler;
    protected Panel CurrentPanel;
    protected Button Button;

    protected virtual void Awake()
    {
        CurrentPanel = GetComponentInParent<Panel>();
        Button = GetComponent<Button>();
    }

    protected virtual void OnEnable()
    {
        Button.onClick.AddListener(OnButtonClick);
    }

    protected virtual void OnDisable()
    {
        Button.onClick.RemoveListener(OnButtonClick);
    }

    [Inject]
    public void InjectDependencies(GameHandler gameHandler)
    {
        GameHandler = gameHandler;
    }

    //нужно ли оставлять protected или сделать public?
    protected abstract void OnButtonClick();
}
