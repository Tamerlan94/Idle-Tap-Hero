using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PunchHero
{
    public class PlayerController : MonoBehaviour, IStartEndGame, IRetryGame
    {
        public static event UnityAction<int> OnDamageTaken;
        //public static event UnityAction<bool> OnPlayerDie;

        public int health = 3;
        public bool facingRight = true;

        public Transform attackPoint;
        public float attackRadius;
        public LayerMask enemyLayer;

        private Animator animator;
        private bool isStartedGame;

        public AudioClip attackSound1;
        public AudioClip attackSound2;


        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            GameUI.OnStartEndGame += StartEndGame;
            GameUI.OnRetryGame += RetryGame;
        }

        public void StartEndGame(bool b)
        {
            isStartedGame = b;
        }

        private void OnEnable()
        {
            InputManager.Instance.OnTouchPressed += Attack;
        }
        private void OnDisable()
        {
            InputManager.Instance.OnTouchPressed -= Attack;
        }

        public void Attack(Vector2 pos)
        {
            if (isStartedGame)
            {
                if (pos.x > Screen.width / 2)
                {
                    if (!facingRight)
                        Flip(facingRight = true);

                    HitEnemyAnimation();                  
                }
                else if (pos.x < Screen.width / 2)
                {
                    if (facingRight)
                        Flip(facingRight = false);

                    HitEnemyAnimation();                   
                }
            }            
        }
        private void Flip(bool f)
        {
            transform.Rotate(0f, 180f, 0f);
            facingRight = f;
        }
        public void TakeDamage()
        {
            health -= 1;
            OnDamageTaken?.Invoke(health);

            //animation
            animator.SetTrigger("GetHit");

            if (health <= 0)
            {
                Die();
            }
        }
        private void Die()
        {       
            //animation
            animator.SetBool("isDie", true);
            GameUI.EndGame();
        }
        private void HitEnemyAnimation()
        {
            //animation
            int random = Random.Range(0, 2);
            switch (random)
            {
                case 0: animator.SetTrigger("Attack1"); break;
                case 1: animator.SetTrigger("Attack2"); break;
                default: break;
            }
        }
        public void HitEnemySoundAnimation()
        {
            int random = Random.Range(0, 2);
            switch (random)
            {
                case 0:
                    SoundManager.SharedInstance.PlaySfx(attackSound1);
                    break;
                case 1:
                    SoundManager.SharedInstance.PlaySfx(attackSound2);
                    break;
                default:
                    break;
            }
        }
        public void HitEnemy()
        {     
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

            foreach (Collider2D item in hitEnemies)
            {
                item.GetComponent<EnemyController>().TakeDamage();
            }   
        }
        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;

            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }

        public void RetryGame(bool t)
        {
            health = 3;
            animator.SetBool("isDie", false);
            isStartedGame = true;
        }
        //method invoke from unityevent button "exit button"
        public void BackToMenu()
        {
            health = 3;
            animator.SetBool("isDie", false);
            isStartedGame = false;
        }
    }
}
