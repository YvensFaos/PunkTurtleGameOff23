using System;
using Utils;

namespace Core
{
    [Serializable]
    public class BackgroundSpawnerChancePair : Pair<BackgroundSpawner, float>
    {
        public BackgroundSpawnerChancePair(BackgroundSpawner one, float two) : base(one, two)
        { }
    }
}