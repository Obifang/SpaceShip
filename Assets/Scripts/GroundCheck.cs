using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float GroundCheckDistance;
    public bool Enabled = true;

    private bool _grounded = true;

    public bool Grounded { get => _grounded;}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Enabled) {
            return;
        }   

        CheckGrounded();
    }

    public void CheckGrounded()
    {
        var groundLayer = 1 << LayerMask.NameToLayer("Ground");
        var centerHit = Physics.Raycast(transform.position, -transform.up, GroundCheckDistance, groundLayer);

        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundCheckDistance, transform.position.z), Color.red);

        _grounded = centerHit;
    }
}
