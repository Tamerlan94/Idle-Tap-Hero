using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

namespace PunchHero
{
    public class ScoreUI : MonoBehaviour, IStartEndGame, IRetryGame
    {
        public Text scoreText;
        public Text scoreTextEnd;
        public Text bestScoreText;
        private int score;
        private int bestScore;

        public Text scoreTextEndPause;
        public Text bestScoreTextPause;

        private void Start()
        {
            EnemyController.OnEnemyDie += AddScore;
            GameUI.OnRetryGame += RetryGame;
            GameUI.OnStartEndGame += StartEndGame;

            bestScore = PlayerPrefs.GetInt("bestScore", 0);
            bestScoreText.text = bestScore.ToString();

            scoreTextEndPause.text = score.ToString();
            bestScoreTextPause.text = bestScore.ToString();
        }               

        private void AddScore()
        {
            score += 1;

            scoreText.text = score.ToString("0000");
            scoreTextEnd.text = score.ToString();

            scoreTextEndPause.text = score.ToString();

            if (score > bestScore)
            {
                bestScore = score;
                PlayerPrefs.SetInt("bestScore", bestScore);
                bestScoreText.text = bestScore.ToString();
                bestScoreTextPause.text = bestScore.ToString();
            }
        }

        public void RetryGame(bool t)
        {
            if (t)
            {
                score = 0;
                scoreText.text = score.ToString("0000");
                scoreTextEnd.text = score.ToString();
                scoreTextEndPause.text = score.ToString();
            }
            
        }

        public void StartEndGame(bool t)
        {
            if (t)
            {
                score = 0;
                scoreText.text = score.ToString("0000");
                scoreTextEnd.text = score.ToString();
                scoreTextEndPause.text = score.ToString();
            }
            else
            {
                if (score >= bestScore)
                {
                    Social.ReportScore(bestScore, GPGSIds.leaderboard_best_score, (bool success) =>
                    {
                        // handle success or failure
                        if (success) PlayerPrefs.SetInt("bestScore", bestScore);
                        else Debug.Log("not success");
                    });
                }                
            }
           
        }
    }
}
