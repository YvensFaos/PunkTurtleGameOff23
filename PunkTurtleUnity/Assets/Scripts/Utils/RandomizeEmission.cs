using System.Collections;
using UnityEngine;
using Utils;

/// <summary>
/// Deprecated - Not in use!
/// </summary>
public class RandomizeEmission : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField, Range(1, 10)] private float randomizerTimer;
    [SerializeField, Range(0, 10)] private int minEmission;
    [SerializeField, Range(0, 10)] private int maxEmission;
    [SerializeField] private bool onStart;

    private float startLifeTime;

    private void Awake()
    {
        if (!AssessUtils.CheckRequirement(ref particles, this))
        {
            Destroy(this);
            return;
        }

        startLifeTime = particles.main.startLifetime.constant;
    }
    
    private void Start()
    {
        if (onStart)
        {
            StartRandomize();
        }
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public void StartRandomize()
    {
        StartCoroutine(RandomizeCoroutine());
    }

    private IEnumerator RandomizeCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, randomizerTimer));
            yield return new WaitForSeconds(startLifeTime);
            
            var emissionModule = particles.emission;
            var rateOverTime = emissionModule.rateOverTime;
            rateOverTime.constant = Random.Range(minEmission, maxEmission + 1);
            emissionModule.rateOverTime = rateOverTime;
        }
    }
}
