using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{

    #region Variables
    PlayerControls _inputActions;

    private Vector2 _move;
    private float _shoot;

    public Vector2 Move { get => _move;}
    public float Shoot { get => _shoot; }

    #endregion
    #region Builtin Methods

    #endregion
    #region --Public Custom Methods--
    public void OnMovement(InputValue input)
    {
        _move = input.Get<Vector2>();
    }
    public void OnShooting(InputValue input)
    {
        _shoot = input.Get<float>();
    }
    #endregion
    #region --Private Custom Methods--

    #endregion
}
