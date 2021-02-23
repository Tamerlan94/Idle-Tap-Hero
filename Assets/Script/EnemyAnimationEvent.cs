using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunchHero
{
    public class EnemyAnimationEvent : MonoBehaviour
    {
        public void Die()
        {
            GetComponentInParent<EnemyController>().AnimationDead();
        }
        public void AttackHit()
        {
            GetComponentInParent<EnemyController>().AnimationHit();
        }
    }
}
