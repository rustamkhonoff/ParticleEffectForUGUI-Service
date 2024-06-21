using System;
using UnityEngine;

namespace UGUIParticleEffect
{
    public class FollowPosition : MonoBehaviour
    {
        private Func<Vector3> m_position;
        private float m_speed;

        public static FollowPosition Create(Transform transform, Func<Vector3> position, float speed = 10f)
        {
            FollowPosition instance = transform.gameObject.AddComponent<FollowPosition>();
            instance.Initialize(position, speed);
            return instance;
        }

        private void Initialize(Func<Vector3> position, float speed)
        {
            m_speed = speed;
            m_position = position;
        }

        private void FixedUpdate()
        {
            if (m_position is null)
                return;
            transform.position = Vector3.Lerp(transform.position, m_position.Invoke(), Time.fixedDeltaTime * m_speed);
        }
    }
}