using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace PunchHero
{
    public class HealthUI : MonoBehaviour, IStartEndGame, IRetryGame
    {
        public Sprite fullHealth;
        public Sprite emptyHealth;

        private Image[] health;
        private CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            health = new Image[3];
            health = GetComponentsInChildren<Image>();
            for (int i = 0; i < health.Length; i++)
            {
                health[i].sprite = fullHealth;
            }

            PlayerController.OnDamageTaken += ChangeHealth;
            GameUI.OnStartEndGame += StartEndGame;
            GameUI.OnRetryGame += RetryGame;
        }

        private void ChangeHealth(int one)
        {
            if (one >= 0)
            {
                health[one].sprite = emptyHealth;
            }            
        }

        public void StartEndGame(bool t)
        {
            if (t)
            {
                canvasGroup.DOFade(1f, 1f);
            }          
            else
                canvasGroup.DOFade(0f, 1f);
        }

        public void RetryGame(bool t)
        {
            canvasGroup.DOFade(1f, 1f);
            for (int i = 0; i < health.Length; i++)
            {
                health[i].sprite = fullHealth;
            }
        }
        public void BackToMenu()
        {
            canvasGroup.DOFade(0f, 1f);
            for (int i = 0; i < health.Length; i++)
            {
                health[i].sprite = fullHealth;
            }
        }
    }
}
