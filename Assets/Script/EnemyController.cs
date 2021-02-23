using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace PunchHero
{
    public class EnemyController : MonoBehaviour, IStartEndGame, IRetryGame
    {
        public static event UnityAction OnEnemyDie;

        public int health;
        private int currentHealth;
        public float movementSpeed;
        public float attackRange;

        public AudioClip hittedSound;
        public AudioClip attackSound;

        private Rigidbody2D rb2D;
        private Transform playerTransform;
        private Vector2 targetMove;
        private Animator animator;
        private bool isStartedGame;
        private bool isSecondHIt = false;

        private void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            GameUI.OnStartEndGame += StartEndGame;
            GameUI.OnRetryGame += RetryGame;
            isStartedGame = true;
            //временно
            animator.SetBool("isWalk", true);
        }
        private void FixedUpdate()
        {
            if (isStartedGame)
            {
                if (isSecondHIt) return;

                if (Vector2.Distance(transform.position, playerTransform.position) > attackRange)
                {
                    Move();
                }
                else
                    Attack();
            }          
        }      
        private void OnEnable()
        {
            if (rb2D != null)
            {
                animator.SetBool("isWalk", true);
                GetComponent<CapsuleCollider2D>().enabled = true;
            }
            //health = Random.Range(1, 4);
            currentHealth = health;
            movementSpeed = Random.Range(1, 8);
        }

        private void Move()
        {
            rb2D.MovePosition(rb2D.position + (targetMove * Time.fixedDeltaTime * movementSpeed));
        }
        public void SetTargetMove(float m)
        {
            targetMove = new Vector2(0f + m, 0f);
        }
        public void TakeDamage()
        {
            currentHealth -= 1;

            animator.SetTrigger("GetHit");

            SoundManager.SharedInstance.PlaySfx(hittedSound);
            if (currentHealth >= 1)
            {
                isSecondHIt = true;
                animator.SetBool("isWalk", true);
                StartCoroutine(Knockback());
            }

            if (currentHealth <= 0)
            {              
                animator.SetBool("isWalk", false);
                animator.SetTrigger("isDie");
                GetComponent<CapsuleCollider2D>().enabled = false;
                OnEnemyDie?.Invoke();                
            }
        }
        IEnumerator Knockback()
        {
            rb2D.AddForce(-targetMove * 4f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1);
            isSecondHIt = false;
        }
        private void Attack()
        {
            animator.SetBool("isWalk", false);
            animator.SetTrigger("Attack");
        }
        public void StartEndGame(bool t)
        {
            isStartedGame = t;
            if (!t)
            {
                animator.SetBool("isWalk", false);
            }
        }

        public void AnimationDead()
        {
            gameObject.SetActive(false);
        }
        public void AnimationHit()
        {
            playerTransform.GetComponent<PlayerController>().TakeDamage();
            SoundManager.SharedInstance.PlaySfx(attackSound);
        }

        public void RetryGame()
        {
            isStartedGame = true;
        }
    }
}
