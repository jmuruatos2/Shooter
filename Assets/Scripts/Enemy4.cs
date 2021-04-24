using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{
    private Vector2 _movimiento1;
    private float _speed = 4.0f;
    private bool _playerLock;
    private GameObject _player;
    private Vector3 _playerPosition;
    private Animator _animator;
    private bool _destruido;
    
    // Start is called before the first frame update
    void Start()
    {
        _destruido = false;
        _animator = transform.GetComponent<Animator>();
        _movimiento1 = new Vector2(0, -1);
        _playerLock = false;
        _player = GameObject.FindGameObjectWithTag("Player");
        if (!_player)
        {
            Debug.Log("Error: Player no localizable");
        }

    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
    }

    private void Movimiento()
    {
        if (transform.position.y > 5.4)
        {
            transform.Translate(_movimiento1 * _speed * Time.deltaTime);
        } else if (transform.position.y <= 5.4 && transform.position.y > -6.58f && !_destruido)
        {
            if (!_playerLock)
            {
                PlayerSearch();
            } else
            {
                transform.Translate(Vector2.down * Time.deltaTime * _speed);
            }
        }
    }
    private void PlayerSearch()
    {
        float _distance = Mathf.Abs(Vector2.Distance(transform.position, _player.transform.position));

        if (_distance < 4.0f)
        {
            _playerLock = true;
            float co = _player.transform.position.x - transform.position.x;
            float ca = _player.transform.position.y - transform.position.y;
            float angulo = Mathf.Atan(co / ca);

            transform.Rotate(0, 0, -Mathf.Rad2Deg * angulo);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Player playerScript = _player.GetComponent<Player>();
        playerScript.PlayerDamage();
        _animator.SetTrigger("Destroy");
        _destruido = true;
        Destroy(this.gameObject,1.5f);
    }
}
