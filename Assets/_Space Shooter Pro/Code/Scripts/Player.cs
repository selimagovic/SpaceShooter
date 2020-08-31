﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Variables
    public Text speedText=null;
    public Text powerUpText = null;
    [Header("Player Properties")]
    [SerializeField]
    private GameObject[] _engines = null;
    [SerializeField]
    [Range(2, 15)]
    private float _speed = 5f;

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
    private Vector2 _screenBoundUpDown= new Vector2();
    [SerializeField]
    private Vector2 _screenBoundLeftRight=new Vector2();

    [Header("Audio Properties")]
    [SerializeField]
    private AudioClip _laserSound=null;
    [SerializeField]
    private GameObject _laserPrefab=null;
    [SerializeField]
    private float _laserOffset = 1.05f;
    [SerializeField]
    private GameObject _tripleShootPrefab=null;
    [SerializeField]
    private GameObject _shieldGO = null;
    //private List<PowerUP> _powerUps = new List<PowerUP>();
    [SerializeField]
    private int _score=0;

    private PlayerControls _playerControls;
    InputAction _onMovementAction;
    InputAction _onShootingAction;

    bool isTrippleShotActive=false;
    private float _initialSpeed;

    public GameObject ShieldGO { get => _shieldGO;}

    private AudioSource _audioSource;
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
        speedText.text = "Speed: " + _initialSpeed.ToString();
        powerUpText.gameObject.SetActive(false);
        ShieldGO.gameObject.SetActive(false);
        _score = 0;
        if(_engines==null)
        {
            Debug.LogError("Please Add missing Game Objects at " + transform.name);
        }
        for (int i = 0; i < _engines.Length; i++)
        {
            _engines[i].SetActive(false);
        }
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource==null)
        {
            Debug.LogError("Please Add AudioSource missing Game Objects at " + transform.name);
        }
        else
            _audioSource.clip = _laserSound;
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

        _onShootingAction.Enable();
        _onMovementAction.Enable();
    }

    private void OnDisable()
    {
        _onMovementAction.Disable();
        _onShootingAction.Disable();
    }
    #endregion
    #region --Public Custom Methods--
    public void Damage()
    {
        _lives--;
        UIManager.Instance.UpdateLives(_lives);
        if(_lives==2)
        {
            _engines[1].SetActive(true);
        }
        else if(_lives==1)
        {
            _engines[0].SetActive(true);
        }
        if(_lives<1)
        {
            //comunicate with spawnManager
            //Let them know 
            SpawnManager.Instance.OnPlayerDeath();            
            Destroy(this.gameObject);
            //Debug.Log("Player Destroyed");
        }
    }

    public void AddPowerup(PowerType powerup)
    {
        switch(powerup)
        {
            case PowerType.TripleShot:
                Powerup("triple");
                break;
            case PowerType.Shield:
                Powerup("shield");
                break;
            case PowerType.Speed:
                Powerup("speed");
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
        transform.Translate(direction * _speed * Time.deltaTime);

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

    void Shoot()
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
            //play audio clip
        }
    }

    void Powerup(string powerup)
    {
        switch (powerup)
        {
            case "triple"://tripleshot
                isTrippleShotActive = true;
                powerUpText.text = "Powerup: " + PowerType.TripleShot.ToString();
                powerUpText.gameObject.SetActive(true);
                StartCoroutine(SetPowerup("triple"));
                break;
            case "shield":
                //activate shield game object
                ShieldGO.gameObject.SetActive(true);
                powerUpText.text = "Powerup: " + PowerType.Shield.ToString();
                powerUpText.gameObject.SetActive(true);
                StartCoroutine(SetPowerup("shield"));
                break;
            case "speed":
                _speed *= _speedMultiplier;
                speedText.text = "Speed: " + _speed.ToString();
                powerUpText.text ="Powerup: " +PowerType.Speed.ToString();
                powerUpText.gameObject.SetActive(true);
                StartCoroutine(SetPowerup("speed"));
                break;
        }
    }
    IEnumerator SetPowerup(string powerup)
    {                
        yield return new WaitForSeconds(_powerUPDuration);
        switch (powerup)
        {
            case "triple"://tripleshot
                isTrippleShotActive = false;
                powerUpText.gameObject.SetActive(false);
                break;
            case "shield": //activate shield game object
                ShieldGO.gameObject.SetActive(false);
                powerUpText.gameObject.SetActive(false);
                break;
            case "speed":
                _speed = _initialSpeed;
                speedText.text = "Speed: " + _speed.ToString();
                powerUpText.gameObject.SetActive(false);
                break;
        }
        
    }

    #endregion

}