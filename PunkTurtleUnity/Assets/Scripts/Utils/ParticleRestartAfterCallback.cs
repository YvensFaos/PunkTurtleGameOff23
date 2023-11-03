using System.Collections;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class ParticleRestartAfterCallback : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField, Range(1, 10)] private float randomizerTimer;

    private void Awake()
    {
        AssessUtils.CheckRequirement(ref particles, this);
    }

    public void OnParticleSystemStopped()
    {
        StartCoroutine(RestartAfterCoroutine());
    }

    private IEnumerator RestartAfterCoroutine()
    {
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        yield return new WaitUntil(() => particles.isStopped && particles.particleCount == 0);
        yield return new WaitForSeconds(Random.Range(0, randomizerTimer));
        particles.Play();
    }
}