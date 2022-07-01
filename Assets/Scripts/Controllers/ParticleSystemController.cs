using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemController : MonoBehaviour
{
    private void Awake()
    {
        var system = GetComponent<ParticleSystem>().main;
        system.stopAction = ParticleSystemStopAction.Callback;
    }
    void OnParticleSystemStopped()
    {
        if(gameObject.activeSelf)
            GetComponent<IDisposable>()?.Dispose();
    }
}