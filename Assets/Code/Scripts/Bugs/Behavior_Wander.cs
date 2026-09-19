using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
public class Behavior_Wander : Behavior
{
    float wanderAngle = 0.0f;
    float angleChange = 0.1f;
    override public Vector3 CalculateForce(Steering_Controller controller)
    {
        if (target == null) return Vector3.zero;

        // a circle directly in front of the insect
        // used to calculate displacement from target path
        Vector3 circleCenter;
        circleCenter = Vector3.Normalize(controller.GetVelocity());
        circleCenter.Scale(controller.GetBugWanderCircleDistance());

        // displacement force
        Vector3 displacement = new Vector3(0, -1, 0);
        float circleRadius = controller.GetBugWanderCircleRadius();
        displacement.Scale(new Vector3(0, circleRadius, 0));

        // change vector direction
        wanderAngle = Random.Range(0, Mathf.PI);
        float radians = wanderAngle * Mathf.Deg2Rad;
        displacement = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * displacement.magnitude;

        // wander isn't the same angle as prev frame
        wanderAngle += Random.Range(0.0f, 1.0f) * angleChange - angleChange * 0.5f;

        Vector3 wanderForce = circleCenter + displacement;
        return wanderForce;
    }

}
