using System.Collections;
using UnityEngine;

namespace Projectile
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] GameObject hitVFX;
        [SerializeField] float damage;
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] protected Vector2 moveDirection;

        protected GameObject target;

        protected virtual void OnEnable()
        {
            StartCoroutine(MoveDirectly());
        }

        private IEnumerator MoveDirectly()
        {
            while (gameObject.activeSelf)
            {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

                yield return null;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Character>(out var character))
            {
                character.TakeDamage(damage);
                PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
                gameObject.SetActive(false);
            }
        }
    }
}
