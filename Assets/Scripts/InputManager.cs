using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction m_moveAction;
    private InputAction m_lookAction;
    private InputAction m_jumpAction;
    private InputAction m_sprintAction;
    private InputAction m_crouchAction;
    private InputAction m_interactAction;

    private Vector2 m_moveAmt;
    private Vector2 m_lookAmt;
    private Rigidbody m_rb;

    [Header("GameObject References")]
    public GameObject PlayerCamera;
    public GameObject playerBody;

    [Header("Mobility Settings")]
    public bool isSprinting = false;
    public bool isCrouching = false;
    public bool isCrawling = false;
    public float moveSpeed = 5;
    public float walkSpeed = 5;
    public float sprintSpeed = 10;
    public float crouchSpeed = 3;
    public float crawlSpeed = 1;
    public float rotateSpeed = 5;
    public float jumpSpeed = 5;
    public float jumpSpeedWalk = 3;
    public float jumpSpeedCrouch = 1;
    public float jumpSpeedCrawl = .25f;
    public Vector3 walkScale = new Vector3(1f, 1f, 1f);
    public Vector3 crouchScale = new Vector3(1f, .7f, 1f);
    public Vector3 crawlScale = new Vector3(1f, .3f, 2f);
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float minVerticalAngle = -90f;
    [SerializeField] private float maxVerticalAngle = 90f; 
    [SerializeField] private Vector3 previousScale;

    [Header("Interactables")]
    public GameObject EquippedObject;
    public bool canInteract;
    
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
        previousScale = playerBody.transform.localScale;
        m_moveAction = InputActions.FindAction("Player/Move");
        m_lookAction = InputActions.FindAction("Player/Look");
        m_jumpAction = InputActions.FindAction("Player/Jump");
        m_sprintAction = InputActions.FindAction("Player/Sprint");
        m_crouchAction = InputActions.FindAction("Player/Crouch");
        m_interactAction = InputActions.FindAction("Player/Interact");
        if (m_moveAction == null)
        {
            Debug.LogError("m_moveAction = null");
        }
        Cursor.visible = false;
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        isSprinting = m_sprintAction.IsPressed();
        m_moveAmt = m_moveAction.ReadValue<Vector2>();
        m_lookAmt = m_lookAction.ReadValue<Vector2>();
        Moving();
        Rotating();
        if (m_jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
        if (m_crouchAction.WasPressedThisFrame())
        {
            if (!isCrouching && !isCrawling)
            {
                //transition from walk to crawl
                isCrouching = true;
            }
            else if (!isCrawling && isCrouching)
            {
                // transition from crawl to crouch
                isCrawling = true;
                isCrouching = false;
            }
            else
            {
                isCrawling = false;
            }
            
        }
        if (m_interactAction.WasPressedThisFrame())
        {
            Interact();
        }
    }

    public void Jump()
    {
        m_rb.AddForceAtPosition(new Vector3(0, jumpSpeed, 0), Vector3.up, ForceMode.Impulse);
    }

    private void Moving()
    {
        if (isSprinting)
        {
            isCrouching = false;
            isCrawling = false;
            Sprint();
        }
        else if (isCrouching)
        {
            Crouch();
        }
        else if (isCrawling)
        {
            Crawl();
        }
        else
        {
            Walking();
        }
        m_rb.MovePosition(m_rb.position + transform.forward * m_moveAmt.y * moveSpeed * Time.deltaTime);
        m_rb.MovePosition(m_rb.position + transform.right * m_moveAmt.x * moveSpeed * Time.deltaTime);

    }

    private void Rotating()
    {

        float rotationx = m_lookAmt.x * rotateSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(0, rotationx, 0));

        

        float rotationy = Mathf.Clamp(m_lookAmt.y * rotateSpeed * Time.deltaTime, minVerticalAngle, maxVerticalAngle);
        PlayerCamera.transform.Rotate(-rotationy,0,0);

    }
    public void Sprint()
    {
        Debug.Log("sprinting");
        // sprinting animations will be added here

        moveSpeed = sprintSpeed;
    }


    public void Walking()
    {

        moveSpeed = walkSpeed;
        jumpSpeed = jumpSpeedWalk;
        Vector3 oldScale = playerBody.transform.localScale;
        playerBody.transform.localScale = walkScale;

        // Snap to ground: move up by half the difference in height
        float heightDiff = (walkScale.y - oldScale.y) * playerBody.GetComponent<Collider>().bounds.size.y / oldScale.y;
        playerBody.transform.position += new Vector3(0, heightDiff / 2f, 0);

        previousScale = walkScale;

    }

    public void Crouch()
    {
        moveSpeed = crouchSpeed;
        jumpSpeed = jumpSpeedCrouch;
        Vector3 oldScale = playerBody.transform.localScale;
        playerBody.transform.localScale = crouchScale;

        // Snap to ground: move down by half the difference in height
        float heightDiff = (oldScale.y - crouchScale.y) * playerBody.GetComponent<Collider>().bounds.size.y / oldScale.y;
        playerBody.transform.position -= new Vector3(0, heightDiff / 2f, 0);

        previousScale = crouchScale;
    }

    public void Crawl()
    {
        moveSpeed = crawlSpeed;
        jumpSpeed = jumpSpeedCrawl;
        Vector3 oldScale = playerBody.transform.localScale;
        playerBody.transform.localScale = crawlScale;

        // Snap to ground: move down by half the difference in height
        float heightDiff = (oldScale.y - crawlScale.y) * playerBody.GetComponent<Collider>().bounds.size.y / oldScale.y;
        playerBody.transform.position -= new Vector3(0, heightDiff / 2f, 0);

        previousScale = crawlScale;
    }

    public void Interact()
    {
        if (canInteract)
        {
            Debug.Log("cube has been interacted with.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("interactable") && !canInteract)
        {
            Debug.Log("press e to interact.");
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("interactable") && canInteract)
        {
            Debug.Log("you have left the interactable zone.");
            canInteract = false;
        }
    }
}
