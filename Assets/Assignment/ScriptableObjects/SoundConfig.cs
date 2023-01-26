using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Sound", menuName = "GameConfiguration/Sound", order = 0)]
    public class SoundConfig : ScriptableObject
    {
        public SfxConfigInfo sfx;
    }

    [Serializable]
    public struct SfxConfigInfo
    {
        public List<AudioClip> hitSfx;
    }
}