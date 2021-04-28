using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : MonoBehaviour
{
    private float _speed=3.0f;
    private GameObject _player;
    private bool _mirandoAbajo;
    private float _posX,_posY;
    [SerializeField]
    private GameObject _laser;
    private WaitForSeconds _wait;
    private Animator _animator;
    private bool _destruido;


    // Start is called before the first frame update
    void Start()
    {
        _destruido = false;
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null)
        {
            Debug.LogError("Objeto player es null");
        }
        _mirandoAbajo = true;
        _posX = transform.position.x;
        _posY = 8.0f;
        _wait = new WaitForSeconds(2.0f);
        _animator = transform.GetComponent<Animator>();
        //StartCoroutine(FireRate());
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
    } 

    public void Movimiento()
    {
        if (_destruido)
        {
            return;
        }
        _posY -= Time.deltaTime * _speed;
        Vector2 _movimiento = new Vector2(_posX,_posY);
        transform.position = _movimiento;

        if (_player.transform.position.y < transform.position.y && !_mirandoAbajo)
        {
            _mirandoAbajo = true;
            transform.Rotate(0, 0, 180);

        } else if (_player.transform.position.y > transform.position.y && _mirandoAbajo)
        {
            _mirandoAbajo = false;
            transform.Rotate(0, 0, 180);
        }

        if (_posY < -8.0f)
        {
            _posY = 8.0f;
        }

        RaycastHit2D hit;
        if (_mirandoAbajo)
        {
             hit = Physics2D.Raycast(transform.position - new Vector3(0, 1.0f, 0), Vector2.down);
            Debug.DrawRay(transform.position - new Vector3(0,1.0f,0), Vector2.down);

        } else
        {
             hit = Physics2D.Raycast(transform.position + new Vector3(0, 1.0f, 0), Vector2.up);
            Debug.DrawRay(transform.position + new Vector3(0, 1.0f, 0), Vector2.up);
        }
            
        if (hit.collider != null && hit.collider.gameObject.tag == "Player")
            {
                Fire();
            }

        


    }

    public void Fire()
    {
        if (_mirandoAbajo)
        {
            GameObject _laserObj =  Instantiate(_laser, new Vector2(transform.position.x, transform.position.y - 1.0f), Quaternion.identity);
            _laserObj.transform.Rotate(0, 0, 180);
        } else
        {
            Instantiate(_laser, new Vector2(transform.position.x, transform.position.y + 1.0f), Quaternion.identity);
            Debug.Log("Disparo arriba");
        }
    }

    private IEnumerator FireRate()
    {       
        yield return _wait;
            Fire();
        
    }

    public void Destruir()
    {
        _destruido = true;
        _animator.SetTrigger("Destroy");
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.0f);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destruir();
        } else if(other.CompareTag("Player"))
        {
            Player _player = other.GetComponent<Player>();
            _player.PlayerDamage();
            Destruir();
        }
    }
}
