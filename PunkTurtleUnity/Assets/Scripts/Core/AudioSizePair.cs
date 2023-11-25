using System;
using UnityEngine;
using Utils;

namespace Core
{
    [Serializable]
    public class AudioSizePair : Pair<AudioClip, float>
    {
        public AudioSizePair(AudioClip one, float two) : base(one, two)
        { }
    }
}