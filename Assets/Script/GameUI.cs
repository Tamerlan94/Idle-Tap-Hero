using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;

namespace PunchHero
{
    [DefaultExecutionOrder(-2)]
    public class GameUI : MonoBehaviour, IStartEndGame, IRetryGame
    {

        public RectTransform endMenuUI;
        public RectTransform scoreUI;
        public RectTransform pauseButtonUI;
        public RectTransform pauseUI;

        public static event UnityAction<bool> OnRetryGame;
        public static event UnityAction<bool> OnStartEndGame;

        //carousale
        public RectTransform[] rectTransforms;
        public Button[] backButtons;
        public Button[] forwardButtons;

        private int step = 0;
        private float rectWidth;
        private float rectHeight;

        public Button adsButton;
        public Button leaderboardMenu;

        public Button startGame;
        public Button retryGame;
        public Button backToMenu;
        public Button pauseGame;
        public Button unPauseGame;
        public Button exitGame;
        public Button gpsExit;

        private void Start()
        {
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool ok) =>
            {
                if (ok) Debug.Log("success");
                else Debug.Log("not success");
            });

            rectWidth = rectTransforms[0].rect.width;
            rectHeight = rectTransforms[0].rect.height;

            OnStartEndGame += ShowEndMenu;

            foreach (Button item in backButtons)
            {
                item.onClick.AddListener(NextStepMenu);
            }
            foreach (Button item in forwardButtons)
            {
                item.onClick.AddListener(BackStepMenu);
            }

            startGame.onClick.AddListener(() => StartEndGame(true));
            retryGame.onClick.AddListener(() => RetryGame(true));
            backToMenu.onClick.AddListener(BackToMenu);
            pauseGame.onClick.AddListener(() => PauseGame(true));
            unPauseGame.onClick.AddListener(() => PauseGame(false));
            adsButton.onClick.AddListener(() => RetryGame(false));
            leaderboardMenu.onClick.AddListener(ShowLeaderboard);
            exitGame.onClick.AddListener(ExitGame);
            gpsExit.onClick.AddListener(GpsExit);
        }

        private void GpsExit()
        {
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (result) =>
            {
                // handle results
            });
            //PlayGamesPlatform.Instance.SignOut();

        }
        private void ExitGame()
        {
            Application.Quit();
        }
    
        private void ShowLeaderboard()
        {
            Social.ShowLeaderboardUI();
        }

        private void ShowEndMenu(bool t)
        {
            if (!t)
            {
                endMenuUI.DOAnchorPosY(0f, 0.5f);
                scoreUI.DOAnchorPosX(-150f, 0.5f);
                pauseButtonUI.DOAnchorPosX(150f, 0.5f);
            }
            else
            {
                scoreUI.DOAnchorPosX(150f, 0.5f);
                pauseButtonUI.DOAnchorPosX(-20f, 0.5f);
            }

        }

        //сделать через rect.width, посмотреть будет работать или нет
        private void BackStepMenu()
        {
            //rectTransforms[step].DOAnchorMin(new Vector2(-1f, 0.5f), 0.5f);
            //rectTransforms[step].DOAnchorMax(new Vector2(0f, 0.5f), 0.5f);
            rectTransforms[step].DOAnchorPosX(-rectWidth, 0.5f);
            step--;
            if (step < 0)
            {
                step = rectTransforms.Length -1;
            }
            rectTransforms[step].anchoredPosition = new Vector2(rectWidth, 0f);
            rectTransforms[step].DOAnchorPosX(0f, 0.5f);

            //rectTransforms[step].anchorMin = new Vector2(1f, 0.5f);
            //rectTransforms[step].anchorMax = new Vector2(2f, 0.5f);

            //rectTransforms[step].DOAnchorMin(new Vector2(0f, 0.5f), 0.5f);
            //rectTransforms[step].DOAnchorMax(new Vector2(1f, 0.5f), 0.5f);
        }
        private void NextStepMenu()
        {
            //rectTransforms[step].DOAnchorMin(new Vector2(1f, 0.5f), 0.5f);
            //rectTransforms[step].DOAnchorMax(new Vector2(2f, 0.5f), 0.5f);
            rectTransforms[step].DOAnchorPosX(rectWidth, 0.5f);
            step++;
            if (step >= rectTransforms.Length)
            {
                step = 0;
            }
            rectTransforms[step].anchoredPosition = new Vector2(-rectWidth, 0f);
            rectTransforms[step].DOAnchorPosX(0f, 0.5f);
            //rectTransforms[step].anchorMin = new Vector2(-1f, 0.5f);
            //rectTransforms[step].anchorMax = new Vector2(0f, 0.5f);

            //rectTransforms[step].DOAnchorMin(new Vector2(0f, 0.5f), 0.5f);
            //rectTransforms[step].DOAnchorMax(new Vector2(1f, 0.5f), 0.5f);
        }

        public void PauseGame(bool b)
        {
            if (b)
            {
                Time.timeScale = 0;
                pauseUI.DOAnchorPosY(0f, 0.5f).SetUpdate(true);
                scoreUI.DOAnchorPosX(-150f, 0.5f).SetUpdate(true);
                pauseButtonUI.DOAnchorPosX(150f, 0.5f).SetUpdate(true);                
            }
            else
            {
                Time.timeScale = 1;
                pauseUI.DOAnchorPosY(rectHeight, 0.5f).SetUpdate(true);
                scoreUI.DOAnchorPosX(150f, 0.5f).SetUpdate(true);
                pauseButtonUI.DOAnchorPosX(-20f, 0.5f).SetUpdate(true);
            }
           
        }
        public void FromPauseMenu(bool b)
        {
            //b - again, !b - main menu
            Time.timeScale = 1;
            if (b)
            {
                RetryGame(true);
                PauseGame(false);
            }
            else
            {
                BackToMenu();
                EndGame();
                pauseUI.DOAnchorPosY(1080f, 0.5f);
                scoreUI.DOAnchorPosX(-150f, 0.5f);
                pauseButtonUI.DOAnchorPosX(150f, 0.5f);
                endMenuUI.DOAnchorPosY(rectHeight, 0.5f);
            }
        }
        public void BackToMenu()
        {  
            foreach (RectTransform item in rectTransforms)
            {
                item.DOAnchorPosY(0f, 0.5f);
            }
            endMenuUI.DOAnchorPosY(rectHeight, 0.5f);
            scoreUI.DOAnchorPosX(-150f, 0.5f);
            pauseButtonUI.DOAnchorPosX(150f, 0.5f);

            
        }
        public void StartEndGame(bool t)
        {
            //menu
            foreach (RectTransform item in rectTransforms)
            {
                item.DOAnchorPosY(1080f, 0.5f);
            }
            OnStartEndGame?.Invoke(t);
        }
        public void RetryGame(bool t)
        {
            endMenuUI.DOAnchorPosY(rectHeight, 0.5f);
            scoreUI.DOAnchorPosX(150f, 0.5f);
            pauseButtonUI.DOAnchorPosX(-20f, 0.5f);
            OnRetryGame?.Invoke(t);
        }
        public static void EndGame()
        {
            OnStartEndGame?.Invoke(false);
        }
    }
}
