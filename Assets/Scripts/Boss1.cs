using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1 : MonoBehaviour
{
    private float _speed;
    private int _stage;
    private int _health;
    private GameObject _playerObject;
    [SerializeField]
    private float _speedRotation = 50.0f;
    [SerializeField]
    private GameObject _fireball1;
    [SerializeField]
    private GameObject _explosion;
    private SpawnManager _spawnManager;

    private bool _waiting;
    private bool _firing;
    private bool _stageStarted;
    private Slider _slider;
    [SerializeField]
    private GameObject _enemyBar;
    private Player _player;


    // Start is called before the first frame update
    void Start()
    {
        _speed = 4.0f;
        _stage = 0;
        _waiting = false;
        _firing = false;
        _stageStarted = false;
        _health = 100;

        _playerObject = GameObject.FindGameObjectWithTag("Player");

        if (_playerObject == null)
        {
            Debug.LogError("Error Player no encontrado");
        }
        _player = _playerObject.GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player script no encontrado");
        }

        _slider = _enemyBar.GetComponent<Slider>();
        _slider.value = _health;

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("SpawnManager no encontrado");
        }

        _spawnManager.StartSpawningBoss();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_stage)
        {
            case 0:
                FirstStage();
                break;
            case 1:
                SecondStage();
                break;
            case 2:
                ThirdStage();
                break;
        }
    }

    private void FirstStage()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
        if (transform.position.y < 4.5)
        {
            _stage = 1;
        }
    }

    private void SecondStage()
    {
        if (!_stageStarted)
        {
            _stageStarted = true;
            _waiting = true;
            StartCoroutine("Wait");
        }

        if (_waiting)
        {
            LookAt();
          
        } else if (!_firing)
        {
            _firing = true;
            StartCoroutine("Fire");
        }
    } 

    private void LookAt()
    {
        Vector3 vectorToTarget = _playerObject.transform.position - transform.position;
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _speedRotation);
    }
    private void ThirdStage()
    {
        if (!_stageStarted)
        {
            _stageStarted = true;
            _waiting = true;
            StartCoroutine("Wait");
        }

        if (_waiting)
        {
            LookAt();
        } 
        else if (!_firing)
        {
            _firing = true;
            StartCoroutine("Fire2");
        }
    }

    private IEnumerator Wait()
    {
        
        yield return new WaitForSeconds(3);
        _waiting = false;
    }

    private IEnumerator Fire()
    {
        int i = 0;
        while (i < 10)
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(_fireball1, transform.position + transform.right*4, transform.rotation * Quaternion.Euler(0,0,90));
            i++;
        }
        _firing = false;
        _stageStarted = false;
    }

    private IEnumerator Fire2()
    {
        int i = 0;
        while (i < 10)
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(_fireball1, transform.position + transform.right * 4, transform.rotation * Quaternion.Euler(0, 0, 20));
            Instantiate(_fireball1, transform.position + transform.right * 4, transform.rotation * Quaternion.Euler(0, 0, 40));
            Instantiate(_fireball1, transform.position + transform.right * 4, transform.rotation * Quaternion.Euler(0, 0, 60));
            Instantiate(_fireball1, transform.position + transform.right * 4, transform.rotation * Quaternion.Euler(0, 0, 70));
            Instantiate(_fireball1, transform.position + transform.right * 4, transform.rotation * Quaternion.Euler(0, 0, 90));
            Instantiate(_fireball1, transform.position + transform.right * 4, transform.rotation * Quaternion.Euler(0, 0, 110));
            Instantiate(_fireball1, transform.position + transform.right * 4, transform.rotation * Quaternion.Euler(0, 0, 130));
            Instantiate(_fireball1, transform.position + transform.right * 4, transform.rotation * Quaternion.Euler(0, 0, 150));
            Instantiate(_fireball1, transform.position + transform.right * 4, transform.rotation * Quaternion.Euler(0, 0, 170));
            i++;
        }
        _firing = false;
        _stageStarted = false;

    }

    public void Damage()
    {
        _health -= 1;
        _slider.value = _health;

        if(_health <=50 && _stage == 1)
        {
            _stageStarted = false;
            _stage = 2;
        }
        if (_health <= 0)
        {
            Destruir();
        }

    }

    private void Destruir()
    {
        _spawnManager.StopPlaying();
        Instantiate(_explosion, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player.PlayerDamage();
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Damage();

        }
    }
}
