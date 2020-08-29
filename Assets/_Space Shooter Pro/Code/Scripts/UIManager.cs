using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables
    private static UIManager _instance;

    public static UIManager Instance 
    {
        get
        {
            if(_instance==null)
            {
                Debug.LogError("UIManager instance is null!!!!");
            }
            return _instance;
        }
    }
    [Header("UI Properties")]
    [SerializeField]
    private Text _scoreText = null;
    [SerializeField]
    private Image _livesImage=null;
    [SerializeField]
    private Sprite[] _livesSprites = null;
    [SerializeField]
    private GameObject _gameoverGO = null;
    [SerializeField]
    private Text _finalScore = null;
    #endregion
    #region Builtin Methods
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        _scoreText.text = "Score: "+0;
        _gameoverGO.gameObject.SetActive(false);
        _finalScore.text = "Total Score: " + 0;
    }
    #endregion
    #region --Public Custom Methods--
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
        _finalScore.text = "Total Score: " + playerScore.ToString();
    }
    public void UpdateLives(int currentLives)
    {
        //display image sprite
        //give it a new one based on the current lives index
        _livesImage.sprite = _livesSprites[currentLives];
        if (currentLives == 0)
        {
            GameOverSequence();

        }
    }
    #endregion
    #region --Private Custom Methods--
    void GameOverSequence()
    {
        _gameoverGO.gameObject.SetActive(true);
        GameManager.Instance.GameOver();
        StartCoroutine(GameOverFlickerRoutine());
    }
    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameoverGO.GetComponentInChildren<Text>().text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameoverGO.GetComponentInChildren<Text>().text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion
}
