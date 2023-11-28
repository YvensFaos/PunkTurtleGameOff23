using NaughtyAttributes;
using UnityEngine;
using Utils;

namespace Core
{
    public class GameManager: Singleton<GameManager>
    {
        [SerializeField, ReadOnly]
        private int maxScore;
        [SerializeField, ReadOnly]
        private float maxDistance;
        
        private void Awake()
        {
            ControlSingleton();
        }

        private void Start()
        {
            maxScore = PlayerPrefs.GetInt("MaxScore");
            maxDistance = PlayerPrefs.GetFloat("MaxDistance");
        }
        
        public bool NewScore(int score, float distance)
        {
            if (score < MaxScore && distance < maxDistance) return false;
            
            maxScore = score;
            maxDistance = distance;
            PlayerPrefs.SetInt("MaxScore", score);
            PlayerPrefs.SetFloat("MaxDistance", distance);

            return true;
        }

        [Button("Reset Score")]
        public void ResetScore()
        {
            maxScore = 0;
            maxDistance = 0.0f;
            PlayerPrefs.SetInt("MaxScore", maxScore);
            PlayerPrefs.SetFloat("MaxDistance", maxDistance);
        }

        public int MaxScore => maxScore;
        public float MaxDistance => maxDistance;
    }
}