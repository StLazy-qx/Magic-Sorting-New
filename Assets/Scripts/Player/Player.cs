using UnityEngine;
using Zenject;

public class Player : MonoBehaviour 
{
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private Texture _currentTexture;

    private Material _materialInstance;
    private Wallet _wallet;

    public Wallet PlayerWallet => _wallet;

    private void Awake()
    {
        _materialInstance = _meshRenderer.material;
    }

    [Inject]
    public void Construct(Wallet walletl)
    {
        _wallet = walletl;
    }

    public void SetTexture(Texture texture)
    {
        if (_materialInstance == null || texture == null)
            return;

        _currentTexture = texture;
        _materialInstance.SetTexture("_MainTex", _currentTexture);
    }
}
