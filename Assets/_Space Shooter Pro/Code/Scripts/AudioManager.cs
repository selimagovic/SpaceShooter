using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    #region Variables
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Audio Manager Instance is NULL!!!!");
            return _instance;
        }
    }
    [Header("Options Properties")]
    public Button musicBTN;
    public Sprite musicSprite;


    [SerializeField]
    private AudioClip _musicClip = null;

    private AudioSource _aSource;

    private int currentSample;
    private Sprite currentSprite;
    private bool soundToggle = true;
    #endregion
    #region Builtin Methods
    private void Awake()
    {
        _instance = this;
        _aSource = GetComponent<AudioSource>();
        if (_aSource == null)
            Debug.LogError("Audio Source is NULL!!!!" + "on this " + gameObject.name + " gameobject");
        _aSource.clip = _musicClip;
        _aSource.Play();
        CheckPublicFields();
        currentSprite = musicBTN.image.sprite;
        soundToggle = true;
        TurnMusicOnOFF();
    }
    #endregion
    #region --Public Custom Methods--
    public void TurnMusicOnOFF()
    {
        soundToggle = !soundToggle;
        if (soundToggle)
        {
            currentSample = _aSource.timeSamples;
            musicBTN.image.sprite = musicSprite;
            _aSource.Stop();
        }
        else
        {
            _aSource.timeSamples = currentSample;
            musicBTN.image.sprite = currentSprite;
            _aSource.Play();
        }
    }
    #endregion
    #region --Private Custom Methods--
    private void CheckPublicFields()
    {
        if (musicBTN == null)
        {
            Debug.LogError("Please Add music button to apropriate field.");
        }
        if (musicSprite == null)
        {
            Debug.LogError("Please Add Sprite button to apropriate field.");
        }
    }
    #endregion

}