using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    { 
        InvokeRepeating(nameof(Spawn), 0, 1f); 
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Spawn()
    {
        ObjectsPool.PoolInstance.SpawnGameObject("Cube", transform.position, Quaternion.identity);
        
    }
}
