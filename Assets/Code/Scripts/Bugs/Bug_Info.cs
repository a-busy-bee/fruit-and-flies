using UnityEngine;

[CreateAssetMenu(fileName = "Bug_Info", menuName = "Scriptable Objects/Bug_Info")]
public class Bug_Info : ScriptableObject
{
	public bool causesDamage;
	public float initialDamage;
	public float continuousDamage; // while the insect remains touching the object
}
