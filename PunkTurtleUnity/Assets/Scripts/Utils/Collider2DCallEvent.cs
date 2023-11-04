using UnityEngine;
using UnityEngine.Events;
using Utils;

public class Collider2DCallEvent : AgnosticCollisionSolver2D
{
   [SerializeField] 
   private UnityEvent<GameObject> callEvent;

   protected override void Solve(GameObject gameObject)
   {
      callEvent?.Invoke(gameObject);
   }
}
