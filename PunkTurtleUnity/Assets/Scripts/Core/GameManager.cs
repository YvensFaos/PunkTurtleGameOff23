using NaughtyAttributes;
using UnityEngine;
using Utils;

namespace Core
{
    public class GameManager: Singleton<GameManager>
    {
        private void Awake()
        {
            ControlSingleton();
        }

        private void Start()
        {
            maxScore = PlayerPrefs.GetInt("MaxScore");
            maxDistance = PlayerPrefs.GetFloat("MaxDistance");
        }

        [SerializeField, ReadOnly]
        private int maxScore;
        [SerializeField, ReadOnly]
        private float maxDistance;

        public bool NewScore(int score, float distance)
        {
            if (score < MaxScore && distance < maxDistance) return false;
            
            maxScore = score;
            maxDistance = distance;
            PlayerPrefs.SetInt("MaxScore", score);
            PlayerPrefs.SetFloat("MaxDistance", distance);
            return true;
        }

        public int MaxScore => maxScore;
        public float MaxDistance => maxDistance;
    }
}