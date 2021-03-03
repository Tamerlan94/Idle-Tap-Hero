using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace PunchHero
{
    //для добавления новых противников, не забыть добавить сюда новые значения
    public enum Enemy
    {
        Enemy1,
        Enemy2,
        Enemy3,
        Enemy4,
        Enemy5
    }
    public class EnemySpawner : MonoBehaviour, IStartEndGame, IRetryGame
    {
        public Transform[] sides;

        private bool isGameStarted;
        private float timer = 0f;
        private float timerEnd = 5f;

        private int levelMiltiply = 7;
        private int spawnNumber = 0;
        private int levelNumber = 1;
        private static float enemySpeed = 3f;

        public CanvasGroup levelObj;
        public Text levelText;

        private void Start()
        {
            GameUI.OnStartEndGame += StartEndGame;
            GameUI.OnRetryGame += RetryGame;

            levelText.text = levelNumber.ToString();
        }

        public void StartEndGame(bool t)
        {
            isGameStarted = t;
            FadeCanvas(t);
        }
        private void FadeCanvas(bool t)
        {
            if (!t)
            {
                levelObj.DOFade(0f, 1f);
            }
            else
            {
                levelObj.DOFade(1f, 1f);
            }
        }
        private void Update()
        {
            if (isGameStarted)
            {
                timer += Time.deltaTime;
                if (timer > timerEnd)
                {
                    SpawnEnemy();  
                    timer = 0f;
                }
            }            
        }

        private void SpawnEnemy()
        {
            int random = Random.Range(0, sides.Length);
            int enemyRandom = Random.Range(0, System.Enum.GetValues(typeof(Enemy)).Length);
            string randomEnemyName = System.Enum.GetName(typeof(Enemy),enemyRandom);
            GameObject obj;
            switch (random)
            {
                case 0:
                    obj = ObjectPooler.SharedInstance.GetPooledObject(randomEnemyName);
                    if (obj != null)
                    {
                        //left
                        obj.transform.position = sides[0].transform.position;
                        obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                        obj.GetComponent<EnemyController>().SetTargetMove(1f);
                        obj.SetActive(true);
                    }
                    break;
                case 1:
                    obj = ObjectPooler.SharedInstance.GetPooledObject(randomEnemyName);
                    if (obj != null)
                    {
                        //right
                        obj.transform.position = sides[1].transform.position;
                        obj.transform.rotation = Quaternion.Euler(0f, 0, 0f);
                        obj.GetComponent<EnemyController>().SetTargetMove(-1f);
                        obj.SetActive(true);
                    }
                    break;
                default: obj = null;
                    break;
            }

            //уровень сложности
            spawnNumber++;
            if (spawnNumber > levelMiltiply)
            {                
                if (timerEnd > 1)
                {
                    timerEnd -= 0.5f;
                    enemySpeed++;
                }

                levelText.text = (++levelNumber).ToString();
                spawnNumber = 0;
            }
        }
        public static float SetSpeed()
        {
            return enemySpeed;
        }

        public void RetryGame(bool t)
        {
            if (t)
            {
                timerEnd = 5f;
                levelNumber = 1;
                enemySpeed = 3f;
                levelText.text = levelNumber.ToString();
                spawnNumber = 0;
                isGameStarted = true;
                levelObj.DOFade(1f, 1f);
                ObjectPooler.SharedInstance.DisablePooledObject();
            }
            else
            {
                //second chance
                isGameStarted = true;
                levelObj.DOFade(1f, 1f);
                ObjectPooler.SharedInstance.DisablePooledObject();
            }
        }
    }
}
