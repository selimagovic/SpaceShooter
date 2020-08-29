using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    #region Variables
    [SerializeField]
    private List<GameObject> _enemyPrefabs = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _powerPrefabs = new List<GameObject>();
    [SerializeField]
    private GameObject _enemyGRP=null;
    [SerializeField]
    private GameObject _powerupGRP = null;
    [SerializeField]
    private Vector2 _randomSpawningPositionX = new Vector2();
    [SerializeField]
    [Range(2.0f,10.0f)]
    private float _waitEnemySeconds=5.0f;
    [SerializeField]
    [Range(2.0f, 10.0f)]
    private float _waitPowerupSeconds = 5.0f;
    private bool _stopSpawning = false;

    public static SpawnManager Instance { get => _instance; }
    #endregion
    #region Builtin Methods
    private void Awake()
    {
        _instance = this;
    }
    #endregion
    #region --Public Custom Methods--
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
    #endregion
    #region --Private Custom Methods--
    IEnumerator SpawnEnemy()
    {
        //while loop
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(_randomSpawningPositionX.x,
                _randomSpawningPositionX.y), 7, 0);
            GameObject newEnemy= Instantiate(_enemyPrefabs[Random.Range(0,_enemyPrefabs.Count)], posToSpawn,Quaternion.identity);
            newEnemy.transform.SetParent(_enemyGRP.transform);
            yield return new WaitForSeconds(_waitEnemySeconds);

        }
    }
    IEnumerator SpawnPowerup()
    {
        //while loop
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(_randomSpawningPositionX.x,
                _randomSpawningPositionX.y), 7, 0);
            GameObject newPowerup = Instantiate(_powerPrefabs[Random.Range(0, _powerPrefabs.Count)], posToSpawn, Quaternion.identity);
            newPowerup.transform.SetParent(_powerupGRP.transform);
            yield return new WaitForSeconds(_waitPowerupSeconds);

        }
    }
    #endregion

}
