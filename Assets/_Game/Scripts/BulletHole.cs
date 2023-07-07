using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime;

    private float _timer;
    private void Start()
    {
        _timer = _lifeTime;
    }
    void FixedUpdate()
    {
        _timer -= Time.deltaTime;
        if (_timer <= _lifeTime / 2 && _timer > 0)
        {
            float a = _timer * ( 2 / _lifeTime);
            
            gameObject.GetComponent<SpriteRenderer>().color = new Color (1, 1, 1, a);
        }
        else if (_timer <= 0)
            Destroy(gameObject);
    }
}
