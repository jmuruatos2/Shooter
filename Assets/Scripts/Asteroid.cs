using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _speed = 40.0f;
    [SerializeField]
    private GameObject _explotion;
    [SerializeField]
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();


    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            GameObject explotionObject = Instantiate(_explotion,new Vector3(transform.position.x, transform.position.y, transform.position.z),Quaternion.identity);
            explotionObject.GetComponent<AudioSource>().Play();
            Destroy(explotionObject, 3.0f);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject);
        }
    }
}
