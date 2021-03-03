using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using DG.Tweening;

namespace PunchHero
{
    public class AdsScript : MonoBehaviour, IStartEndGame, IRetryGame, IUnityAdsListener
    {
        private readonly string gameID = "4022479";
        private readonly string bannerPlacement = "banner";
        private readonly string rewardedVideoPlacement = "rewardedVideo";
        private readonly string defaultPlacement = "video";

        private int attempt = 0;
        private int count = 0;
        private bool isWatched;

        public Button secondChance;
        public Transform recoveryImage;

        private void Start()
        {
            //GameUI.OnStartEndGame += StartEndGame;
            EnemyController.OnEnemyDie += EnemyCount;
            GameUI.OnRetryGame += RetryGame;
            secondChance.onClick.AddListener(ShowRewardedVideo);

            secondChance.gameObject.SetActive(false);
            recoveryImage.gameObject.SetActive(false);

            Advertisement.AddListener(this);
            Advertisement.Initialize(gameID);
            StartCoroutine(ShowBannerWhenInitialized());
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        }

        private void EnemyCount()
        {
            count++;
            if (count > 8)
            {
                if(!isWatched)
                    secondChance.gameObject.SetActive(true);
            }
        }

        IEnumerator ShowBannerWhenInitialized()
        {
            while (!Advertisement.isInitialized)
            {
                yield return new WaitForSeconds(0.5f);
            }
            Advertisement.Banner.Show(bannerPlacement);
        }

        public void ShowInterstitialAd()
        {
            if (Advertisement.IsReady())
            {
                Advertisement.Show(defaultPlacement);
            }
            else
            {
                Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
            }
        }

        public void ShowRewardedVideo()
        {
            if (Advertisement.IsReady(rewardedVideoPlacement))
            {
                Advertisement.Show(rewardedVideoPlacement);
            }
            else
            {
                Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
            }
        }

        public void StartEndGame(bool t)
        {
            //if (!t)
            //{
            //    attempt++;          
            //}
            //if (attempt > 3)
            //{
            //    ShowInterstitialAd();
            //    attempt = 0;
            //    count = 0;
            //}
        }

        public void OnUnityAdsReady(string placementId)
        {
            // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
            if (placementId == rewardedVideoPlacement)
            {
                // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)     
               
            }
        }

        public void OnUnityAdsDidError(string message)
        {
            // Log the error.
            Debug.Log(message);
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            // Optional actions to take when the end-users triggers an ad.
            if (placementId == rewardedVideoPlacement)
            {
                secondChance.gameObject.SetActive(false);
                isWatched = true;
                Time.timeScale = 0;
            }
            if (placementId == defaultPlacement)
            {
                Time.timeScale = 0;
            }
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            // Define conditional logic for each ad completion status:
            if (showResult == ShowResult.Finished && placementId == rewardedVideoPlacement)
            {
                // Reward the user for watching the ad to completion.   
                recoveryImage.gameObject.SetActive(true);
                StartCoroutine(TimeDefault());
            }
            else if (showResult == ShowResult.Skipped && placementId == rewardedVideoPlacement)
            {
                // Do not reward the user for skipping the ad.
                recoveryImage.gameObject.SetActive(false);
                GameUI.EndGame();
                Time.timeScale = 1;
            }
            else if (showResult == ShowResult.Failed && placementId == rewardedVideoPlacement)
            {
                Debug.LogWarning("The ad did not finish due to an error.");
                StartCoroutine(TimeDefault());
            }

            if ((showResult == ShowResult.Failed || showResult == ShowResult.Finished || showResult == ShowResult.Skipped) && placementId == defaultPlacement)
            {
                StartCoroutine(TimeDefault());
            }
        }
        public void OnDestroy()
        {
            Advertisement.RemoveListener(this);
        }
        IEnumerator TimeDefault()
        {            
            yield return new WaitForSecondsRealtime(2);
            Time.timeScale = 1;
        }

        public void RetryGame(bool t)
        {
            attempt++;
            if (attempt > 3)
            {
                ShowInterstitialAd();
                attempt = 0;
                count = 0;
            }
        }
    }
}
