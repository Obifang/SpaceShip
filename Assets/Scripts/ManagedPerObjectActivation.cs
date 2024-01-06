using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagedPerObjectActivation : MonoBehaviour
{
    private PerObjectGravity _body;
    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponentInParent<PerObjectGravity>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        _body.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _body.enabled = false;
        other.TryGetComponent<ConstantForce>(out ConstantForce constantForce);
        if (constantForce != null) {
            constantForce.force = Vector3.zero;
        }
        
    }
}
