using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class SpawnAreaControl : MonoBehaviour
{
   [SerializeField]
   private BoxCollider2D spawnArea;
   [SerializeField]
   private List<SpriteRenderer> spawnables;
   [SerializeField]
   private int minSpawns;
   [SerializeField]
   private int maxSpawns;
   [SerializeField]
   private bool mirror;

   private List<SpriteRenderer> spawnedObjects;
   
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
      
      spawnedObjects = new List<SpriteRenderer>();
      var spawnCount = Random.Range(minSpawns, maxSpawns);
      for (var i = 0; i < spawnCount; i++)
      {
         var spawnObject = RandomHelper<SpriteRenderer>.GetRandomFromList(spawnables);
         var position = RandomPointUtils.GetRandomPointWithBox2D(spawnArea);
         var spawnedObject = Instantiate(spawnObject, position, Quaternion.identity); 
            //LeanPool.Spawn(spawnObject, position, Quaternion.identity);
         spawnedObjects.Add(spawnedObject);

         if (mirror)
         {
            spawnedObject.flipX = true;
         }
      }
   }

   private void DespawnRemainders()
   {
      spawnedObjects?.ForEach(Destroy);
   }
}
