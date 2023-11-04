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

        private void Awake()
        {
            AssessUtils.CheckRequirement(ref scoreText, this);
            AssessUtils.CheckRequirement(ref distanceText, this);
        }

        private void Start()
        {
            PlayerControl.GetSingleton().RegisterUpdateScore(UpdateScore);
            PlayerControl.GetSingleton().RegisterUpdateDistance(UpdateDistance);
            UpdateScore(0);
        }

        private void OnDestroy()
        {
            PlayerControl.GetSingleton().UnregisterUpdateScore(UpdateScore);
            PlayerControl.GetSingleton().UnregisterUpdateDistance(UpdateDistance);
        }

        private void UpdateScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        private void UpdateDistance(float distance)
        {
            distanceText.text = $"Distance: {distance:n2}m";
        }
    }
}
