using TMPro;
using UnityEngine;
using Utils;

namespace Core
{
    public class HUDControl : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI scoreText;
        [SerializeField]
        private TextMeshProUGUI distanceText;
        [SerializeField]
        private LivesPanelControl shellPlacer;

        private void Awake()
        {
            AssessUtils.CheckRequirement(ref scoreText, this);
            AssessUtils.CheckRequirement(ref distanceText, this);
        }

        private void Start()
        {
            var playerControl = PlayerControl.GetSingleton();
            playerControl.RegisterUpdateScore(UpdateScore);
            playerControl.RegisterUpdateDistance(UpdateDistance);
            playerControl.RegisterUpdateLives(UpdateLives);
            
            //Force an update on the amount of lives to initialize this component
            playerControl.UpdateLives(0);
            playerControl.UpdateScore(0);
        }

        private void OnDestroy()
        {
            var playerControl = PlayerControl.GetSingleton();
            playerControl.UnregisterUpdateScore(UpdateScore);
            playerControl.UnregisterUpdateDistance(UpdateDistance);
            playerControl.UnregisterUpdateLives(UpdateLives);
        }

        private void UpdateScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        private void UpdateDistance(float distance)
        {
            distanceText.text = $"Distance: {distance:n2}m";
        }

        private void UpdateLives(int lives)
        {
            shellPlacer.UpdateLives(lives);
        }
    }
}
