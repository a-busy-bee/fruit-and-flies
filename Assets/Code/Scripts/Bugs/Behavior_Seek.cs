using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Behavior_Seek : Behavior
{
    override public Vector3 CalculateForce(Steering_Controller controller)
    {
        if (target == null) return Vector3.zero;

        float slowingRadius = controller.GetBugSlowingRadius();
        float maxVelocity = controller.GetBugMaxVelocity();

        // get distance and desiredVelocity
        Vector3 desiredVelocity = target - transform.position;
        float distance = desiredVelocity.magnitude;
        desiredVelocity = Vector3.Normalize(desiredVelocity);

        if (distance < slowingRadius) // if it's within the slowing radius, gradually slow down 
        {
            desiredVelocity = desiredVelocity * maxVelocity * (distance / slowingRadius);
        }
        else
        {
            desiredVelocity = desiredVelocity * maxVelocity;
        }

        return desiredVelocity - controller.GetVelocity(); // return steering
    }
}
