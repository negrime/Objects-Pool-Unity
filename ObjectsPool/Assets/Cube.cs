using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnEnable()
    {
        Invoke(nameof(DestroyPoolObject), 1f);
    }

    
    public void DestroyPoolObject()
    {
        ObjectsPool.PoolInstance.DestroyPoolObject("Cube", this.gameObject);
    }
}
