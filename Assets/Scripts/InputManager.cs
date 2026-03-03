using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerControls controls;
    //PlayerControls.GroundMovementActions.groundMovement;

    private void Awake()
    {
        controls = new PlayerControls();


    }
}
