using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _restartText;
    private bool _playing;
    private bool _gameOverVisible;
    private GameManager _gameManager;

    void Start()
    {
        _scoreText.text = "Score: 0";
        _gameOver.gameObject.SetActive(false);
        _playing = true;
        _gameOverVisible = false;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("GameManager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

   public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }
 
    public void UpdateLives(int lives)
    {
        _livesImage.sprite = _liveSprites[lives];
    }

    public void ShowGameOverText()
    {
        _playing = false;
        StartCoroutine("ShowGameOverFlick");
        _restartText.gameObject.SetActive(true);
    }

    private IEnumerator ShowGameOverFlick()
    {
        while (!_playing)
        {
            _gameOver.gameObject.SetActive(_gameOverVisible);
            _gameOverVisible = !_gameOverVisible;
            _gameManager.GameOver();
            yield return new WaitForSeconds(1);
            
        }
    }

}
