using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private Player _player;
    private Animator _animator;
    private AudioSource _audioExplotion;
    [SerializeField]
    private GameObject _laser;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _audioExplotion = gameObject.GetComponent<AudioSource>();

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
        Instantiate(_laser, new Vector2(transform.position.x, transform.position.y -1.37f) , Quaternion.identity);
    }
    private void Destruir()
    {
        _animator.SetTrigger("destroy");
        _speed = 0;
        _audioExplotion.Play();
        Destroy(GetComponent<Collider2D>());
        //yield return new WaitForSeconds(2);
        Destroy(this.gameObject, 2.8f);
    }
}
