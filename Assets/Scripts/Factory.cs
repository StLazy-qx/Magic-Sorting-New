using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class Factory<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T Prefab;
    [SerializeField] protected Transform[] SpawnPoints;
    [SerializeField] protected DifficultyDatabase DifficultyDatabase;

    protected DifficultyState DifficultyState;
    protected DifficultySettings CurrentSettings;
    protected List<T> Objects = new List<T>();

    public event Action<List<T>> ListObjectsChanged;

    protected virtual void OnDestroy()
    {
        if (DifficultyState != null)
            DifficultyState.DifficultyChanged -= OnDifficultyChanged;
    }

    [Inject]
    public void Construct(DifficultyState difficultyState)
    {
        DifficultyState = difficultyState;

        if (DifficultyDatabase != null)
        {
            CurrentSettings = DifficultyDatabase.
                GetSettings(DifficultyState.CurrentDifficulty);
        }

        DifficultyState.DifficultyChanged += OnDifficultyChanged;

        BuildInstances();
    }

    public virtual void ResetFactory(DifficultyLevel level)
    {
        ClearInstances();

        if (DifficultyState != null)
            DifficultyState.SetDifficulty(level);

        if (DifficultyDatabase != null)
            CurrentSettings = DifficultyDatabase.GetSettings(level);

        BuildInstances();
    }

    public virtual List<T> GetInstances()
    {
        return new List<T>(Objects);
    }

    protected abstract void BuildInstances();

    protected virtual void OnDifficultyChanged(DifficultyLevel level)
    {
        ResetFactory(level);
    }

    protected virtual void ClearInstances()
    {
        foreach (var @object in Objects)
        {
            if (@object != null)
                Destroy(@object.gameObject);
        }

        Objects.Clear();
    }

    protected void NotifyInstancesChanged()
    {
        ListObjectsChanged?.Invoke(Objects);
    }
}
