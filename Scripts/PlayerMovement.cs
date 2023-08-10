using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float forceMagnitute;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float rotationSpeed;
    private Camera mainCamera;
    private Rigidbody rb;
    private Vector3 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();

        KeepPlayerOnScreen();

        RotateToFaceVelocity();
    }

    void FixedUpdate()
    {
        if(movementDirection == Vector3.zero) { return; }

        rb.AddForce(movementDirection * forceMagnitute * Time.deltaTime, ForceMode.Force);
        
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    private void ProcessInput()
    {
         if(Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
            
            movementDirection = transform.position - worldPosition;
            movementDirection.z = 0f;
            movementDirection.Normalize(); 
        }
        else
        {
            movementDirection = Vector3.zero;
        }
    }

    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position;
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if(viewportPosition.x>1)
        {
            newPosition.x = -newPosition.x + 0.01f;
        }
        else if(viewportPosition.x<0)
        {
            newPosition.x = -newPosition.x + 0.01f;
        }
        else if (viewportPosition.y>1)
        {
            newPosition.y = -newPosition.y + 0.01f;
        }
        else if (viewportPosition.y<0)
        { 
            newPosition.y = -newPosition.y + 0.01f;
        }

        transform.position = newPosition;
    }
    private void RotateToFaceVelocity()
    {
        if(rb.velocity == Vector3.zero) {return;}
        
        Quaternion targetRotation =  Quaternion.LookRotation(rb.velocity, Vector3.back);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
}
