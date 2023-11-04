using UnityEngine;
using UnityEngine.Events;
using Utils;

public class Collider2DCallEvent : MonoBehaviour
{
   [SerializeField] 
   private UnityEvent<GameObject> callEvent;

   private void OnCollisionEnter2D(Collision2D other)
   {
      DebugUtils.DebugLogMsg($"Other {other.gameObject.name}");
      Solve(other.gameObject);   
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      DebugUtils.DebugLogMsg($"Other {other.gameObject.name}");
      Solve(other.gameObject);
   }

   private void Solve(GameObject gameObject)
   {
      callEvent?.Invoke(gameObject);
   }
}
