using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class StaffItem : ItemHoldable
    {
        public int minDamage;
        public int maxDamage;
        public float chargeTime;
        public Transform projectilePos;
        public GameObject projectilePrefab;

        Projectile attackProjectile;
        bool isCharging = false;
        float chargeStartTime;
        PlayerController player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.items.Add(this);
            player.currentItem = this;
        }

        public override MoveData UseItem(MoveData inputData)
        {
            if (inputData.axisPrimary > 0f && !isCharging)
            {
                isCharging = true;
                chargeStartTime = Time.time;
                attackProjectile = Instantiate(projectilePrefab, projectilePos.position, projectilePos.rotation, transform).GetComponent<Projectile>();
                attackProjectile.gameObject.GetComponent<Animator>().SetFloat("speed", 10f / chargeTime, 0f, 1f);
            }
            else if (inputData.axisPrimary <= 0f && isCharging)
            {
                isCharging = false;
                Shoot();
            }

            return inputData;
        }

        private void Shoot()
        {
            float chargedPercent = Mathf.Clamp((Time.time - chargeStartTime) / chargeTime, 0f, 1f);
            int damage = minDamage + Mathf.RoundToInt(chargedPercent * (maxDamage - minDamage));
            attackProjectile.gameObject.GetComponent<Animator>().SetTrigger("Shoot");
            attackProjectile.gameObject.GetComponent<Animator>().SetFloat("speed", chargedPercent);

            attackProjectile.damage = damage;
            attackProjectile.speed = 8f;
            attackProjectile.lifetime = 10f;
            attackProjectile.startTime = Time.time;
            attackProjectile.transform.parent = null;

            Vector3 direction;

            RaycastHit hit;
            if (Physics.Raycast(player.camTransform.position, player.camTransform.forward, out hit, 100f))
            {
                direction = (hit.point - attackProjectile.transform.position).normalized;
            }
            else
            {
                direction = ((player.camTransform.position + (100f * player.camTransform.forward)) - attackProjectile.transform.position).normalized;
            }

            attackProjectile.direction = direction;
        }
    }
}
