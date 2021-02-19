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

        public GameObject spawnPrefab;
        public GameObject killPrefab;

        private void Start()
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindWithTag("Player").GetComponent<CharacterController>());
        }

        public void SpawnPrefab()
        {
            if (spawnPrefab != null)
                Instantiate(spawnPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        }

        void Update()
        {
            transform.position += direction * speed * Time.deltaTime;

            if (startTime > 0f && Time.time > startTime + lifetime)
                DestroyProjectile();

        }

        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
                other.gameObject.GetComponent<EntityStats>().ChangeHp(-damage);

            DestroyProjectile();
        }

        private void DestroyProjectile()
        {
            if (killPrefab != null)
                Instantiate(killPrefab, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }
}
