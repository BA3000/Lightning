using System.Collections;
using UnityEngine;

namespace Projectile
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private Vector2 moveDirection;

        private void OnEnable()
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
