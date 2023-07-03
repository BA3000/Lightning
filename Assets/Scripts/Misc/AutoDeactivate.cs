using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [SerializeField] private bool destroyGameObject;
    [SerializeField] private float lifetime = 3f;
    private WaitForSeconds _waitLifetime;
    private void Awake()
    {
        _waitLifetime = new WaitForSeconds(lifetime);
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());
    }

    private IEnumerator DeactivateCoroutine()
    {
        yield return _waitLifetime;

        if (destroyGameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
