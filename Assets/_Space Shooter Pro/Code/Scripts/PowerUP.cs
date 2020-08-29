using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerType
{
    TripleShot,
    Shield,
    Speed
}
public class PowerUP : MonoBehaviour
{

    #region Variables
    [SerializeField]
    private PowerType powerType = PowerType.TripleShot;
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _lowerScreenBound = -4.5f;

    public PowerType PowerType { get => powerType;}
    [SerializeField]
    AudioClip _clip = null;
    #endregion
    #region Builtin Methods
    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < _lowerScreenBound)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if (player != null)
            {
                switch (powerType)
                {
                    case PowerType.TripleShot:
                        player.AddPowerup(PowerType.TripleShot);
                        break;
                    case PowerType.Shield:
                        player.AddPowerup(PowerType.Shield);
                        break;
                    case PowerType.Speed:
                        player.AddPowerup(PowerType.Speed);
                        break;
                }
                Destroy(this.gameObject);
            }
            
        }

    }
    #endregion

}
