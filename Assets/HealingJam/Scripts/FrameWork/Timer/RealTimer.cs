using System;
using UnityEngine;

namespace HealingJam
{
    public class RealTimer
    {
        public readonly float startTime;
    
        public RealTimer()
        {
            startTime = Time.realtimeSinceStartup;

            //startTime = DateTime.UtcNow;
        }

        public float GetElapsedTime()
        {
            return Time.realtimeSinceStartup - startTime;
            //return (DateTime.UtcNow - startTime).TotalSeconds;
        }
    }
}
