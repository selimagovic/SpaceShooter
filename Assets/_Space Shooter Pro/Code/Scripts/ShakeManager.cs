using UnityEngine;

/// <summary>
/// This Class code is found on Internet and Modified to fit my purpose
/// ShakeManager class create camera shake on enemy or laser from enemy hit. 
/// </summary>
public class ShakeManager : MonoBehaviour
{
    private static ShakeManager _instance;
    #region Variables
    [SerializeField]
    private Camera _mainCamera = null;
    [SerializeField]
    private float _shakeMagnitude = 0.005f;
    [SerializeField]
    private float _shakeTime = 0.5f;

    private Vector3 _currentCameraPosition;

    public static ShakeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("CameraShaker instance is null!!!");
            }
            return _instance;
        }
    }
    #endregion
    #region Builtin Methods
    private void Awake()
    {
        _instance = this;
    }
    #endregion
    #region --Public Custom Methods--
    public void ShakeCamera()
    {
        _currentCameraPosition = _mainCamera.transform.position;
        InvokeRepeating("StartShaking", 0f, 0.005f);
        Invoke("StopShaking", _shakeTime);
    }
    #endregion
    #region --Private Custom Methods--
    void StartShaking()
    {
        float shakingOffsetX = Random.value * _shakeMagnitude * 2f - _shakeMagnitude;
        float shakingOffsetY = Random.value * _shakeMagnitude * 2f - _shakeMagnitude;
        Vector3 currentShakingPosition = _mainCamera.transform.position;
        currentShakingPosition.x += shakingOffsetX;
        currentShakingPosition.y += shakingOffsetY;
        _mainCamera.transform.position = currentShakingPosition;

    }
    void StopShaking()
    {
        CancelInvoke("StartShaking");
        _mainCamera.transform.position = _currentCameraPosition;
    }
    #endregion
}
