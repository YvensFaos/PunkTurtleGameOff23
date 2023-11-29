using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] 
        private GameObject dashTextObject;
        [SerializeField] 
        private GameObject invincibilityTextObject;
        [SerializeField]
        private GameObject doubleTextObject;
        [SerializeField] 
        private GameObject newRankingObject;

        private Coroutine textDisplayCoroutine;
        private Tweener imageTween;

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
            playerControl.RegisterGetCollectableEvent(GetCollectable);
            
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
            playerControl.UnregisterGetCollectableEvent(GetCollectable);
        }

        private void UpdateScore(int score)
        {
            scoreText.text = $"Score: {score}";
            scoreText.rectTransform.DOPunchScale(new Vector3(0.2f, 0.00f), 0.3f, 1, 0).OnComplete(() =>
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

            var newHighScore = GameManager.GetSingleton().NewScore(score, distance);
            DebugUtils.DebugLogMsg($"New High Score! {newHighScore}");
            newRankingObject.SetActive(newHighScore);
        }

        private void GetCollectable(CollectableControl collectable)
        {
            switch (collectable)
            {
                case DashCollectableControl:
                    StopCurrentPowerUp();
                    textDisplayCoroutine = StartCoroutine(DisplayForATime(dashTextObject));
                    break;
                case InvincibleCollectableControl:
                    StopCurrentPowerUp();
                    textDisplayCoroutine = StartCoroutine(DisplayForATime(invincibilityTextObject));
                    break;
                case DoublePointsCollectableControl:
                    StopCurrentPowerUp();
                    textDisplayCoroutine = StartCoroutine(DisplayForATime(doubleTextObject));
                    break;
            }
        }

        private void StopCurrentPowerUp()
        {
            dashTextObject.SetActive(false);
            invincibilityTextObject.SetActive(false);
            doubleTextObject.SetActive(false);

            if (textDisplayCoroutine != null)
            {
                StopCoroutine(textDisplayCoroutine);
            }
        }

        private IEnumerator DisplayForATime(GameObject textObject)
        {
            var image = textObject.GetComponent<Image>();
            image.color = Color.white;
            imageTween?.Kill();
            textObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            imageTween = image.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.0f), 1.0f);
            yield return new WaitForSeconds(1.0f);
            textObject.SetActive(false);
        }
    }
}
