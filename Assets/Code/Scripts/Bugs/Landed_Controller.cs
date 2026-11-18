using UnityEngine;
using System.Collections;


public class Landed_Controller : MonoBehaviour
{
    private const float ROT_ANGLE_MAX = 180f;
	private const float ROT_ANGLE_MIN = -180f;
    private const float ROT_TIME = 0.05f;
    private bool isRotating = false;
    private float currVelocity;
    private float currTargetAngle;

    public void StartLandedBehaviors()
	{
		StartCoroutine(LandedBehavior());
	}

	private IEnumerator LandedBehavior()
	{
		int cycles = Random.Range(4, 7);

		for (int i = 0; i < cycles; i++)
		{
			isRotating = true;
            currTargetAngle = Random.Range(ROT_ANGLE_MIN, ROT_ANGLE_MAX);

            yield return new WaitForSeconds(Random.Range(0f, 1f));
		}

        GetComponent<Bug_Manager>().Fly();
        transform.rotation = Quaternion.identity;
	}

    private void Update()
    {
        if (isRotating)
        {
            // get curr angle
            float angle = transform.eulerAngles.z;

            // lerp using smoothlerp
            angle = Mathf.SmoothDampAngle(angle, currTargetAngle, ref currVelocity, ROT_TIME);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // check if at target angle
            float deltaAngle = Mathf.DeltaAngle(angle, currTargetAngle);
            if (Mathf.Abs(deltaAngle) < 0.001f)
            {
                isRotating = false;
            }
        }
    }
}
