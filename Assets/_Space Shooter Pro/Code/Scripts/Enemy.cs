using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _laserPrefab=null;
    [SerializeField]
    private float _deathSpeed = 2f;

    private Player _player;
    private Animator _anim;

    private float _fireRate = 3f;
    private float _canFire = -1;
    AudioSource _aSource;
    #endregion
    #region Builtin Methods
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player==null)
        {
            Debug.LogError("Player is NULL!!!");
        }
        _anim = gameObject.GetComponent<Animator>();
        if(_anim==null)
        {
            Debug.LogError("Animator is NULL!!!");
        }
        _aSource = GetComponent<AudioSource>();
        if(_aSource == null)
        {
            Debug.LogError("AudioSource is NULL!!!");
        }
    }
    private void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser =  Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        //move down at 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           //Player player = other.transform.GetComponent<Player>();
            if (_player != null && _player.ShieldGO.activeSelf == false)
            {
                _player.Damage();
                _anim.SetTrigger("OnEnemyDeath");
                _speed /= _deathSpeed;
                _aSource.Play();
                Destroy(this.gameObject,2.8f);
            }
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if(_player !=null)
            {
                _player.AddScore(Random.Range(2,10));
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed /= _deathSpeed;
            _aSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject,2.8f);
        }
    }
    #endregion
}
