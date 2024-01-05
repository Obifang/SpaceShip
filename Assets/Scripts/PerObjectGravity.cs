using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerObjectGravity : MonoBehaviour
{
    public float ObjectGravity = 9.8f;
    public float StabaliseValue;
    private ConstantForce _ship;
    private Rigidbody _shipRB;
    private Vector3 ColliderMax;
    // Start is called before the first frame update
    void Start()
    {
        _ship = FindAnyObjectByType<ShipMovement>().GetComponent<ConstantForce>();
        _shipRB = _ship.GetComponent<Rigidbody>();
        ColliderMax = GetComponent<Collider>().bounds.max;
        ApplyGravityScaleToObject(_ship);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyGravityScaleToObject(_ship);
    }

    public void ApplyGravityScaleToObject(ConstantForce constantForce)
    {
        var relativeDirection = (transform.position - constantForce.transform.position).normalized;
        var forceTransform = constantForce.transform;
        //forceTransform.rotation = Quaternion.FromToRotation(forceTransform.up, relativeDirection) * forceTransform.rotation;
        var goalRotation = Quaternion.FromToRotation(-forceTransform.up, relativeDirection) * forceTransform.rotation;
        //forceTransform.rotation = Quaternion.Slerp(forceTransform.rotation, Quaternion.RotateTowards(forceTransform.rotation, goalRotation, StabaliseValue * Time.fixedDeltaTime), StabaliseValue);
        forceTransform.rotation = Quaternion.Slerp(forceTransform.rotation, goalRotation, StabaliseValue * Time.fixedDeltaTime);
        constantForce.force = relativeDirection * _shipRB.mass * ObjectGravity;
    }
}
