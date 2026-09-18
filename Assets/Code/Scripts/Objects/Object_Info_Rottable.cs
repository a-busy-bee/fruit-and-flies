using UnityEngine;

[CreateAssetMenu(fileName = "Object_Info_Rottable", menuName = "Scriptable Objects/Object_Info_Rottable")]
public class Object_Info_Rottable : Object_Info_Base
{
	public AudioManager.SoundType soundTypeRotted;

	[Header("Health")]
	public float maxHealth;

	[Header("Rot")]
	public AnimationClip transitionToMold;
	public AnimationClip transitionToRot;
	public AnimationClip transitionToGone; // scope creep?
}
