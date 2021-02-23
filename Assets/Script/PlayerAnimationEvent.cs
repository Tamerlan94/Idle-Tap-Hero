using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunchHero
{
    public class PlayerAnimationEvent : MonoBehaviour
    {
        public void ReHit()
        {
            GetComponentInParent<PlayerController>().HitEnemy();
        }
        public void SoundHit()
        {
            GetComponentInParent<PlayerController>().HitEnemySoundAnimation();
        }
    }
}
