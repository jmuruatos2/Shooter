using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HommingLaser : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    private Transform _target;
    [SerializeField]
    private float _rotateSpeed = 1000.0f;

    [SerializeField]
    private float _speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * _rotateSpeed);

        if (_target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        } 
        else
        {
            _enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (_enemy != null)
            {
                _target = _enemy.transform;
            }
        }
    }
}
