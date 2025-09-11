using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private MagicianAnimator _animator;
    [SerializeField] private SoundPlayer _soundPlayer;
    [SerializeField] private VFXPlayer _magicEffect;
    [SerializeField] private ColumnsFactory _columnsFactory;
    
    private List<MagicColumn> _interactables;

    private void OnEnable()
    {
        if (_columnsFactory != null)
        {
            _columnsFactory.ColumnsListChanged += OnColumnsListChanged;
        }
    }

    private void OnDisable()
    {
        if (_columnsFactory != null)
        {
            _columnsFactory.ColumnsListChanged -= OnColumnsListChanged;
        }
    }

    private void OnColumnsListChanged(List<MagicColumn> magicColumns)
    {
        _interactables = magicColumns;

        foreach (var interactable in _interactables)
        {
            if (interactable is MagicColumn magicColumn)
            {
                magicColumn.CellDisplacing += OnPlayerAction;
            }
        }
    }

    private void OnPlayerAction()
    {
        //найти элеметны для визуализации 

        //_animator.PlayInteract();
        _soundPlayer.PlayInteractSound();
        //_vfxController.SpawnInteractEffect();
    }
}

