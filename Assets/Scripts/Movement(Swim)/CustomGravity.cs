using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //special condition as Rigidbody is important for this to realistically work
public class CustomGravity : MonoBehaviour
{
    //custom gravity as physics change when player is underwater vs player just on ground or normal surface
    [SerializeField] private float gravityScale;
    private const float globalGravity = -9.81f;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
    }

    private void FixedUpdate() //Main Function
    {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rigidbody.AddForce(gravity, ForceMode.Acceleration);
    }
}
