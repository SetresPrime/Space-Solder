using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireButton : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private GameObject _muzzleFlash;
    [SerializeField]
    private GameObject _crosshair;
    [SerializeField]
    private Transform _bulletSpawnPoint;

    [Header("Settings")]
    [SerializeField]
    private float _inSecRound;
    [SerializeField]
    private float _bulletSpeed;


    private Vector3 _crosshairBaseSize;
    private float _crosshairBaseDistance;
    private bool _isFire;
    private float _fireKD;
    private void Start()
    {
        _crosshairBaseDistance = _crosshair.transform.localPosition.y;
        _crosshairBaseSize = _crosshair.transform.localScale;
    }
    private void FixedUpdate()
    {
        CrosshairPos();
        CrosshairSize();
        _muzzleFlash.SetActive(false);
        /* 
        Я намагався зробить це через #if, але при будь-якому випадку:
        або працює скрізь або, не працює зовсім (як на телефоні через Unity Remote, так і на ПК) 

        if (!_isFire)
            return;  
        */
        if (Input.GetMouseButton(0) && Time.time > _fireKD)
        {
            _fireKD = Time.time + 1f / _inSecRound;
            Fire();
        } 
    }
    public void ButtonDown()
    {
        _isFire = true;
    }
    public void ButtonUp()
    {
        _isFire = false;
    }
    void CrosshairSize()
    {
        float crosshairSizeMult = 1f / _crosshair.transform.localPosition.y;
        _crosshair.transform.localScale = new Vector3(_crosshairBaseSize.x - crosshairSizeMult,
                                                      _crosshairBaseSize.y - crosshairSizeMult,
                                                      _crosshairBaseSize.z - crosshairSizeMult);
    }
    void CrosshairPos()
    {
        RaycastHit hit;
        Physics.Raycast(_bulletSpawnPoint.position, _bulletSpawnPoint.up, out hit);

        if (hit.collider && hit.collider.tag != "Bullet")
            _crosshair.transform.position = hit.point;

        else if (!hit.collider)
            _crosshair.transform.localPosition = new Vector3(0, _crosshairBaseDistance, 0);
    }
    void Fire()
    {
        _muzzleFlash.SetActive(true);
        GameObject bullet = Instantiate(_bullet, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(_bulletSpawnPoint.up * _bulletSpeed);
    }
}
