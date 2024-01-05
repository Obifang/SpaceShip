using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ShipMovement : MonoBehaviour
{
    public PlayerInputActions PlayerControls;
    public float BaseThrustSpeed;
    public float RotationSpeed = 10f;
    public float ForwardRotationSpeed = 1f;
    public float GroundCheckDistance = 5f;
    public GroundCheck FrontLeftLandingGear;
    public GroundCheck FrontRightLandingGear;
    public GroundCheck BackLeftLandingGear;
    public GroundCheck BackRightLandingGear;

    private Rigidbody _rb;
    private Collider _collider;
    private bool _grounded = true;
    private bool _isFlying = false;
    private Vector3 _thrustDirection;
    private float _rotateDirection;
    private Vector2 _lookDirection;
    private InputAction _rotate;
    private InputAction _move;
    private InputAction _look;

    private void Awake()
    {
        PlayerControls = new PlayerInputActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _thrustDirection = _move.ReadValue<Vector3>();
        _rotateDirection = _rotate.ReadValue<float>();
        _lookDirection = _look.ReadValue<Vector2>();

        if (FrontLeftLandingGear.Grounded && FrontRightLandingGear.Grounded && BackLeftLandingGear.Grounded && BackRightLandingGear.Grounded) {
            return;
        }

        var mouseAxis = new Vector3(-_lookDirection.y, _lookDirection.x, _rotateDirection * ForwardRotationSpeed) * RotationSpeed * Time.deltaTime;
        transform.Rotate(mouseAxis);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        /*if (Input.GetKey(KeyCode.Space)) {
            forceToAdd += transform.up * BaseThrustSpeed;
        }

        if (Input.GetKey(KeyCode.D)) {
            forceToAdd += transform.right * BaseThrustSpeed;
        }

        if (Input.GetKey(KeyCode.A)) {
            forceToAdd += -transform.right * BaseThrustSpeed;
        }

        if (Input.GetKey(KeyCode.W)) {
            forceToAdd += transform.forward * BaseThrustSpeed;
        }

        if (Input.GetKey(KeyCode.S)) {
            forceToAdd += -transform.forward * BaseThrustSpeed;
        }*/

        _rb.AddRelativeForce(_thrustDirection * BaseThrustSpeed);
    }

    public void FireThruster(InputAction.CallbackContext context)
    {

    }

    public void CheckGrounded()
    {
        var groundLayer = 1 << LayerMask.NameToLayer("Ground");
        var halfWidth = _collider.bounds.size.x * 0.5f;
        var center = _collider.bounds.center;
        var left = new Vector3(center.x - halfWidth, center.y, center.z);
        var right = new Vector3(center.x + halfWidth, center.y, center.z);
        var leftHit = Physics.Raycast(left, -transform.up, GroundCheckDistance, groundLayer);
        var rightHit = Physics.Raycast(right, -transform.up, GroundCheckDistance, groundLayer);
        var centerHit = Physics.Raycast(center, -transform.up, GroundCheckDistance, groundLayer);

        Debug.DrawLine(left, new Vector3(left.x, left.y - GroundCheckDistance, left.z), Color.red);
        Debug.DrawLine(center, new Vector3(center.x, center.y - GroundCheckDistance, center.z), Color.red);
        Debug.DrawLine(right, new Vector3(right.x, right.y - GroundCheckDistance, right.z), Color.red);

        _grounded = (leftHit &&  rightHit && centerHit);
    }

    private void OnEnable()
    {
        _rotate = PlayerControls.Player.Rotate;
        _move = PlayerControls.Player.Move;
        _look = PlayerControls.Player.Look;
        _move.Enable();
        _rotate.Enable();
        _look.Enable();
    }

    private void OnDisable()
    {
        _move.Disable();
        _rotate.Disable();
        _look.Disable();
    }
}
