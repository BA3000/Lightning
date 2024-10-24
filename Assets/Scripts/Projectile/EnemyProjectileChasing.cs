using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class EnemyProjectileChasing : Projectile
    {
        private void Awake()
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        protected override void OnEnable()
        {
            StartCoroutine(nameof(MoveDirectionCoroutine));
            base.OnEnable();
        }

        IEnumerator MoveDirectionCoroutine()
        {
            yield return null;
            if (target.activeSelf)
            {
                moveDirection = (target.transform.position - transform.position).normalized;
            }
        }
    }
}
