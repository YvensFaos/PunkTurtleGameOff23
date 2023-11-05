using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class SpawnAreaControl : MonoBehaviour
{
   [SerializeField]
   private BoxCollider2D spawnArea;
   [SerializeField]
   private List<GameObject> spawnables;
   [SerializeField]
   private int minSpawns;
   [SerializeField]
   private int maxSpawns;

   private List<GameObject> spawnedObjects;
   
   private void OnEnable()
   {
      Spawn();
   }

   private void OnDisable()
   {
      DespawnRemainders();
   }

   private void Spawn()
   {
      DespawnRemainders();
      
      spawnedObjects = new List<GameObject>();
      var spawnCount = Random.Range(minSpawns, maxSpawns);
      for (var i = 0; i < spawnCount; i++)
      {
         var spawnObject = RandomHelper<GameObject>.GetRandomFromList(spawnables);
         var position = RandomPointUtils.GetRandomPointWithBox2D(spawnArea);
         var spawnedObject = LeanPool.Spawn(spawnObject, position, Quaternion.identity, transform);
         spawnedObjects.Add(spawnedObject);
      }
   }

   private void DespawnRemainders()
   {
      spawnedObjects?.ForEach(spawn => LeanPool.Despawn(spawn));
   }
}
