using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    private static GameManager _instance;

    private bool _isGameOver = false;

    private PlayerControls _playerControls;
    InputAction _onRestartAction;

    public static GameManager Instance
    {
        get
        {
            if(_instance==null)
            {
                Debug.LogError("GameManager instance is NULL!!!");
            }
            return _instance;
        }
    }
    #endregion
    #region Builtin Methods
    private void Awake()
    {
        _instance = this;
        _playerControls = new PlayerControls();
        _onRestartAction = _playerControls.Player.Restart;         
    }
    private void OnEnable()
    {
        _playerControls.Enable();
        _onRestartAction.Enable();
    }
    private void OnDisable()
    {
        _playerControls.Disable();
        _onRestartAction.Disable();
    }
    private void Update()
    {
        if(_isGameOver && _onRestartAction.triggered)
        {
            Restart();
        }
    }
    #endregion
    #region --Public Custom Methods--
    public void GameOver()
    {
        _isGameOver = true;
    }
    #endregion
    #region --Private Custom Methods--

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);//current game Scene
    }
    #endregion

}
