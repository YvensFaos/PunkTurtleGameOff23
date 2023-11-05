using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Core
{
    public class GameOverPanelControl : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI scoreText;
        [SerializeField]
        private TextMeshProUGUI distanceText;
        
        public void GameOver(int score, float distance)
        {
            scoreText.text = $"Score: {score}";
            distanceText.text = $"Distance: {distance:n2}m";
        }

        public void Restart()
        {
            DebugUtils.DebugLogMsg("Restart!");
            //Resets the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}