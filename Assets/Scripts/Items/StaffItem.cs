using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class StaffItem : ItemHoldable
    {
        public int minDamage;
        public int maxDamage;
        public int minCost;
        public int maxCost;
        public float chargeTime;
        public float minTimeBetweenShots;
        public Transform projectilePos;
        public GameObject projectilePrefab;
        public AudioSource chargeFx;
        public Animator anim;

        Projectile attackProjectile;
        bool isCharging = false;
        bool inShoot = false;
        float chargeStartTime;
        PlayerController player;

        float[] times;
        short timesProcessed;

        bool pressedLastTime = false;

        private void Start()
        {
            player = GameController.Instance.gamePlayer;
            if (transform.parent.name == "ItemPos")
            {
                player.AddItem(this, true);
            }
        }

        public override MoveData UseItem(MoveData inputData)
        {
            if (inShoot) return inputData;

            if (isCharging)
            {
                float currentValue = Mathf.Clamp((Time.time - chargeStartTime) / chargeTime, 0f, 1f);
                chargeFx.pitch = currentValue * 0.5f + 0.7f;
                chargeFx.volume = currentValue;
            }

            if (inputData.axisPrimary > 0f && !isCharging)
            {
                if (player.stats.mp <= minCost)
                {
                    if (!pressedLastTime)
                        player.ShakeMp();
                    pressedLastTime = true;
                    return inputData;
                }

                isCharging = true;
                anim.SetBool("isCharging", true);

                chargeFx.pitch = 0.2f;
                chargeFx.volume = 0f;
                chargeFx.Play();

                attackProjectile = Instantiate(projectilePrefab, projectilePos.position, projectilePos.rotation, transform).GetComponent<Projectile>();
                attackProjectile.gameObject.GetComponent<Animator>().SetFloat("speed", 10f / chargeTime, 0f, 1f);
                Collider projectileCollider = attackProjectile.gameObject.GetComponent<Collider>();
                Physics.IgnoreCollision(player.charController, projectileCollider, true);
                projectileCollider.enabled = false;

                chargeStartTime = Time.time;
                player.stats.ChangeMp(-minCost);
                player.stats.mpGainLocked = true;
                float costTime = chargeTime / (maxCost - minCost);

                times = new float[(maxCost - minCost)];
                timesProcessed = 0;
                for (int i = 0; i < (maxCost - minCost); i++)
                {
                    times[i] = chargeStartTime + costTime * (i + 1);
                }
            }
            else if (inputData.axisPrimary <= 0f && isCharging)
            {
                Shoot();
            }
            else if (isCharging && timesProcessed < times.Length && Time.time > times[timesProcessed])
            {
                timesProcessed++;
                player.stats.ChangeMp(-1);
                if (player.stats.mp <= 0)
                {
                    Shoot();
                    player.ShakeMp();
                }
            }

            pressedLastTime = inputData.axisPrimary > 0f;
            return inputData;
        }

        IEnumerator ShootCooldown()
        {
            inShoot = true;
            yield return new WaitForSeconds(minTimeBetweenShots);
            inShoot = false;
        }

        private void Shoot()
        {
            chargeFx.Stop();
            isCharging = false;
            player.stats.mpGainLocked = false;
            anim.SetBool("isCharging", false);

            float chargedPercent = Mathf.Clamp((Time.time - chargeStartTime) / chargeTime, 0f, 1f);
            int damage = minDamage + Mathf.RoundToInt(chargedPercent * (maxDamage - minDamage));
            attackProjectile.gameObject.GetComponent<Animator>().SetTrigger("Shoot");
            attackProjectile.gameObject.GetComponent<Animator>().SetFloat("speed", chargedPercent);
            attackProjectile.gameObject.GetComponent<Collider>().enabled = true;

            attackProjectile.damage = damage;
            attackProjectile.speed = 8f;
            attackProjectile.lifetime = 10f;
            attackProjectile.startTime = Time.time;
            attackProjectile.transform.parent = null;
            attackProjectile.SpawnPrefab();

            Vector3 direction;

            RaycastHit hit;
            if (Physics.Raycast(player.eyeHeightTransform.position, player.eyeHeightTransform.forward, out hit, 100f))
            {
                direction = (hit.point - attackProjectile.transform.position).normalized;
            }
            else
            {
                direction = ((player.eyeHeightTransform.position + (100f * player.eyeHeightTransform.forward)) - attackProjectile.transform.position).normalized;
            }

            attackProjectile.direction = direction;

            StartCoroutine(ShootCooldown());
        }

        public override void OnEquip()
        {
            anim.enabled = true;
            anim.SetBool("Hidden", false);
        }

        public override void OnUnequip()
        {
            anim.SetBool("Hidden", true);
        }
    }
}
