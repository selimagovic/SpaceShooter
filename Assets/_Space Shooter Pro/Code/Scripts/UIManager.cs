using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditorInternal;
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

    public Slider BoostSlider { get => _boostSlider; private set => _boostSlider = value; }

    public bool canBoost = true;
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
    private Slider _boostSlider = null;
    [SerializeField]
    private Text _finalScore = null;
    [SerializeField]
    private Text _ammo = null;
    #endregion
    #region Builtin Methods
    private void Awake()
    {
        _instance = this;
        canBoost = true;
    }
    private void Start()
    {
        
        _scoreText.text = "Score: "+0;
        _gameoverGO.gameObject.SetActive(false);
        _finalScore.text = "Total Score: " + 0;
        _ammo.text = "Ammo: 15/15";
        _boostSlider.value = 1f;
    }
    private void Update()
    {
    }
    #endregion
    #region --Public Custom Methods--
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
        _finalScore.text = "Total Score: " + playerScore.ToString();
    }
    public void UpdateAmmoUI(int currentAmmo,int maxAmmo=15)
    {
        _ammo.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
    }
    public void UpdateLives(int currentLives)
    {
        //display image sprite
        //give it a new one based on the current lives index
        if(currentLives>0)
            _livesImage.sprite = _livesSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
        
    }

    public void UpdateSlider(float targetValue = 1.0f,float targetDuration=0.015f)
    {
        _boostSlider.value = Mathf.MoveTowards(_boostSlider.value, targetValue, targetDuration);
        
        if(_boostSlider.value <= 0.01f)
        {
            canBoost = false;
            StopAllCoroutines();
            StartCoroutine(SliderCoolDown());
        }       
    }


    public void StartCoroutineUI(GameObject go, string text,float delay=0.5f)
    {
        StartCoroutine(FlickerRoutine(go, text,delay));
    }

    #endregion
    #region --Private Custom Methods--
    void GameOverSequence()
    {
        _gameoverGO.gameObject.SetActive(true);
        GameManager.Instance.GameOver();
        StartCoroutine(FlickerRoutine(_gameoverGO));
    }
    private IEnumerator FlickerRoutine(GameObject go, string text = "GAME OVER",float delay=0.5f)
    {
        while (true)
        {
            go.GetComponentInChildren<Text>().text = text;
            yield return new WaitForSeconds(delay);
            go.GetComponentInChildren<Text>().text = "";
            yield return new WaitForSeconds(delay);
        }
    }
    private IEnumerator SliderCoolDown(float targetValue = 1.0f, float targetDuration = 0.0015f)
    {
        while (_boostSlider.value != 1.0f)
        {
            _boostSlider.value = Mathf.MoveTowards(_boostSlider.value, targetValue, targetDuration);
            yield return new WaitForEndOfFrame();
            if(_boostSlider.value == 1.0f)
            {
                canBoost = true;
                break;
            }
        }
    }
    #endregion
}
