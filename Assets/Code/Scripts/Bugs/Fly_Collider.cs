using UnityEngine;

public class Fly_Collider : MonoBehaviour
{
	[SerializeField] private Bug_Manager bugManager;
	[SerializeField] private Steering_Controller steeringController;
	[SerializeField] private LayerMask layerMask;
	private float raycastDistance = 5;
	private void Update()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position + transform.forward, raycastDistance, layerMask);

		if (hit.collider != null)
		{
			Debug.Log("hit: " + hit.collider.name);

			if (steeringController.CheckTargetReached())
			{
				// send damage to obj if rottable
				if (hit.collider.GetComponent<Object_Manager_Rottable>() != null)
				{
					hit.collider.GetComponent<Object_Manager_Rottable>().ContactDamage(bugManager.GetBugInfo());
				}
				
				// land the fly (landing check done within Land() function)
				bugManager.Land();
			}
		}
	}

	// void OnDrawGizmosSelected()
	// {
	// 	Gizmos.color = Color.cyan;
	// 	Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10f);
	// }
}