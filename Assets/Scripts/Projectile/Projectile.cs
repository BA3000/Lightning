using System.Collections;
using UnityEngine;

namespace Projectile
{
    public class Projectile : MonoBehaviour
    {

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
    }
}
