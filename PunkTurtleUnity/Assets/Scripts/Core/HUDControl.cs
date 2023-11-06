using DG.Tweening;
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
        [SerializeField]
        private GameOverPanelControl gameOverPanel;

        private void Awake()
        {
            AssessUtils.CheckRequirement(ref scoreText, this);
            AssessUtils.CheckRequirement(ref distanceText, this);
            AssessUtils.CheckRequirement(ref shellPlacer, this);
            AssessUtils.CheckRequirement(ref gameOverPanel, this);
        }

        private void Start()
        {
            var playerControl = PlayerControl.GetSingleton();
            playerControl.RegisterUpdateScore(UpdateScore);
            playerControl.RegisterUpdateDistance(UpdateDistance);
            playerControl.RegisterUpdateLives(UpdateLives);
            playerControl.RegisterGameOverEvent(GameOver);
            
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
            playerControl.UnregisterGameOverEvent(GameOver);
        }

        private void UpdateScore(int score)
        {
            scoreText.text = $"Score: {score}";
            scoreText.rectTransform.DOPunchScale(new Vector3(1.05f, 1.05f), 0.3f, 1, 0).OnComplete(() =>
            {
                scoreText.rectTransform.localScale = Vector3.one;
            });
        }

        private void UpdateDistance(float distance)
        {
            distanceText.text = $"Distance: {distance:n2}m";
        }

        private void UpdateLives(int lives)
        {
            shellPlacer.UpdateLives(lives);
        }

        private void GameOver(int score, float distance)
        {
            gameOverPanel.gameObject.SetActive(true);
            gameOverPanel.GameOver(score, distance);   
        }
    }
}
