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

    public bool IsEnemyLaser { get; private set; } = false;

    bool shield = false;
    #endregion
    #region Builtin Methods

    private void Update()
    {
        //if it is Enemy laser then move laser down
        if (IsEnemyLaser == true)
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
        if (other.CompareTag("Player") && IsEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            //check if shield is active on player if not then give damage to player
            if (player != null)
            {
                if (shield == false)
                    player.Damage();
            }
            Destroy(this.gameObject);
        }
    }
    #endregion
    #region Public Methods
    public void AssignEnemyLaser()
    {
        IsEnemyLaser = true;
    }
    public void AssignPlayerShield(bool shieldValue)
    {
        shield = shieldValue;
    }
    #endregion
}
