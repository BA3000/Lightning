using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Pool[] playerProjectilePools;

    private static Dictionary<GameObject, Pool> _dictionary;

    private void Start()
    {
        _dictionary = new Dictionary<GameObject, Pool>();
        Initialize(playerProjectilePools);
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(playerProjectilePools);
    }
#endif

    private void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning($"Pool: {pool.Prefab.name} has a runtime size {pool.RuntimeSize} bigger than its initial size {pool.Size}!");
            }
        }
    }

    public void Initialize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR
            if (_dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Prefab appears in multiple pools! Prefab: " + pool.Prefab);
                continue;
            }
#endif
            _dictionary.Add(pool.Prefab, pool);

            var poolParent = new GameObject(pool.Prefab.name + "Pool").transform;

            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    /// <summary>
    /// <para>return a specific <paramref name="prefab"></paramref> gameObject in the pool</para>
    /// <para> 根据传入的<paramref name="prefab"/>参数，返回对象池中预备好的对象 </para>
    /// </summary>
    /// <param name="prefab">
    /// <para> specified gameObject in the pool</para>
    /// <para>指定的游戏对象预制体</para>
    /// </param>
    /// <returns>
    /// <para>Prepared gameObject in the pool</para>
    /// <para>对象池中预备好的 gameObject</para>>
    /// </returns>
    public static GameObject Release(GameObject prefab)
    {
        if (_dictionary.ContainsKey(prefab)) return _dictionary[prefab].PrepareObject();
#if UNITY_EDITOR
        Debug.LogError("Pool Manager Could Not Find Prefab Pool Consist of: " + prefab.name);
        return null;
#endif
    }

    /// <summary>
    /// <para>return a specific <paramref name="prefab"></paramref> gameObject in the pool</para>
    /// <para> 根据传入的<paramref name="prefab"/>参数，返回对象池中预备好的对象 </para>
    /// </summary>
    /// <param name="prefab">
    /// <para> specified gameObject in the pool</para>
    /// <para>指定的游戏对象预制体</para>
    /// </param>
    /// <param name="position">
    /// <para>Specify gameObject position</para>
    /// <para>指定 gameObject 位置</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position)
    {
        if (_dictionary.ContainsKey(prefab)) return _dictionary[prefab].PrepareObject(position);
#if UNITY_EDITOR
        Debug.LogError("Pool Manager Could Not Find Prefab Pool Consist of: " + prefab.name);
        return null;
#endif
    }

    /// <summary>
    /// <para>return a specific <paramref name="prefab"></paramref> gameObject in the pool</para>
    /// <para> 根据传入的<paramref name="prefab"/>参数，返回对象池中预备好的对象 </para>
    /// </summary>
    /// <param name="prefab">
    /// <para> specified gameObject in the pool</para>
    /// <para>指定的游戏对象预制体</para>
    /// </param>
    /// <param name="position">
    /// <para>Specify gameObject position</para>
    /// <para>指定 gameObject 位置</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specify rotation</para>
    /// <para>指定旋转值</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (_dictionary.ContainsKey(prefab)) return _dictionary[prefab].PrepareObject(position, rotation);
#if UNITY_EDITOR
        Debug.LogError("Pool Manager Could Not Find Prefab Pool Consist of: " + prefab.name);
        return null;
#endif
    }

    /// <summary>
    /// <para>return a specific <paramref name="prefab"></paramref> gameObject in the pool</para>
    /// <para> 根据传入的<paramref name="prefab"/>参数，返回对象池中预备好的对象 </para>
    /// </summary>
    /// <param name="prefab">
    /// <para> specified gameObject in the pool</para>
    /// <para>指定的游戏对象预制体</para>
    /// </param>
    /// <param name="position">
    /// <para>Specify gameObject position</para>
    /// <para>指定 gameObject 位置</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specify rotation</para>
    /// <para>指定旋转值</para>
    /// </param>
    /// <param name="localScale">
    /// <para>Specify Scale</para>
    /// <para>指定缩放值</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        if (_dictionary.ContainsKey(prefab)) return _dictionary[prefab].PrepareObject(position, rotation, localScale);
#if UNITY_EDITOR
        Debug.LogError("Pool Manager Could Not Find Prefab Pool Consist of: " + prefab.name);
        return null;
#endif
    }
}