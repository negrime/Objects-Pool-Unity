using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool : MonoBehaviour
{

    public List<ObjectPool> Pools = new List<ObjectPool>();
    private Dictionary<string, Queue<GameObject>> _poolDictionary = new Dictionary<string, Queue<GameObject>>();

    public static ObjectsPool PoolInstance;
    [System.Serializable]
    public class ObjectPool
    {
        public string Tag;
        public GameObject GameObject;
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

    void Update()
    {
        
    }

    private void Init()
    {
        foreach (var pool in Pools)
        {
            Queue<GameObject> queueObjects = new Queue<GameObject>();
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
        else
        {
            if (_poolDictionary[tag].Count < 2)
            {
                GameObject addGameObject = Instantiate(_poolDictionary[tag].Peek(), transform.position, Quaternion.identity);
                addGameObject.SetActive(false);
                _poolDictionary[tag].Enqueue(addGameObject);
                
            }
            
            GameObject gameObject = _poolDictionary[tag].Dequeue();
            gameObject.transform.position = spawnPosition;
            gameObject.transform.rotation = quaternion;
            gameObject.SetActive(true);
        }

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

