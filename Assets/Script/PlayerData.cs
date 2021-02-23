using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunchHero
{
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject
    {
        public int id;
        public RuntimeAnimatorController animator;
        public Sprite icon;
        public int cost;
    }
}
