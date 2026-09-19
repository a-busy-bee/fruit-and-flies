using UnityEngine;

public class Behavior_Flee : Behavior
{
    override public Vector3 CalculateForce(Steering_Controller controller)
    {
        if (target == null) return Vector3.zero;

        // fleeing vector is the vector orthogonal to the target vector
        Vector3 desiredVelocity = Vector3.Normalize(transform.position - target) * controller.GetBugMaxVelocity();

        return desiredVelocity - controller.GetVelocity();
    }
}
