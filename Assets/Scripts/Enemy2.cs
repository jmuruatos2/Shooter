using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{

    private float _angulo;
    private float _anguloVista;
    private float _radio = 4.0f;
    private float _velocidadRotacionRad = 2.0f;
    private bool _home;
    private Vector2 _centro;
    private Animator _animator;

    
    public override void Start()
    {
        _angulo = 0;
        _home = false;          // El enemigo no ha llegado al punto de inicio de rutina
        _animator = GetComponent<Animator>();
    }

    
    public override void Movimiento()
    {



        if (_home)
        {
            _angulo -= _velocidadRotacionRad * Time.deltaTime;
            _anguloVista = Mathf.Rad2Deg * _velocidadRotacionRad * Time.deltaTime;
            Vector2 vector = new Vector2(_centro.x + _radio * Mathf.Cos(_angulo), _centro.y + _radio * Mathf.Sin(_angulo));
            transform.position = vector;
            transform.Rotate(0, 0, -_anguloVista);
        } else
        {
            transform.Translate(Vector3.down * _velocidadRotacionRad * Time.deltaTime);
            if (transform.position.y <= 0)
            {
                _home = true;
                _centro = new Vector2(transform.position.x - _radio, transform.position.y);
            }
        }
    }

    public override void Destruir()
    {

        _animator.SetTrigger("destroy");
        _speed = 0;
        //_audioExplotion.Play();
        Destroy(GetComponent<Collider2D>());
        //yield return new WaitForSeconds(2);
        _isEnemyAlive = false;
        Destroy(this.gameObject, 1.3f);
    }
}
