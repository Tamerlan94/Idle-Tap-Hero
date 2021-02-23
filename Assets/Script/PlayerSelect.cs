using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PunchHero
{
    public class PlayerSelect : MonoBehaviour
    {
        public SpriteRenderer playerRen;
        public Animator playerAnim;

        public PlayerData[] playerDatas;

        public Image image;

        public Text costText;
        public Text selectText;
        public Text lockedText;

        private int selectedHero;
        private int step = 0;

        public Button leftButton;
        public Button rightButton;
        public Button selectButton;

        private void Start()
        {
            selectedHero = PlayerPrefs.GetInt("selectedHero", 0);
            step = selectedHero;
            SelectHero();
            lockedText.text = "Unlocked";

            image.sprite = playerDatas[step].icon;
            costText.text = playerDatas[step].cost.ToString();

            leftButton.onClick.AddListener(LeftStep);
            rightButton.onClick.AddListener(RightStep);
            selectButton.onClick.AddListener(SelectHero);
        }

        public void LeftStep()
        {            
            step--;            
            if (step < 0)
            {
                step = 0;
            }
            image.sprite = playerDatas[step].icon;
            costText.text = playerDatas[step].cost.ToString();

            if (step == selectedHero)
            {
                selectText.text = "Selected";
            }
            else
            {
                selectText.text = "Select";
            }

            int score = PlayerPrefs.GetInt("bestScore");
            if (score >= playerDatas[step].cost)
            {
                selectButton.interactable = true;
                lockedText.text = "Unlocked";
                costText.color = new Color32(51, 50, 61, 255);
            }
            else
            {
                selectButton.interactable = false;
                lockedText.text = "locked";
                costText.color = Color.red;
            }

        }
        public void RightStep()
        {
            step++;
            if (step > playerDatas.Length - 1)
            {
                step = playerDatas.Length - 1;
            }
            image.sprite = playerDatas[step].icon;
            costText.text = playerDatas[step].cost.ToString();

            if (step == selectedHero)
            {
                selectText.text = "Selected";
            }
            else
            {
                selectText.text = "Select";
            }

            int score = PlayerPrefs.GetInt("bestScore");
            if (score > playerDatas[step].cost)
            {
                selectButton.interactable = true;
                lockedText.text = "Unlocked";
                costText.color = new Color32(51, 50, 61, 255);
            }
            else
            {
                selectButton.interactable = false;
                lockedText.text = "locked";
                costText.color = Color.red;
            }
        }
        public void SelectHero()
        {
            playerRen.sprite = playerDatas[step].icon;
            playerAnim.runtimeAnimatorController = playerDatas[step].animator;
            PlayerPrefs.SetInt("selectedHero", step);
            selectedHero = step;
            selectText.text = "Selected";
        }
    }
}
