using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlanetaryBody : MonoBehaviour
{
    [Header("General")]
    public Vector3 InitialVelocity;
    public float CalculationTimeInterval;
    public bool EnableGravityCalculations;
    public List<PlanetaryBody> OtherPlanetaryBodies;
    [Header("Debug")]
    public bool EnableOrbitPathDebug;
    public Color DebugOribitColor;

    public Vector3 OriginalPosition { get => _originalPosition; set => _originalPosition = value; }
    public Vector3 NewPosition { get => _newPosition; set => _newPosition = value; }
    public Vector3 Velocity { get => _velocity; set => _velocity = value; }
    public Vector3 Acceleration { get => _acceleration; set => _acceleration = value; }

    private Vector3 _originalPosition;
    private Vector3 _newPosition;
    private Vector3 _velocity;
    private Vector3 _acceleration;

    private PlanetaryBody _debugBody;

    private Rigidbody _bodyRB;

    public float Mass { get => _bodyRB.mass;}

    // Start is called before the first frame update
    void Start()
    {
        _bodyRB = GetComponent<Rigidbody>();
        _velocity = InitialVelocity;
    }

    void FixedUpdate()
    {
        if (!EnableGravityCalculations) {
            return;
        }

        foreach(PlanetaryBody p in OtherPlanetaryBodies) {
            if (p.Mass < Mass) {
                HandleOrbitalCalcs(p, this);
                _debugBody = this;
            }
        }
    }

    /// <summary>
    /// Used to calculate the movement of 2 planetary bodies
    /// </summary>
    /// <param name="smallerBody"></param>
    /// <param name="largerBody"></param>
    public void HandleOrbitalCalcs(PlanetaryBody smallerBody, PlanetaryBody largerBody)
    {
        float time = CalculationTimeInterval;

        smallerBody.OriginalPosition = smallerBody.transform.position;
        smallerBody.Acceleration = ApplyNewtonianGravityScaleToObject(smallerBody, largerBody) / smallerBody.Mass;
        smallerBody.transform.position += (smallerBody.Velocity * time + 0.5f * smallerBody.Acceleration * (time * time));
        smallerBody.NewPosition = smallerBody.transform.position;
        smallerBody.Velocity = (smallerBody.NewPosition - smallerBody.OriginalPosition) / time;
    }

    /// <summary>
    /// Used to calculate force between 2 planetary bodies
    /// </summary>
    /// <param name="smallerBody"></param>
    /// <param name="largerBody"></param>
    /// <returns></returns>
    public Vector3 ApplyNewtonianGravityScaleToObject(PlanetaryBody smallerBody, PlanetaryBody largerBody)
    {
        var relativeDirection = (largerBody.transform.position - smallerBody.transform.position).normalized;
        float distance = Vector3.Distance(largerBody.transform.position, smallerBody.transform.position);
        float distanceSqd = distance * distance;
        float G = 6.67f * Mathf.Pow(10, -11);
        float force = G * largerBody.Mass * smallerBody.Mass / distanceSqd;
        return (force * relativeDirection);
    }

    private void OnDrawGizmos()
    {
        if (!EnableOrbitPathDebug || _bodyRB == null)
            return;

        Debug.DrawLine(OriginalPosition, NewPosition, DebugOribitColor, 500f);
        string text = "Debug Menu\nCurrent Velocity (SqrM): " + Velocity.sqrMagnitude + "\nCurrent Mass: " + Mass + "\nCurrent Acceleration (SqrM): " + Acceleration.sqrMagnitude +
            "\nCurrent Force (SqrM): " + (Acceleration * Mass).sqrMagnitude;
        Handles.Label(transform.position, text);
    }
}
