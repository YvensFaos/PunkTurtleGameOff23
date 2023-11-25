using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Core
{
    public class PlayerAudioControl : MonoBehaviour
    {
        [SerializeField] 
        private AudioSource source;
        
        [SerializeField]
        private List<AudioSizePair> eatSounds;

        private void Awake()
        {
            AssessUtils.CheckRequirement(ref source, this);
        }

        public void PlayEatSound(float normalizedSize)
        {
            foreach (var eatPair in eatSounds.Where(eatPair => normalizedSize <= eatPair.Two))
            {
                source.PlayOneShot(eatPair.One);
                break;
            }
        }
    }
}
