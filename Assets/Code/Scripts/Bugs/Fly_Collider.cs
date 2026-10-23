using UnityEngine;

public class Fly_Collider : MonoBehaviour
{

	public void OnTriggerEnter(Collider collision)
	{
		Debug.Log("ouchie");
		if (collision.gameObject.TryGetComponent<Object_Manager_Rottable>(out Object_Manager_Rottable objManager))
        {
			Debug.Log("ouchie more");
            transform.parent.gameObject.GetComponent<Bug_Manager>().CheckDamage(objManager.gameObject);
        }
	}
}
