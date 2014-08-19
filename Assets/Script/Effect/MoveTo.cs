using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class MoveTo : MonoBehaviour 
    {
        public Vector3 To;
        public float From;
        public float Duration;
        private Vector3 _startPoint;

        private float lastTime;

        void Start()
        {
            _startPoint = transform.localPosition;
        }

        void Update() 
        {
            var relativeTime = Time.timeSinceLevelLoad - From;
            float time;
            if (Duration > Time.deltaTime)
                time = relativeTime / Duration;
            else time = relativeTime > 0 ? 1 : 0;
            if (time > 0 && time <= 1)
            {
                var step = time - lastTime;
                if ((step + time) > 1)
                    time = 1;
                transform.localPosition = Vector3.Lerp(_startPoint, To, time);
                lastTime = time;
            }
        }
    }
}