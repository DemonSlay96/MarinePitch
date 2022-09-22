using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SwimmingController : MonoBehaviour
{
    //Serializing as it helps me to tweak values during production phase 
    [SerializeField] private float swimmingForce;
    [SerializeField] private float resistanceForce;
    [SerializeField] private float deadZone;
    [SerializeField] private Transform trackingSpace;

    private new Rigidbody rigidbody; 
    private Vector3 currentDirection;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() 
    //function to make sure we move using trigger buttons of both controllers hence we have and operator to check that condition
    {
        bool rightButtonPressed = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        bool leftButtonPressed = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);

        if (rightButtonPressed && leftButtonPressed)
        {
            Vector3 leftHandDirection = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
            Vector3 rightHandDirection = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
            Vector3 localVelocity = leftHandDirection + rightHandDirection;
            localVelocity *= -1f;
            //localveloctiy is referred to general swimming physics and resistance force as well
            if (localVelocity.sqrMagnitude > deadZone * deadZone)
            {
                AddSwimmingForce(localVelocity);
            }
        }
        ApplyReststanceForce();
    }

    private void ApplyReststanceForce()
    {
        if (rigidbody.velocity.sqrMagnitude > 0.01f && currentDirection != Vector3.zero)
        {
            rigidbody.AddForce(-rigidbody.velocity * resistanceForce, ForceMode.Acceleration);
        }
        else
        {
            currentDirection = Vector3.zero;
        }
    }

    private void AddSwimmingForce(Vector3 localVelocity) 
        //this function adds swimming force based on values provided or tweaked
        //we can tweak and make our swimming experience more smooth or difficult based on values we provide
    {
        Vector3 worldSpaceVelocity = trackingSpace.TransformDirection(localVelocity);
        rigidbody.AddForce(worldSpaceVelocity * swimmingForce, ForceMode.Acceleration);
        currentDirection = worldSpaceVelocity.normalized;
    }
}
