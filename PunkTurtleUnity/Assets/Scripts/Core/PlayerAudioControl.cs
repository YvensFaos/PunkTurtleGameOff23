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
        private AudioSource sourceHit;
        
        [Header("Database")]
        [SerializeField]
        private List<AudioSizePair> eatSounds;
        [SerializeField]
        private List<AudioSizePair> transformSounds;
        [SerializeField]
        private AudioClip collectableSound;
        [SerializeField]
        private AudioClip shellSound;
        [SerializeField]
        private List<AudioClip> hitSounds;

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
        
        public void PlayTransformSound(float normalizedSize)
        {
            foreach (var transformPair in transformSounds.Where(eatPair => normalizedSize <= eatPair.Two))
            {
                source.PlayOneShot(transformPair.One);
                break;
            }
        }

        public void PlayCollectable()
        {
            source.PlayOneShot(collectableSound);
        }

        public void PlayShell()
        {
            source.PlayOneShot(shellSound);
        }

        public void PlayHitSound()
        {
            sourceHit.pitch = Random.Range(0.9f, 1.1f);
            sourceHit.PlayOneShot(RandomHelper<AudioClip>.GetRandomFromList(hitSounds));
        }
    }
}
