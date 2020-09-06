using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// Created By Selim Agovic
/// Player controll script uses Unity New Input system, 
/// every code is designed to work with new input system.
/// </summary>
public class Player : MonoBehaviour
{
    #region Variables

    public Text powerUpText = null;
    [Header("Player Properties")]
    [SerializeField]
    private GameObject[] _engines = null;
    [SerializeField]
    private GameObject _thruster = null;
    [SerializeField]
    [Range(2, 18)]
    private float _speed = 5f;
    [SerializeField]
    [Range(1.0f, 6.0f)]
    private float _boostMultiplier = 1.5f;
    [SerializeField]
    [Range(1f, 5f)]
    private float _speedMultiplier = 2f;
    [SerializeField]
    private float _powerUPDuration = 5f;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private Vector2 _screenBoundUpDown = new Vector2();
    [SerializeField]
    private Vector2 _screenBoundLeftRight = new Vector2();

    [Header("Audio Properties")]
    [SerializeField]
    private AudioClip _laserSound = null;
    [Space]
    [Header("Laser Properties")]
    public Text ammoText = null;
    [SerializeField]
    private GameObject _laserPrefab = null;
    [SerializeField]
    private float _laserOffset = 1.05f;
    [SerializeField]
    private int _maxAmmo = 15;
    private int _currentAmmo = 0;
    [Space]
    [Header("Powerup Properties")]
    [SerializeField]
    private GameObject _tripleShootPrefab = null;
    [SerializeField]
    private GameObject _shieldGO = null;

    private int _score = 0;
    private PlayerControls _playerControls;
    InputAction _onMovementAction;
    InputAction _onShootingAction;
    InputAction _onBoostAction;

    bool isTrippleShotActive = false;
    private float _initialSpeed;

    public GameObject ShieldGO { get => _shieldGO; }
    public InputAction OnShootingAction { get => _onShootingAction; set => _onShootingAction = value; }

    private AudioSource _audioSource;
    private SpriteRenderer _thrusterSpriteRenderer;
    #endregion
    #region Builtin Methods
    private void Awake()
    {
        _playerControls = new PlayerControls();
        _onShootingAction = _playerControls.Player.Shooting;
        _onShootingAction.performed += ctx => Shoot();

    }

    private void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _initialSpeed = _speed;
        //speedText.text = "Speed: " + _initialSpeed.ToString();
        powerUpText.gameObject.SetActive(false);
        ShieldGO.gameObject.SetActive(false);

        //change Thruister Collor when boost is applied
        _thrusterSpriteRenderer = _thruster.GetComponent<SpriteRenderer>();
        if (_thrusterSpriteRenderer == null)
        {
            Debug.LogError("Please Add missing Game Objects {Thruster} at " + transform.name);
        }
        _score = 0;
        if (_engines == null)
        {
            Debug.LogError("Please Add missing Game Objects at " + transform.name);
        }
        for (int i = 0; i < _engines.Length; i++)
        {
            _engines[i].SetActive(false);
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Please Add AudioSource missing Game Objects at " + transform.name);
        }
        else
            _audioSource.clip = _laserSound;

        _currentAmmo = _maxAmmo;
    }
    private void FixedUpdate()
    {
        CalculateMovement();
    }
    private void OnEnable()
    {
        _playerControls.Enable();
        _onMovementAction = _playerControls.Player.Movement;
        _onShootingAction = _playerControls.Player.Shooting;
        _onBoostAction = _playerControls.Player.Boosting;

        _onShootingAction.Enable();
        _onMovementAction.Enable();
        _onBoostAction.Enable();
    }

    private void OnDisable()
    {
        _onMovementAction.Disable();
        _onShootingAction.Disable();
        _onBoostAction.Disable();
    }
    #endregion
    #region --Public Custom Methods--
    public void Damage()
    {
        _lives--;
        UIManager.Instance.UpdateLives(_lives);

        if (_lives == 2)
        {
            _engines[1].SetActive(true);
        }
        else if (_lives == 1)
        {
            _engines[0].SetActive(true);
        }

        if (_lives == 0)
        {
            //comunicate with spawnManager
            //Let them know 
            SpawnManager.Instance.OnPlayerDeath();
            Destroy(this.gameObject);
            //Debug.Log("Player Destroyed");
        }
        UIManager.Instance.UpdateLives(_lives);
    }

    public void AddPowerup(PowerType powerup)
    {
        switch (powerup)
        {
            case PowerType.TripleShot:
                Powerup(powerup);
                break;
            case PowerType.Shield:
                Powerup(powerup);
                break;
            case PowerType.Speed:
                Powerup(powerup);
                break;
            case PowerType.Ammo:
                Powerup(powerup);
                break;
            case PowerType.Health:
                if (_lives < 3)
                {
                    Powerup(powerup);
                }
                break;
        }
    }
    public void AddScore(int points)
    {
        _score += points;
        UIManager.Instance.UpdateScore(_score);
    }
    #endregion
    #region --Private Custom Methods--
    void CalculateMovement()
    {
        Vector2 movement = _onMovementAction.ReadValue<Vector2>();

        Vector3 direction = new Vector3(movement.x, movement.y);

        //If we press left shift(keyboard) or right trigger (gamepad) boost is activated, othervise normal speed is activated
        //as a on screen visualisation thruster collor is changed and set to normal when deactivated.
        if (_onBoostAction.activeControl != null)
        {
            Boost(direction);
            //Debug.Log("Boost speed is: "+ _speed);
        }
        else
        {
            _speed = _initialSpeed;
            transform.Translate(direction * _speed * Time.deltaTime);
            _thrusterSpriteRenderer.color = new Color(255, 255, 255, 255);
            //speedText.text = "Speed: " + _speed.ToString();
            //Debug.Log("Normal speed is: " + _speed);
        }


        //Clamp Vertical screen position
        transform.position = new Vector3(transform.position.x,
            Mathf.Clamp(transform.position.y, _screenBoundUpDown.y, _screenBoundUpDown.x), 0);

        if (transform.position.x >= _screenBoundLeftRight.x)
        {
            transform.position = new Vector3(_screenBoundLeftRight.y, transform.position.y, 0);
        }
        else if (transform.position.x <= _screenBoundLeftRight.y)
        {
            transform.position = new Vector3(_screenBoundLeftRight.x, transform.position.y, 0);
        }
    }
    void Boost(Vector3 direction)
    {
        if (UIManager.Instance.canBoost == true)
        {
            transform.Translate(direction * (_speed * _boostMultiplier) * Time.deltaTime);
            _thrusterSpriteRenderer.color = new Color(0, 176, 255, 255);
            UIManager.Instance.UpdateSlider(0, 0.005f);
        }
        else
        {
            _thrusterSpriteRenderer.color = new Color(255, 255, 255, 255);
            transform.Translate(direction * _speed * Time.deltaTime);
    }


        //speedText.text = "Speed: " + _speed.ToString();
    }
    void Shoot()
    {
        if (_currentAmmo <=0)
        {
            StopAllCoroutines();
            UIManager.Instance.StartCoroutineUI(ammoText.gameObject, "Ammo: 0/15", 0.25f);
            _onShootingAction.Disable();
        }
        else
        {
            if (Time.time > _canFire)
            {
                _canFire = Time.time + _fireRate;
                if (isTrippleShotActive == true)
                {
                    Instantiate(_tripleShootPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
                }
                _audioSource.PlayOneShot(_laserSound);
                _currentAmmo--;
                UIManager.Instance.UpdateAmmoUI(_currentAmmo, _maxAmmo);
                //play audio clip
            }
        }
    }
    void Powerup(PowerType powerup)
    {
        switch (powerup)
        {
            case PowerType.TripleShot://tripleshot
                isTrippleShotActive = true;
                //powerUpText.text = "Powerup: " + PowerType.TripleShot.ToString();
                //powerUpText.gameObject.SetActive(true);
                StartCoroutine(SetPowerup(powerup));
                break;
            case PowerType.Shield:
                //activate shield game object
                _shieldGO.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                _shieldGO.gameObject.SetActive(true);
                _laserPrefab.GetComponent<Laser>().AssignPlayerShield(true);
                //powerUpText.text = "Powerup: " + PowerType.Shield.ToString();
                //powerUpText.gameObject.SetActive(true);
                StartCoroutine(SetPowerup(powerup, _powerUPDuration));//for testing purposes i used _powerUPDuration variable
                break;
            case PowerType.Speed:
                _speed *= _speedMultiplier;
                //speedText.text = "Speed: " + _speed.ToString();
                //powerUpText.text ="Powerup: " +PowerType.Speed.ToString();
                //powerUpText.gameObject.SetActive(true);

                StartCoroutine(SetPowerup(powerup));
                break;
            case PowerType.Ammo:
                _currentAmmo = 15;
                UIManager.Instance.UpdateAmmoUI(_currentAmmo);
                break;
            case PowerType.Health:
                _lives++;
                UIManager.Instance.UpdateLives(_lives);
                break;
        }
    }

    //coroutine is cooldown system for powerup
    IEnumerator SetPowerup(PowerType powerup, float powerUpDuration = 5.0f)
    {
        yield return new WaitForSeconds(powerUpDuration);
        switch (powerup)
        {
            case PowerType.TripleShot://tripleshot
                isTrippleShotActive = false;
                //powerUpText.gameObject.SetActive(false);
                break;
            case PowerType.Shield: //activate shield game object
                _laserPrefab.GetComponent<Laser>().AssignPlayerShield(false);
                ShieldGO.gameObject.SetActive(false);
                //powerUpText.gameObject.SetActive(false);
                break;
            case PowerType.Speed:
                _speed = _initialSpeed;
                //speedText.text = "Speed: " + _speed.ToString();
                //powerUpText.gameObject.SetActive(false);
                break;
        }

    }

    #endregion

}
