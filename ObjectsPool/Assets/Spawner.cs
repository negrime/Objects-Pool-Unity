using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    void Start()
    { 
        InvokeRepeating(nameof(Spawn), 0, 1f); 
    }

    void Update()
    {
    }

    public void Spawn()
    {
        ObjectsPool.PoolInstance.SpawnGameObject("Cube", transform.position, Quaternion.identity);
        
    }
}
