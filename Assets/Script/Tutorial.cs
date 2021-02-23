using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PunchHero
{
    public class Tutorial : MonoBehaviour, IStartEndGame
    {
        private CanvasGroup canvasGroup;
        private bool pressed;

        public Transform leftTransform;
        public Transform rightTransform;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            GameUI.OnStartEndGame += StartEndGame;
            InputManager.Instance.OnTouchPressed += FadeTutorial;
        }

        private void FadeTutorial(Vector2 arg0)
        {
            if (pressed)
            {
                canvasGroup.DOFade(0f, .5f);
                pressed = false;
                DOTween.Pause(leftTransform);
                DOTween.Pause(rightTransform);
            }
        }

        public void StartEndGame(bool t)
        {
            if (t)
            {
                canvasGroup.DOFade(1f, .5f);
                pressed = true;
                leftTransform.DOLocalMoveX(-500f, 1f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.Linear);
                rightTransform.DOLocalMoveX(500f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            }
        }

    }
}
