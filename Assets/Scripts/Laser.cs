using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {

        
    }


    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 8 && transform.position.y > -8)
        {
            transform.Translate(Vector3.up * Time.deltaTime * _speed);
        } else
        {
            if (transform.parent != null )
            {
                Destroy(transform.parent.gameObject);
            } else
            {
                Destroy(this.gameObject);
            }
            
        }
        
    }
}
