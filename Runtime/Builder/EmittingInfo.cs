using System;
using UnityEngine;

namespace UIParticle.Service
{
    [Serializable]
    public class EmittingInfo
    {
        [field: SerializeField] public float Delay { get; internal set; }
        [field: SerializeField] public int Amount { get; internal set; }

        public EmittingInfo(float delay,int amount)
        {
            Delay = delay;
            Amount = amount;
        }
    }
}