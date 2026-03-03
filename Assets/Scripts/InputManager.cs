using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction m_moveAction;
    private InputAction m_lookAction;
    private InputAction m_jumpAction;

    private Vector2 m_moveAmt;
    private Vector2 m_lookAmt;
    private Rigidbody m_rb;

    public float WalkSpeed = 5;
    public float RotateSpeed = 5;
    public float JumpSpeed = 5;
    //PlayerControls.GroundMovementActions.groundMovement;

 private void OnEnable()
    {
        InputActions.FindActionMap("Player");
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player");
    }

    private void Awake()
    {
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_lookAction = InputSystem.actions.FindAction("Look");
        m_jumpAction = InputSystem.actions.FindAction("Jump");

        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        m_moveAmt = m_moveAction.ReadValue<Vector2>();
        m_lookAmt = m_lookAction.ReadValue<Vector2>();

        if (m_jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Walking();
        Rotating();
    }

    public void Jump()
    {
        m_rb.AddForceAtPosition(new Vector3(0, 5f, 0), Vector3.up, ForceMode.Impulse);
    }

    private void Walking()
    {
        m_rb.MovePosition(m_rb.position + transform.forward * m_moveAmt.y * WalkSpeed * Time.deltaTime);
    }

    private void Rotating()
    {
        if (m_moveAmt.y  != 0)
        {
            float rotation = m_lookAmt.x * RotateSpeed * Time.deltaTime;
            Quaternion deltaRotation = Quaternion.Euler(0, rotation, 0);
            m_rb.MoveRotation(m_rb.rotation * deltaRotation);
        }
    }
}
