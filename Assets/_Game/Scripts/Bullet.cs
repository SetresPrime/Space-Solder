using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    
    [SerializeField]
    private GameObject _bulletImpact;
    [SerializeField]
    private GameObject _bulletHole;
    [SerializeField]
    private Sprite[] _bulletHoleSprites; 

    private float _timer;
    private void FixedUpdate()
    {
        _timer += Time.deltaTime;
        if (_timer > 20)
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject bulletHole = Instantiate(_bulletHole, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));

        bulletHole.GetComponent<SpriteRenderer>().sprite = _bulletHoleSprites[Random.Range(0, _bulletHoleSprites.Length)];

        Destroy(gameObject);
    }
}
