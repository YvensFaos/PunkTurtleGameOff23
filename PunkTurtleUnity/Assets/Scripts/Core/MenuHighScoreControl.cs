using TMPro;
using UnityEngine;

namespace Core
{
   public class MenuHighScoreControl : MonoBehaviour
   {
      [SerializeField]
      private TextMeshProUGUI score;
      [SerializeField]
      private TextMeshProUGUI distance;

      private void Start()
      {
         score.text = $"Score: {GameManager.GetSingleton().MaxScore}";
         distance.text = $"Distance: {GameManager.GetSingleton().MaxDistance:n2}m";
      }
   }
}
