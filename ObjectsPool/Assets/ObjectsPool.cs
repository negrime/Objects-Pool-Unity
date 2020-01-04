using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectsPool : MonoBehaviour
{

    public List<ObjectPool> Pools = new List<ObjectPool>();
    private Dictionary<string, Queue<GameObject>> _poolDictionary = new Dictionary<string, Queue<GameObject>>();

    public static ObjectsPool PoolInstance;
    [System.Serializable]
    public class ObjectPool
    {
        public GameObject GameObject;
        public string Tag;
        public int Size;
    }

    private void Awake()
    {
        #region Singleton
        if (PoolInstance == null)
        {
            PoolInstance = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion
    }

    void Start()
    {
        Init();
    }


    private void Init()
    {
        foreach (var pool in Pools)
        {
            Queue<GameObject> queueObjects = new Queue<GameObject>();
            if (pool.Tag == "")
            {
                Debug.LogWarning("PoolObject doesn't have Tag. Now tag equals: " + pool.GameObject.name);
                pool.Tag = pool.GameObject.name;
            }
            for (int i = 0; i < pool.Size; i++)
            { 
                GameObject gameObject = Instantiate(pool.GameObject, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                queueObjects.Enqueue(gameObject);
            }
            _poolDictionary.Add(pool.Tag, queueObjects);
        }
    }

    public GameObject SpawnGameObject(string tag, Vector3 spawnPosition, Quaternion quaternion)
    {
        if (_poolDictionary.Count < 1)
        {
            Debug.LogError("ObjectsPool don't have any objects!");
            return null;
        }

        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("ObjectsPool don't have objects with a tag: " + tag + "!");
            return null;
        }

        if (_poolDictionary[tag].Count == 1)
        {
            GameObject addGameObject = Instantiate(_poolDictionary[tag].Peek(), transform.position, Quaternion.identity);
            addGameObject.SetActive(false);
            ObjectPool pool = Pools.Single(s => s.Tag == tag);
            pool.Size++;
            _poolDictionary[tag].Enqueue(addGameObject);
                
        }
            
        GameObject gameObject = _poolDictionary[tag].Dequeue();
        gameObject.transform.position = spawnPosition;
        gameObject.transform.rotation = quaternion;
        gameObject.SetActive(true);
        return null;
    }

    public void DestroyPoolObject(string tag, GameObject gameObject)
    {
        if (_poolDictionary.ContainsKey(tag))
        {
            gameObject.transform.position = new Vector3(0,0,0);
            gameObject.SetActive(false);
            _poolDictionary[tag].Enqueue(gameObject);
        }
        else
        {
            Debug.LogWarning("ObjectsPool don't have objects with a tag: " + tag + "!");
        }
    }
}

