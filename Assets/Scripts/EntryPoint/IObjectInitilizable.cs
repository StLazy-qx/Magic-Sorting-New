using UnityEngine;

// �������� ��� ���������
public interface IObjectInitilizable
{
    public bool IsInitialize { get; }

    public void Initilize();
}
