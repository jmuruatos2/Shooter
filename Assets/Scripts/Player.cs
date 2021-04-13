using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    private float _normalSpeed = 4.0f;  //es la velocidad inicial
    [SerializeField]
    private float speed; // es la velocidad actual del jugador    
    [SerializeField]
    private float _maxSpeed = 20.0f;
    private bool _isAccelerating;
    private SpriteRenderer _shieldSpriteRenderer;
    private int _ammo;
    [SerializeField]
    private int _shieldStrength;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _hommingMissilePrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private bool _tripleShot;
    [SerializeField]
    private bool _hommingMissileActive;
    [SerializeField]
    private GameObject _shieldObject;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject UIManager;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _FireLeft;
    [SerializeField]
    private GameObject _FireRight;
    [SerializeField]
    private AudioSource _laserAudio;
    [SerializeField]
    private AudioSource _powerUpAudio;

    
    


    private int _score;

    void Start()
    {
        _hommingMissileActive = false;
        _shieldSpriteRenderer = _shieldObject.GetComponent<SpriteRenderer>();
        _ammo = 15;
        

        if (_shieldSpriteRenderer == null)
        {
            Debug.LogError("Shield SpriteRenderer Null");
        }
        speed = _normalSpeed;
        _isAccelerating = false;
        transform.position = new Vector3(0, 0,0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _tripleShot = false;
        _uiManager = UIManager.GetComponent<UIManager>();
        _uiManager.UpdateAmmo(_ammo);

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager null");
        }
    }


    void Update()
    {
        CalculateMovement();
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Fire();
        }



    }

    void CalculateMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isAccelerating)
        {
            StartCoroutine(Accelerate());
        }
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
        transform.Translate(Vector2.up * verticalInput * speed * Time.deltaTime);

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }

        if (transform.position.y > 2)
        {
            transform.position = new Vector3(transform.position.x, 2, 0);
        }
        else if (transform.position.y < -3.57)
        {
            transform.position = new Vector3(transform.position.x, -3.57f, 0);
        }


    }
    void Fire() 
    {        
            _canFire = Time.time + _fireRate;
        if (_ammo > 0)
        {
            if (_hommingMissileActive)
            {
                Instantiate(_hommingMissilePrefab, new Vector3(transform.position.x, transform.position.y + 2.0f), Quaternion.identity);
            }
            else if (_tripleShot)
            {
                Instantiate(_tripleShotPrefab, new Vector3(transform.position.x - 0.325177f, transform.position.y + 2.24f), Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.5f), Quaternion.identity);
            }

            _ammo--;
            _laserAudio.Play();

            _uiManager.UpdateAmmo(_ammo);

        }
    }

    public void PlayerDamage()
    {
        if (_shieldStrength > 0)
        {
            _shieldStrength--;



            switch (_shieldStrength)
            {
                case 0:
                    _shieldObject.SetActive(false);
                    break;
                case 1:
                    _shieldSpriteRenderer.color = Color.red;
                    break;
                case 2:
                    _shieldSpriteRenderer.color = Color.yellow;
                    break;
            }

            return;
        }
        _lives--;
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _FireLeft.SetActive(true);
        } else if (_lives == 1)
        {
            _FireRight.SetActive(true);
        }


        if (_lives == 0)
        {
            _spawnManager.StopPlaying();
            _uiManager.ShowGameOverText();
            Destroy(this.gameObject);
        }
    }

    public void ActivateTripleShot(int seconds)
    {
        _tripleShot = true;
        _hommingMissileActive = false;
        _powerUpAudio.Play();
        StartCoroutine(TripleShotPowerDownRoutine(seconds));
        

    }

    private IEnumerator TripleShotPowerDownRoutine(int seconds)
    {
        yield return new WaitForSeconds(5);
        _tripleShot = false;
    }

    public void ActivateMissile()
    {
        _tripleShot = false;
        _hommingMissileActive = true;
        StartCoroutine(HommingMissilePowerDown(5));
    }

    private IEnumerator HommingMissilePowerDown(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        _hommingMissileActive = false;
    }

    public void ActivateSpeedPowerup (int seconds)
    {
        _powerUpAudio.Play();
        speed = 8;
        StartCoroutine(SpeedPowerDownRoutine(seconds));
    }

    private IEnumerator SpeedPowerDownRoutine(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        speed = 4;
    }

    public void ActivateShield()
    {
        _powerUpAudio.Play();
        _shieldObject.SetActive(true);
        _shieldStrength = 3;
        _shieldSpriteRenderer.color = Color.white;

    }

    public void RechargeAmmo(int ammo)
    {
        _ammo += ammo;
        _uiManager.UpdateAmmo(_ammo);
    }

    public void addScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void RecoverLife()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Laser"))
        {
            PlayerDamage();
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator Accelerate()
    {
        Debug.Log("Acelerando");
        _isAccelerating = true;
        
        while (Input.GetKey(KeyCode.LeftShift))
        {
            if (speed <= _maxSpeed)
            {
                speed++;
            }
            yield return new WaitForSeconds(1);
        }
        _isAccelerating = false;
        speed = _normalSpeed;
    }
}
