using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasVelocity
{
    public Vector3 GetInitialVelocity();
    public Vector3 GetCurrentVelocity();
    public Vector3 GetAcceleration();
}
