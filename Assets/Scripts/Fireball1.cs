using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball1 : MonoBehaviour
{
    private float _speed = 4;
    float x;
    float y;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
        x = transform.position.x;
        y = transform.position.y;

        if (transform.position.x > 11.0f || transform.position.x < -11.0f || transform.position.y < -7.0f || transform.position.y > 9.0f )
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player _player = other.GetComponent<Player>();
            _player.PlayerDamage();
            Destroy(this.gameObject);
        }
    }
}
