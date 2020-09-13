using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private static Shield _instance;
    #region Variables
    private SpriteRenderer _shieldColor = null;
    private int _shieldHit = 3;

    public static Shield Instance 
    {
        get
        {
            if(_instance==null)
            {
                Debug.LogError("Shield Instance is NULL!!!");
            }
            return _instance;
        }
    }
    #endregion
    #region Builtin Methods
    private void Awake()
    {
        _instance = this;
        _shieldColor = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            SheildDamageColor();
            other.GetComponent<Animator>().SetTrigger("OnEnemyDeath");
            Destroy(other.GetComponent<Collider2D>());
            Destroy(other.gameObject, 2.8f);
        }
    }
    #endregion
    #region --Public Custom Methods--
    /// <summary>
    /// Shield visualisation method.
    /// </summary>
    public void SheildDamageColor()
    {
        Color color;
        switch (_shieldHit)
        {
            case 3:
                color = new Color(1.0f, 1.0f, 1.0f, 0.27f);//one Hit shield color alha to lower 
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
    #endregion
}
