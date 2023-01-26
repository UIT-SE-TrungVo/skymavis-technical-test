using System;
using System.Collections.Generic;
using Assignment.ScriptableObjects;
using Assignment.Sound.Enum;
using UnityEngine;

namespace Assignment.Sound
{
    public class SoundPlayer : MonoBehaviour
    {
        #region FIELDS

        private static SoundPlayer instance;

        [SerializeField] private SoundConfig config;

        private AudioSource audioSource;
        private bool isAudioNull;
        private HashSet<SoundCategory> nextPlaySounds = new HashSet<SoundCategory>();

        #endregion

        #region PROPERTIES

        public static SoundPlayer Instance
        {
            get => instance;
            private set => instance = value;
        }

        #endregion

        #region UNITY EVETNS

        private void Awake()
        {
            Instance = this;

            this.audioSource = this.GetComponent<AudioSource>();
            this.isAudioNull = (this.audioSource == null);
        }

        private void LateUpdate()
        {
            if (this.isAudioNull || this.nextPlaySounds.Count <= 0) return;
            foreach (SoundCategory soundCategory in this.nextPlaySounds)
            {
                switch (soundCategory)
                {
                    case SoundCategory.Hit:
                        this.PlayHitSound();
                        break;
                    default:
                        break;
                }
            }

            nextPlaySounds.Clear();
        }

        #endregion

        #region METHODS

        public static void AssignPlaySound(SoundCategory soundCategory)
        {
            if (Instance == null || Instance.isAudioNull) return;
            Instance.AddSoundCategory(soundCategory);
        }

        private void AddSoundCategory(SoundCategory soundCategory)
        {
            if (nextPlaySounds.Contains(soundCategory)) return;
            nextPlaySounds.Add(soundCategory);
            nextPlaySounds.Add(soundCategory);
        }

        private void PlayHitSound()
        {
            List<AudioClip> listHitSound = this.config.sfx.hitSfx;
            if (listHitSound.Count <= 0) return;
            AudioClip hitSound = RandomHelper.GetRandomElementFromList(listHitSound);
            this.audioSource.PlayOneShot(hitSound);
        }

        #endregion
    }
}