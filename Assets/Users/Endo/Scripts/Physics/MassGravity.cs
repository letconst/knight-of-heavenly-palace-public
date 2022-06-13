using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MassGravity : MonoBehaviour
{
    private Rigidbody _selfRig;

    private void Start()
    {
        _selfRig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_selfRig)
        {
            _selfRig.AddForce(Physics.gravity * _selfRig.mass);
        }
    }
}
