using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("--- MOVE ---")]
    /// <summary>
    /// 用来调整生成位置，X轴
    /// </summary>
    [SerializeField]
    private float paddingX;

    /// <summary>
    /// 用来调整生成位置，Y 轴
    /// </summary>
    [SerializeField]
    private float paddingY;

    /// <summary>
    /// 移动速度
    /// </summary>
    [SerializeField]
    private float moveSpeed = 2f;

    /// <summary>
    /// 移动时的旋转角度
    /// </summary>
    [SerializeField]
    private float moveRotAngle = 25f;

    [Header("--- FIRE ---")]
    [SerializeField]
    private GameObject[] projectiles;

    /// <summary>
    /// 调整开火位置
    /// </summary>
    [SerializeField]
    private Transform muzzle;

    /// <summary>
    /// 随机开火间隔，最小值
    /// </summary>
    [SerializeField]
    private float minFireInterval;

    /// <summary>
    /// 随机开火间隔，最大值
    /// </summary>
    [SerializeField]
    private float maxFireInterval;

    private void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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
                // rotate on x axis while moving
                transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime),
                    Quaternion.AngleAxis((targetPos - transform.position).normalized.y * moveRotAngle, Vector3.right));
            }
            else
            {
                // set a new targetPosition
                targetPos = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }

            yield return null;
        }
    }

    private IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            foreach(var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
        }
    }
}
