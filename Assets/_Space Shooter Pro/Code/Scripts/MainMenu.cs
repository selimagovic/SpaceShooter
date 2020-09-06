using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private static MainMenu _instance;
    #region Variables
    [SerializeField]
    private int _sceneIndex = 0;
    [SerializeField]
    private GameObject _PauseMenu = null;
    private bool _isGamePaused = false;

    public static MainMenu Instance { get => _instance;}
    #endregion
    #region Builtin Methods
    private void Awake()
    {
        _instance = this;
    }
    #endregion
    #region --Public Custom Methods--
    public void LoadLevel()
    {
        SceneManager.LoadScene(_sceneIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        _isGamePaused = !_isGamePaused;
        if (_isGamePaused == true)
        {
            _PauseMenu.SetActive(true);
            Time.timeScale = 0; 
        }
        else
        {
            _PauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
    #endregion

}
