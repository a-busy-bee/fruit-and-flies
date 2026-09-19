using UnityEngine;


public class Behavior : MonoBehaviour
{
    protected Vector3 target;

    virtual public Vector3 CalculateForce(Steering_Controller controller)
    {
        return Vector3.zero;
    }

    public void SetNewTarget(Vector3 newTarget)
    {
        target = newTarget;
    }
}