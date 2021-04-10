using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    public float speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;
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
    private bool _shieldActive;
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
        transform.position = new Vector3(0, 0,0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _tripleShot = false;
        _shieldActive = false;
        _uiManager = UIManager.GetComponent<UIManager>();

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

        if (_tripleShot)
        {
            Instantiate(_tripleShotPrefab, new Vector3(transform.position.x - 0.325177f, transform.position.y + 2.24f), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.5f), Quaternion.identity);
        }

        _laserAudio.Play();

    }

    public void PlayerDamage()
    {
        if (_shieldActive)
        {
            _shieldActive = false;
            _shieldObject.SetActive(false);
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
        _powerUpAudio.Play();
        StartCoroutine(TripleShotPowerDownRoutine(seconds));
        

    }

    private IEnumerator TripleShotPowerDownRoutine(int seconds)
    {
        yield return new WaitForSeconds(5);
        _tripleShot = false;
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

    public void ActivateShield(int seconds)
    {
        _powerUpAudio.Play();
        _shieldActive = true;
        _shieldObject.SetActive(true);


    }

    public void addScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Laser"))
        {
            PlayerDamage();
            Destroy(collision.gameObject);
        }
    }
}
