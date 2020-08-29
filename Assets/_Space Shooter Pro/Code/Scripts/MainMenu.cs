using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    #region Variables
    [SerializeField]
    private int _sceneIndex = 0;

    private bool _isGamePaused = false;
    #endregion
    #region Builtin Methods
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
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

    }
    #endregion
    #region --Private Custom Methods--

    #endregion

}
