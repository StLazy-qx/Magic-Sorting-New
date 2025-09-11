using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _interactEffect;
    [SerializeField] private Transform _magicEffectPoint;

    private void Awake()
    {
        _interactEffect.transform.position = _magicEffectPoint.position;
    }

    public void SpawnInteractEffect()
    {
        if (_interactEffect == null)
            return;

        _interactEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _interactEffect.Play();
    }
}