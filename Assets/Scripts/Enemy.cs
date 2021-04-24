using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public float _speed = 4.0f;
    [SerializeField]
    private Player _player;
    private Animator _animator;
    private AudioSource _audioExplotion;
    [SerializeField]
    private GameObject _laser;
    public bool _isEnemyAlive;
    public bool _shieldActive;
    [SerializeField]
    public GameObject _shieldObject;

    // Start is called before the first frame update
    public virtual void Start()
    {
        _isEnemyAlive = true;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _audioExplotion = gameObject.GetComponent<AudioSource>();

        if (Random.Range(0,6) == 2)
        {
            _shieldActive = true;
            _shieldObject.SetActive(true);

        }



        if(_animator == null)
        {
            Debug.LogError("Animator Enemy is Null");
        }
        if (_audioExplotion == null)
        {
            Debug.LogError("AudioSource is null");
        }

        StartCoroutine(Shot());

    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
       

    }

    public virtual void Movimiento()
    {
        if (transform.position.y < -5.85f)
        {
            transform.position = new Vector3(Random.Range(-14f, 14f), 7.67f);
        }

        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.addScore(10);
            }
            Destruir();
        } else if (other.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            if ( player != null)
            {
                player.PlayerDamage();
            }

            Destruir();
        }


    }

    private IEnumerator Shot()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        if (_isEnemyAlive)
        {
            Instantiate(_laser, new Vector2(transform.position.x, transform.position.y - 1.37f), Quaternion.identity);
        }
    }
    public virtual void Destruir()
    {
        if(_shieldActive)
        {
            _shieldActive = false;
            _shieldObject.SetActive(false);
            return;
        }

        _animator.SetTrigger("destroy");
        _speed = 0;
        _audioExplotion.Play();
        Destroy(GetComponent<Collider2D>());
        //yield return new WaitForSeconds(2);
        _isEnemyAlive = false;
        Destroy(this.gameObject, 2.8f);
    }
}
