using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace PunchHero
{
    public class Recovery : MonoBehaviour
    {
        private Image image;

        private void Start()
        {
            image = GetComponent<Image>();
            transform.DOScaleX(1f, 1.5f);
            image.DOFade(0f, 2.5f);
        }
        private void OnEnable()
        {
            if (image != null)
            {
                transform.DOScaleX(1f, 1.5f);
                image.DOFade(0f, 2.5f);
            }
        }
        private void OnDisable()
        {
            transform.DOScaleX(300f, 1.5f);
            image.DOFade(1f, 1f);
        }
    }
}
