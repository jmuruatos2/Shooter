using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    [SerializeField]
    private bool _firing;
    [SerializeField]
    private GameObject _laserBeam;
    private RectTransform _laserBeamTransform;
    private Vector3 _vectorEscala, _vectorTranslate;
    private int _scaleSpeed = 20;
    private bool _finishFire;
    private bool _exitScene;
    // Start is called before the first frame update
    void Start()
    {
        _firing = false;
        _vectorEscala = new Vector3(0.005f, 0.1f,0);
        _vectorTranslate = new Vector3(0, 0.1f, 0);
        _finishFire = false;
        _exitScene = false;
        
        _laserBeamTransform = _laserBeam.GetComponent<RectTransform>();
        StartCoroutine(EnemyBehaviour());
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -9.0f)
        {
            transform.Translate(Vector2.down * Time.deltaTime * _speed);
        } 


        if (_firing == true && _laserBeam.transform.localScale.y < 4)
        {

            _laserBeam.transform.Translate(-_vectorTranslate * Time.deltaTime * 2.5f * _scaleSpeed);
            _laserBeam.gameObject.transform.localScale = _laserBeam.transform.localScale + _vectorEscala * Time.deltaTime * _scaleSpeed;
            
           
        }

        if (_finishFire)
        {
            _laserBeam.transform.Translate(-_vectorTranslate * Time.deltaTime * 2.5f * _scaleSpeed);
        }

        if (_exitScene)
        {
            transform.Translate(Vector2.down * Time.deltaTime * _speed);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 140), 10.0f * Time.deltaTime);
        }

    }

    private IEnumerator EnemyBehaviour()
    {
        yield return new WaitForSeconds(2.0f);
        _firing = true;
        _laserBeam.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        _firing = false;
        _finishFire = true;
        yield return new WaitForSeconds(3.0f);
        _finishFire = false;
        yield return new WaitForSeconds(1.0f);
        _exitScene = true;
        Destroy(this.gameObject, 4.0f);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Player"))
        {
            Player _player = other.gameObject.GetComponent<Player>();
            _player.PlayerDamage();
        }
    }

}
