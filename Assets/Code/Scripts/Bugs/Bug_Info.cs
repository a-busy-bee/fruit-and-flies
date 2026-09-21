using UnityEngine;

[CreateAssetMenu(fileName = "Bug_Info", menuName = "Scriptable Objects/Bug_Info")]
public class Bug_Info : ScriptableObject
{
	[Header("Damage")]
	public bool causesDamage;
	public float initialDamage;
	public float continuousDamage; // while the insect remains touching the object
	public float damageDebuff; // how much the insect accelerates rot

	[Header("Movement")]
	public float maxVelocity;
	public float slowingRadius;
	public Vector3 wanderCircleDistance;
	public float wanderCircleRadius;
	public float maxForce;
	public float mass;
	
}
