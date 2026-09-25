using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using System.Numerics;

using Vector3 = UnityEngine.Vector3;
using Random = UnityEngine.Random;

[Serializable]
public class BehaviorWeightPair
{
	public Behavior behavior;
	public float weight;
}
public class Steering_Controller : MonoBehaviour
{
	[SerializeField] private BehaviorWeightPair[] behaviors;
	private Bug_Info bugInfo;
	private Vector3 velocity = Vector3.zero;
	private GameObject currTargetObj;

	private void Start()
	{
		bugInfo = GetComponent<Bug_Manager>().GetBugInfo();

		currTargetObj = LevelManager.instance.GetRandomObject();
		Vector3 targetPos = currTargetObj.GetComponent<Object_Manager_Base>().GetRandomPointOnObj();
		foreach (BehaviorWeightPair behavior in behaviors)
		{
			behavior.behavior.SetNewTarget(targetPos);
		}

		Debug.Log(targetPos);
	}

	public void TargetReached()
	{
		// high chance to stay on this object, but choose diff location
		float chanceToStay = Random.Range(0.0f, 1.0f);

		// if roll is < 0.4, get new object
		if (chanceToStay < 0.4f) currTargetObj = LevelManager.instance.GetRandomObject();

		// choose new target pos
		Vector3 targetPos = currTargetObj.GetComponent<Object_Manager_Base>().GetRandomPointOnObj();

		// if 0.2 < roll < 0.4, choose target in the sky instead
		if (chanceToStay > 0.2 && chanceToStay < 0.4)
		{
			targetPos = LevelManager.instance.GetRandomPointInAir();
			currTargetObj = null;
		}

		foreach (BehaviorWeightPair behavior in behaviors)
		{
			behavior.behavior.SetNewTarget(targetPos);
		}
	}

	private void Update()
	{
		Vector3 totalSteeringForce = Vector3.zero;

		// add up all of the behavior steering forces
		foreach (BehaviorWeightPair behavior in behaviors)
		{
			totalSteeringForce += behavior.behavior.CalculateForce(this) * behavior.weight;
		}

		// clamp so they don't go crazy
		totalSteeringForce = Vector3.ClampMagnitude(totalSteeringForce, bugInfo.maxForce);

		// math 
		Vector3 acceleration = totalSteeringForce / bugInfo.mass;
		velocity += acceleration * Time.deltaTime;
		velocity = Vector3.ClampMagnitude(velocity, bugInfo.maxVelocity);

		// update position
		Vector3 newPos = transform.position + velocity * Time.deltaTime;
		transform.position = new Vector3(newPos.x, newPos.y, -5);

		// check if position is close enough to the target
		
		
	}

	public float GetBugMaxVelocity()
	{
		return bugInfo.maxVelocity;
	}
	public float GetBugSlowingRadius()
	{
		return bugInfo.slowingRadius;
	}
	public Vector3 GetBugWanderCircleDistance()
	{
		return bugInfo.wanderCircleDistance;
	}
	public float GetBugWanderCircleRadius()
	{
		return bugInfo.wanderCircleRadius;
	}
	public Vector3 GetVelocity()
	{
		return velocity;
	}
	
}
