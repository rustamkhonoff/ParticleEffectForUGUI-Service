using System;
using UnityEngine;

namespace UIParticle.Service.Extras
{
    public class FollowPosition : MonoBehaviour
    {
        private Func<Vector3?> m_position;
        private float m_speed;
        private Transform m_transform;

        public static FollowPosition Create(Transform transform, Func<Vector3?> position, float speed = 10f)
        {
            FollowPosition instance = transform.gameObject.AddComponent<FollowPosition>();
            instance.Initialize(position, speed);
            instance.m_transform = instance.transform;
            return instance;
        }

        private void Initialize(Func<Vector3?> position, float speed)
        {
            m_speed = speed;
            m_position = position;
        }

        private void Update()
        {
            if (m_position is null)
            {
                Destroy(this);
                return;
            }

            Vector3? vector = m_position();
            if (vector == null)
            {
                Destroy(this);
                return;
            }

            m_transform.position = Vector3.Lerp(transform.position, vector.Value, Time.fixedDeltaTime * m_speed);
        }
    }
}