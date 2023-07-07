using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletSpawnPoint;

    void FixedUpdate()
    {
        transform.LookAt(_bulletSpawnPoint.transform);
    }
}
