using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{

    #region Variables
    [SerializeField]
    private float _rotateSpeed=5f;
    [SerializeField]
    private GameObject _explosionPrefab=null;
    #endregion
    #region Builtin Methods
    private void Update()
    {
        //roatate asteroid by given speed
        transform.Rotate(Vector3.forward* _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Instantiate(_explosionPrefab,transform.position,Quaternion.identity);
            
            Destroy(other.gameObject);
            SpawnManager.Instance.StartSpawning();
            Destroy(this.gameObject,0.25f);
        }
    }
    #endregion
}
