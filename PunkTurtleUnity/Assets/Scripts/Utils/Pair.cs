using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class Pair<TA, TB>
    {
        [SerializeField] private TA one;
        [SerializeField] private TB two;

        public Pair(TA one, TB two)
        {
            One = one;
            Two = two;
        }

        public TA One
        {
            get => one;
            set => one = value;
        }

        public TB Two
        {
            get => two;
            set => two = value;
        }
    }
}