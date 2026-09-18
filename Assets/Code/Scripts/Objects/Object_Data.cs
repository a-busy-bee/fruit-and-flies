using UnityEngine;

[CreateAssetMenu(fileName = "Object_Info", menuName = "Scriptable Objects/Object_Info")]
public class Object_Info : ScriptableObject
{
	public AudioManager.SoundType soundTypeRotted;
	public AudioManager.SoundType soundTypeNotRotted;

	public bool immune;
	public float maxHealth;
}
