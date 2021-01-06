using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{

    public class Projectile : MonoBehaviour
    {
        public float speed;
        public Vector3 direction;
        public int damage;
        public float lifetime;
        public float startTime = -1f;

        private void Start()
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindWithTag("Player").GetComponent<CharacterController>());
        }

        void Update()
        {
            transform.position += direction * speed * Time.deltaTime;

            if (startTime > 0f && Time.time > startTime + lifetime)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<EntityStats>().ChangeHp(-damage);
            }

            Destroy(gameObject);
        }
    }
}
