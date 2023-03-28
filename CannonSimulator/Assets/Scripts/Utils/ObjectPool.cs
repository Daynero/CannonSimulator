using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly Queue<GameObject> _pool;
    private readonly Func<GameObject> _createFunction;
    private readonly Action<GameObject> _resetFunction;
    private readonly int _maxObjects;
    private readonly GameObject _prefab;

    public ObjectPool(GameObject prefab, Action<GameObject> resetFunction = null, int maxObjects = int.MaxValue)
    {
        _prefab = prefab;
        _createFunction = CreateGameObject;
        _resetFunction = resetFunction;
        _pool = new Queue<GameObject>();
        _maxObjects = maxObjects;
    }

    private GameObject CreateGameObject()
    {
        var obj = GameObject.Instantiate(_prefab);
        obj.SetActive(false);
        return obj;
    }

    public GameObject GetObject()
    {
        if (_pool.Count > 0)
        {
            return _pool.Dequeue();
        }

        if (_pool.Count < _maxObjects)
        {
            return _createFunction();
        }

        return null;
    }

    public void ReturnObject(GameObject obj)
    {
        if (_pool.Count < _maxObjects)
        {
            _resetFunction?.Invoke(obj);
            _pool.Enqueue(obj);
        }
        else
        {
            GameObject.Destroy(obj);
        }
    }
}