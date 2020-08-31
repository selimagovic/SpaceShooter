using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private float _screenUpperBound = 8.0f;

    private bool _isEnemyLaser = false;
    #endregion
    #region Builtin Methods

    private void Update()
    {
        //if it is Enemy laser then move laser down
        if (_isEnemyLaser)
        {
            MoveDown();
        }
        else
        {
            MoveUp();           
        }
    }
    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        //destroy go after certain bound is met
        if (transform.position.y > _screenUpperBound)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //destroy go after certain bound is met
        if (transform.position.y < -_screenUpperBound)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            if(player!=null)
            {
                player.Damage();
            }
        }
    }
    #endregion
    #region Public Methods
    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
    #endregion
}
