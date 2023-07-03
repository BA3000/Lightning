using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public GameObject Prefab => _prefab;

    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _size = 1;
    private Queue<GameObject> _queue;
    private Transform parent;

    public void Initialize(Transform parent)
    {
        _queue = new Queue<GameObject>();
        this.parent = parent;

        for (var i = 0; i < _size; i++)
        {
            _queue.Enqueue(Copy());
        }
    }

    GameObject Copy()
    {
        var copy = GameObject.Instantiate(_prefab, parent);
        copy.SetActive(false);
        return copy;
    }

    private GameObject AvailableObject()
    {
        GameObject availableObject = null;

        if (_queue.Count > 0 && !_queue.Peek().activeSelf)
        {
            availableObject = _queue.Dequeue();
        }
        else
        {
            availableObject = Copy();
        }

        _queue.Enqueue(availableObject);

        return availableObject;
    }

    public GameObject PrepareObject()
    {
        var prepareObject = AvailableObject();
        prepareObject.SetActive(false);

        return prepareObject;
    }

    public GameObject PrepareObject(Vector3 position)
    {
        var prepareObject = AvailableObject();
        prepareObject.SetActive(false);
        prepareObject.transform.position = position;

        return prepareObject;
    }

    public GameObject PrepareObject(Vector3 position, Quaternion rotation)
    {
        var prepareObject = AvailableObject();
        prepareObject.SetActive(false);
        prepareObject.transform.position = position;
        prepareObject.transform.rotation = rotation;

        return prepareObject;
    }

    public GameObject PrepareObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        var prepareObject = AvailableObject();
        prepareObject.SetActive(false);
        prepareObject.transform.position = position;
        prepareObject.transform.rotation = rotation;
        prepareObject.transform.localScale = localScale;

        return prepareObject;
    }
}