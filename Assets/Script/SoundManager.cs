using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PunchHero
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager SharedInstance;

        public Button musicButton;
        public Button sfxButton;

        public Image musicImage;
        public Image sfxImage;

        public Sprite offSprite;
        public Sprite onSprite;

        private bool savedSound = false;
        private bool savedSfx = false;

        public AudioClip music;

        void Awake()
        {
            SharedInstance = this;
        }

        private AudioSource[] audioSource; //0 - sfx, 1 - music

        private void Start()
        {
            audioSource = GetComponents<AudioSource>();
            musicButton.onClick.AddListener(() => MuteUnmuteSound(0));
            sfxButton.onClick.AddListener(() => MuteUnmuteSound(1));

            audioSource[1].clip = music;
            audioSource[1].Play();

            savedSound = Convert.ToBoolean(PlayerPrefs.GetInt("savedSound", 0));
            savedSfx = Convert.ToBoolean(PlayerPrefs.GetInt("savedSfx", 0));

            CheckSprite();
            CheckSound();
        }
        private void CheckSprite()
        {
            if (savedSound)
            {
                musicImage.sprite = offSprite;
            }
            else
            {
                musicImage.sprite = onSprite;
            }

            if (savedSfx)
            {
                sfxImage.sprite = offSprite;
            }
            else
            {
                sfxImage.sprite = onSprite;
            }
        }
        public void PlaySfx(AudioClip audioClip)
        {
            audioSource[0].PlayOneShot(audioClip);
        } 
        private void CheckSound()
        {
            if (savedSound)
            {
                audioSource[1].mute = true;
            }
            if (savedSfx)
            {
                audioSource[0].mute = true;
            }
        }
        private void MuteUnmuteSound(int num)
        {
            //false - on, true - off music
            switch (num)
            {
                case 0:
                    savedSound = !savedSound;
                    if (savedSound)
                    {
                        musicImage.sprite = offSprite;
                        audioSource[1].mute = true;
                        PlayerPrefs.SetInt("savedSound", Convert.ToInt32(savedSound));
                    }
                    else
                    {
                        musicImage.sprite = onSprite;
                        audioSource[1].mute = false;
                        PlayerPrefs.SetInt("savedSound", Convert.ToInt32(savedSound));
                    }
                    break;
                case 1:
                    savedSfx = !savedSfx;
                    if (savedSfx)
                    {
                        sfxImage.sprite = offSprite;
                        audioSource[0].mute = true;
                        PlayerPrefs.SetInt("savedSfx", Convert.ToInt32(savedSfx));
                    }
                    else
                    {
                        sfxImage.sprite = onSprite;
                        audioSource[0].mute = false;
                        PlayerPrefs.SetInt("savedSfx", Convert.ToInt32(savedSfx));
                    }
                    break;
                default:
                    break;
            }
            
        }
    }
}
