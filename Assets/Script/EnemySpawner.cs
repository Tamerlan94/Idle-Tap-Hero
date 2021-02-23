using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private float timerEnd = 3f;

        private int levelMiltiply = 15;
        private int spawnNumber = 0;

        private void Start()
        {
            GameUI.OnStartEndGame += StartEndGame;
            GameUI.OnRetryGame += RetryGame;
        }

        public void StartEndGame(bool t)
        {
            isGameStarted = t;
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
                timerEnd = Random.Range(1, 5);
                levelMiltiply = Random.Range(5, 21);
                spawnNumber = 0;
            }
        }

        public void RetryGame()
        {
            timerEnd = 3f;
            levelMiltiply = 15;
            spawnNumber = 0;
            isGameStarted = true;
            ObjectPooler.SharedInstance.DisablePooledObject();
        }
    }
}
