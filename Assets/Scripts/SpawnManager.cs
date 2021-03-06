using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private bool playing;
    [SerializeField]
    private GameObject _enemyObject;
    [SerializeField]
    private GameObject[] _enemy2Object;
    [SerializeField]
    private GameObject _enemy3Object;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    private float _secondsWaitNextWave;
    private float _waitFrequencyInWave;
    private int _enemyIndex;
    private WaitForSeconds _wait5Seconds;
  

    // Start is called before the first frame update
    void Start()
    {
        _wait5Seconds = new WaitForSeconds(5);
    }

    public void StartSpawning()
    {
        playing = true;
        //StartCoroutine(SpawnEnemy(5)); //spawn enemy every 5 seconds
        StartCoroutine("WaveManager");
        StartCoroutine("SpawnPowerup");
        StartCoroutine("SpawnMissile");
    }

    public void StartSpawningBoss()
    {
        playing = true;
        StartCoroutine(SpawnPowerupBoss1());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy(float timeInSeconds)
    {
        yield return new WaitForSeconds(3.0f);
        

        while (playing)
        {

            GameObject newEnemy = Instantiate(_enemyObject, new Vector3(RandomX(), 8), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(timeInSeconds);
            

        }
    }

    public IEnumerator WaveManager()
    {
        
        _secondsWaitNextWave = 30;
        _waitFrequencyInWave = 5;
        _enemyIndex = 0;
        StartCoroutine("SpawnEnemy2");
        yield return new WaitForSeconds(_secondsWaitNextWave);
        StopCoroutine("SpawnEnemy2");
        _secondsWaitNextWave = 30;
        _waitFrequencyInWave = 3;
        _enemyIndex = 1;
        GameObject SpecialEnemy = Instantiate(_enemy3Object, new Vector3(-11.97f, 0.21f, 0), Quaternion.identity);
        SpecialEnemy.transform.rotation = Quaternion.Euler(0, 0, 90);
        StartCoroutine("SpawnEnemy2");
        yield return new WaitForSeconds(_secondsWaitNextWave);
        StopCoroutine("SpawnEnemy2");
    }

    public IEnumerator SpawnEnemy2()
    {
        int i = 0;
        WaitForSeconds espera = new WaitForSeconds(_waitFrequencyInWave);
        
        while (playing)
        {
            GameObject newEnemy = Instantiate(_enemy2Object[i], new Vector3(RandomX(), 8), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return espera;

            if (i < _enemyIndex)
            {
                i++;
            } else
            {
                i = 0;
            }


        }
        

    }

    IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(3.0f);

        while (playing)
        {
            yield return new WaitForSeconds(Random.Range(3, 7));
            int _powerupIndex = Random.Range(0, 7);
            Instantiate(_powerups[_powerupIndex], new Vector3(RandomX(), 8), Quaternion.identity);
        }
    }

    private IEnumerator SpawnMissile()
    {
        while (playing)
        {
            yield return new WaitForSeconds(Random.Range(10, 20));
            Instantiate(_powerups[5], new Vector3(RandomX(), 8), Quaternion.identity);
        }
    }

    private IEnumerator SpawnPowerupBoss1()
    {
        while (playing)
        {
            yield return _wait5Seconds;
            Instantiate(_powerups[3], new Vector3(RandomX(), 8), Quaternion.identity);
        }
    }

    // Random Value in X axis
    private float RandomX()
    {
        return Random.Range(-9.5f, 9.5f);
    }

    public void StopPlaying()
    {
        playing = false;
    }
}
