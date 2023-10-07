using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    float paddingX;

    [SerializeField]
    float paddingY;

    [SerializeField]
    float moveSpeed = 2f;

    [SerializeField]
    float moveRotAngle = 25f;

    private void OnEnable()
    {

        StartCoroutine(nameof(RandomlyMovingCoroutine));

    }

    private IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);
        var targetPos = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
        while (gameObject.activeSelf)
        {
            // if has not arrived targetPosition
            if (Vector3.Distance(transform.position, targetPos) > Mathf.Epsilon)
            {
                // keep moving
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                // rotate on x axis while moving
                transform.rotation = Quaternion.AngleAxis((targetPos - transform.position).normalized.y * moveRotAngle, Vector3.right);
            }
            else
            {
                // set a new targetPosition
                targetPos = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }

            yield return null;
        }
    }
}
