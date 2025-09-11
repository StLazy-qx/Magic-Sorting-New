using Zenject;

public class WalletInstaller : IInitializable
{
    private readonly NewWallet _wallet;

    public WalletInstaller(NewWallet wallet)
    {
        _wallet = wallet;
    }

    public void Initialize()
    {
        // ����� �������� ��������� ������, ��������:
        _wallet.Reset();
    }

    public NewWallet GetWallet() => _wallet;
}
