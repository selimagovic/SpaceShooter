using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyLaserPrefab=null;
    private SpriteRenderer _shieldColor=null;
    private int _shieldHit=3;

    private Laser[] _laser;
    private void Start()
    {
        _shieldColor = GetComponent<SpriteRenderer>();
        _laser = _enemyLaserPrefab.GetComponentsInChildren<Laser>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            SheildDamageColor();
            other.GetComponent<Animator>().SetTrigger("OnEnemyDeath");
            Destroy(other.GetComponent<Collider2D>());
            Destroy(other.gameObject,2.8f);
        }
        if(other.CompareTag("Laser") && IsEnemyLaser())
        {
            SheildDamageColor();
            Destroy(other.gameObject);
        }
    }
    /// <summary>
    /// Shield visualisation method.
    /// </summary>
    private void SheildDamageColor()
    {
        Color color;
        switch (_shieldHit)
        {
            case 3:
                color = new Color(1.0f, 1.0f, 1.0f,0.27f);//one Hit shield color alha to lower 
                _shieldColor.color = color;
                break;
            case 2:
                color = new Color(1.0f, 1.0f, 1.0f, 0.019f);//Damaged Shield collor almost transparent 
                _shieldColor.color = color;
                break;
            case 1:
                transform.gameObject.SetActive(false);
                break;
        }
        _shieldHit--;
    }

    private bool IsEnemyLaser()
    {
        int laserCount = 0;
        for (int i = 0; i < _laser.Length; i++)
        {
            if (_laser[i].IsEnemyLaser)
            {
                laserCount++;
            }
        }
        if (laserCount == _laser.Length)
            return true;
        else
            return false;
    }
}
