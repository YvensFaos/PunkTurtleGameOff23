using System.Collections.Generic;
using Core;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class SpawnAreaControl : MonoBehaviour
{
   [SerializeField]
   private BoxCollider2D spawnArea;
   [SerializeField]
   private List<SpawnObject> spawnables;
   [SerializeField]
   private int minSpawns;
   [SerializeField]
   private int maxSpawns;
   [SerializeField] 
   private float chance;
   [SerializeField]
   private bool mirror;

   private List<SpawnObject> spawnedObjects;
   private RaycastHit2D[] cashRaycastHit;
   private const int SAFE_CHECK = 10;

   private void Awake()
   {
      cashRaycastHit = new RaycastHit2D[1];
   }
   
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
      if (spawnedObjects != null)
      {
         DespawnRemainders();   
      }
      spawnedObjects = new List<SpawnObject>();

      if (chance > 0.0f)
      {
         if (!RandomChanceUtils.GetChance(chance))
         {
            return;
         }
      }
      var spawnCount = Random.Range(minSpawns, maxSpawns);
      
      for (var i = 0; i < spawnCount; i++)
      {
         var spawnObject = RandomHelper<SpawnObject>.GetRandomFromList(spawnables);
         var position = RandomPointUtils.GetRandomPointWithBox2D(spawnArea);
         var spawnedObject = Instantiate(spawnObject, position, Quaternion.identity); 
         if (spawnedObject.HasCollider)
         {
            var safeCheck = SAFE_CHECK;
            var collider = spawnedObject.SpawnCollider2D;
            while (--safeCheck > 0 &&
                   !AgnosticCollisionSolver2D.ValidPosition(position, collider.bounds, ref cashRaycastHit))
            {
               position = RandomPointUtils.GetRandomPointWithBox2D(spawnArea);
            }

            if (safeCheck >= 0)
            {
               spawnedObject.transform.position = position;
            }
            else
            {
               DebugUtils.DebugLogErrorMsg($"{spawnedObject.name} could not find a suitable position. Collided with {cashRaycastHit[0].transform.name}.");
            }
         }   
         //LeanPool.Spawn(spawnObject, position, Quaternion.identity);
            
         spawnedObjects.Add(spawnedObject);

         if (mirror)
         {
            spawnedObject.SpawnSpriteRenderer.flipX = true;
         }
      }
   }

   private void DespawnRemainders()
   {
      if (spawnedObjects == null) return;
      
      spawnedObjects = spawnedObjects.FindAll(sprite => sprite != null);
      spawnedObjects?.ForEach(sprite =>
      {
         if (sprite != null)
         {
            Destroy(sprite.gameObject);   
         }
      });
   }
}
