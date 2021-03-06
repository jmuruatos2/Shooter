using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    private int _normalSpeed = 4;  //es la velocidad inicial
    [SerializeField]
    private int speed; // es la velocidad actual del jugador    
    [SerializeField]
    private int _maxSpeed = 15; //velocidad maxima normal (left shift)
    private int _maxSpeedPowerup = 20; //velocidad maxima con powerup
    private bool _isAccelerating;
    private bool _isDeaccelerating;
    private bool _speedPowerUpActive;
    [SerializeField]
    private int _speedPowerupsStacked; 

    private SpriteRenderer _shieldSpriteRenderer;
    private int _ammo;
    private int _maxAmmo = 30;
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
    [SerializeField]
    private GameObject _camera;

    private bool _isCameraShaking;

    
    


    private int _score;

    void Start()
    {
        _hommingMissileActive = false;
        _shieldSpriteRenderer = _shieldObject.GetComponent<SpriteRenderer>();
        _ammo = 15;
        _speedPowerupsStacked = 0;
        _isCameraShaking = false;
        

        if (_shieldSpriteRenderer == null)
        {
            Debug.LogError("Shield SpriteRenderer Null");
        }
        speed = _normalSpeed;
        _isAccelerating = false;
        _isDeaccelerating = false;
        _speedPowerUpActive = false;
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

        if (_isCameraShaking)
        {
            _camera.transform.localPosition = _camera.transform.localPosition + Random.insideUnitSphere * 0.10f;
        }


    }

    void CalculateMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isAccelerating && !_speedPowerUpActive)
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
        if (!_isCameraShaking)
        {
            StartCoroutine(CameraShake());
        }
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

    public IEnumerator CameraShake()
    {
        _isCameraShaking = true;
        yield return new WaitForSeconds(0.5f);
        _isCameraShaking = false;
        _camera.transform.position = new Vector3(0, 1, -10);

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
        if (_ammo > _maxAmmo)
        {
            _ammo = _maxAmmo;
        }
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
        _isAccelerating = true;
        _isDeaccelerating = false;
        
        while (Input.GetKey(KeyCode.LeftShift) && !_speedPowerUpActive)
        {
            if (speed <= _maxSpeed)
            {
                speed++;
                _uiManager.UpdateThruster(speed);
            }
            yield return new WaitForSeconds(1);
        }
        _isAccelerating = false;
        if (!_speedPowerUpActive)
        {
            StartCoroutine("Deaccelerate");
        }
        _uiManager.UpdateThruster(speed);
    }


    public void ActivateSpeedPowerup(int seconds)
    {
        _powerUpAudio.Play();
        speed = _maxSpeedPowerup;
        _uiManager.UpdateThruster(speed);
        _speedPowerUpActive = true;
        _isDeaccelerating = false;

        if (_speedPowerupsStacked < 1)
        {
            _speedPowerupsStacked++;
            StartCoroutine(SpeedPowerDownRoutine(seconds));
        } else
        {
            _speedPowerupsStacked++;
        }
        

    }

    
    private IEnumerator SpeedPowerDownRoutine(int seconds)
    {
        while (_speedPowerupsStacked > 0)
        {   
            yield return new WaitForSeconds(seconds);
            _speedPowerupsStacked--;
        }

        _speedPowerUpActive = false;
        StartCoroutine("Deaccelerate");

    }

    private IEnumerator Deaccelerate()
    {
        _isDeaccelerating = true;
        while (speed > _normalSpeed && _isDeaccelerating)
        {
            speed--;
            _uiManager.UpdateThruster(speed);
            yield return new WaitForSeconds(1); //CAMBIAR POR VARIABLE
        }
        _isDeaccelerating = false;
    }
}
