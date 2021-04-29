using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private Vector3 _vectorOffset;

    // Start is called before the first frame update
    void Start()
    {
        _vectorOffset = new Vector3(0.0f, 0.5f, 0.0f);
        
    }


    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 8 && transform.position.y > -8)
        {
            transform.Translate(Vector3.up * Time.deltaTime * _speed);
        } else
        {
            DestroyLaser();
            
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position + _vectorOffset, Vector3.up);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                enemy.Evadir(transform.position.x);
                
            }
        }
        
        
    }

    public void DestroyLaser()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
