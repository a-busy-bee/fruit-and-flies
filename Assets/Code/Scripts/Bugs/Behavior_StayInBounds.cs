using UnityEngine;

public class Behavior_StayInBounds : Behavior
{
    private Vector3 boundsCenter = Vector3.zero;
    private float boundsRadius = 20f;

    override public Vector3 CalculateForce(Steering_Controller controller)
    {
        Vector3 position = transform.position;
        float distFromCenter = Vector3.Distance(position, boundsCenter);

        if (distFromCenter >= boundsRadius)
        {
            Vector3 dirToCenter = (boundsCenter - position).normalized;
            return dirToCenter * controller.GetBugMaxVelocity();
        }

        return Vector3.zero;
    }
}
