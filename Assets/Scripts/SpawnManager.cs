using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private bool playing;
    [SerializeField]
    private GameObject _enemyObject;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
  

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartSpawning()
    {
        playing = true;
        StartCoroutine(SpawnEnemy(5)); //spawn enemy every 5 seconds
        StartCoroutine("SpawnPowerup");
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

    IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(3.0f);

        while (playing)
        {
            yield return new WaitForSeconds(Random.Range(3, 7));
            int _powerupIndex = Random.Range(0, 3);
            Instantiate(_powerups[_powerupIndex], new Vector3(RandomX(), 8), Quaternion.identity);
        }
    }


    // Random Value in X axis
    private float RandomX()
    {
        return Random.Range(-11.0f, 11.0f);
    }

    public void StopPlaying()
    {
        playing = false;
    }
}
