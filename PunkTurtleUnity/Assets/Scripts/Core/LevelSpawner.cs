using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using Utils;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Pair<GameObject, Transform>> spawn;

    private void Start()
    {
        foreach (var pair in spawn)
        {
            var pairTwo = pair.Two;
            LeanPool.Spawn(pair.One, pairTwo.position, pairTwo.rotation);
        }
    }
}
