using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //handle to text
    [SerializeField]
    private Text _scoreText;
    private Player score;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Text _gameoverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        score = GameObject.Find("Player").GetComponent<Player>();
        _scoreText.text = "Score: " + score.ReturnScore();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (score == null)
        {
            Debug.LogError("player is null at uimanager");
        }
    }
        // Update is called once per frame
    void Update()
    {
        if (score != null)
        {
            _scoreText.text = "Score: " + score.ReturnScore();
        }
        gameOverSequence();
    }
    private void gameOverSequence()
    {
        if (score.ReturnLives() < 1)
        {
            if(_gameManager != null)
            {
                _gameManager.gameOver();
            }
        }
    }
    public void updateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
    }
    public void ShowGameOver()
    {
        _gameoverText.gameObject.SetActive(true);
        StartCoroutine(Flickering());
        _restartText.gameObject.SetActive(true);
        //you can press the r key, hide the message, when the game is over 
    }
    IEnumerator Flickering()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            _gameoverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
            _gameoverText.gameObject.SetActive(true);
        }
    }
}