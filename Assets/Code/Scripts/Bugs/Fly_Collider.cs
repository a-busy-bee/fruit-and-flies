using UnityEngine;

public class Fly_Collider : MonoBehaviour
{

	public void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<Object_Manager_Rottable>())
        {
            transform.parent.gameObject.GetComponent<Bug_Manager>().CheckDamage(collision.gameObject);
        }
	}
}
