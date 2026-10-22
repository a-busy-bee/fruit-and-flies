using UnityEngine;
using UnityEngine.InputSystem;

public class Behavior_Flee : Behavior
{
    private float MOUSE_DIST_THRESHOLD = 10;
    override public Vector3 CalculateForce(Steering_Controller controller)
    {
        target = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (Vector2.Distance((Vector2)target, (Vector2)transform.position) > MOUSE_DIST_THRESHOLD)
        {
            return Vector3.zero;
        }

        // fleeing vector is the vector orthogonal to the target vector
        Vector3 desiredVelocity = Vector3.Normalize(transform.position - target) * controller.GetBugMaxVelocity();

        float maxFleeForce = GetComponent<Bug_Manager>().GetBugInfo().maxFleeForce;
        return Vector3.ClampMagnitude(desiredVelocity - controller.GetVelocity(), maxFleeForce);
    }
}
