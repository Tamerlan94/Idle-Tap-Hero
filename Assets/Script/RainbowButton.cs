using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace PunchHero
{
    public class RainbowButton : MonoBehaviour
    {
        private Image image;

        private void Start()
        {
            image = GetComponent<Image>();
            image.DOColor(new Color32(42, 255, 0, 255), 0.3f).SetLoops(-1);
        }
        private void OnEnable()
        {
            DOTween.Play(image);
        }
        private void OnDisable()
        {
            DOTween.Pause(image);
        }
    }
}
