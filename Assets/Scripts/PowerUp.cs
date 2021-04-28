using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private int _powerupID;  // 0 -> TripleShot  1--> Speed 2--> Shield
    // Start is called before the first frame update
    private Laser _laser;
    private Vector3 _vector;
    private float _x;
    private GameObject _player;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");  
        if (_player == null)
        {
            Debug.LogError("Player Null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.C))
        {
            if (_player != null)
            {
                if (_player.transform.position.x > transform.position.x)
                {
                    _x = 1;
                } else if (_player.transform.position.x < transform.position.x)
                {
                    _x = -1;
                }
            }
        } else
        {
            _x = 0;
        }

        _vector = new Vector3(_x, -1, 0);

        transform.Translate(_vector * Time.deltaTime * _speed);


        if (transform.position.y < -6.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            switch(_powerupID)
            {
                case 0:
                    player.ActivateTripleShot(5);
                    break;
                case 1:
                    player.ActivateSpeedPowerup(5);
                    break;
                case 2:
                    player.ActivateShield();
                    break;
                case 3:
                    player.RechargeAmmo(15);
                    break;
                case 4:
                    player.RecoverLife();
                    break;
                case 5:
                    player.ActivateMissile();
                    break;
                case 6:
                    player.PlayerDamage();
                    break;
            }

            Destroy(this.gameObject);
        }
        if(other.CompareTag("Laser"))
        {
            _laser = other.gameObject.GetComponent<Laser>();
            _laser.DestroyLaser();
            Destroy(this.gameObject);
        }
    }
}
