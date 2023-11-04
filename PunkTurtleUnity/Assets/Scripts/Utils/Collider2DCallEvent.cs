using UnityEngine;
using UnityEngine.Events;

public class Collider2DCallEvent : MonoBehaviour
{
   [SerializeField] 
   private UnityEvent<GameObject> callEvent;

   private void OnCollisionEnter2D(Collision2D other)
   {
      Solve(other.gameObject);   
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      Solve(other.gameObject);
   }

   private void Solve(GameObject gameObject)
   {
      callEvent?.Invoke(gameObject);
   }
}
