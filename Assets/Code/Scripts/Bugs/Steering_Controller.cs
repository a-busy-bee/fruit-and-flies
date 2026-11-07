using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using System.Numerics;

using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
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
	[SerializeField] private Rigidbody2D rigidbody;
	private const float TARGET_DIST_THRESHOLD = 5;
	private const float ROT_FORCE_MIN = 10f;
	private const float ROT_FORCE_MAX = 15f;
	private List<float> behaviorWeights = new List<float>(); 
	private Bug_Info bugInfo;
	private Vector3 velocity = Vector3.zero;
	private GameObject currTargetObj;
	private Vector3 currTargetPos;
	private bool targetReached = false;
	private bool pauseBehaviors = false;

	private void Start()
	{
		bugInfo = GetComponent<Bug_Manager>().GetBugInfo();

		currTargetObj = LevelManager.instance.GetRandomObject();
		currTargetPos = currTargetObj.GetComponent<Object_Manager_Base>().GetRandomPointOnObj();
		foreach (BehaviorWeightPair behavior in behaviors)
		{
			behavior.behavior.SetNewTarget(currTargetPos);
			behaviorWeights.Add(behavior.weight);
		}

		Debug.Log(currTargetPos);
	}

	private void Update()
	{
		if (pauseBehaviors) return;

		Vector3 totalSteeringForce = Vector3.zero;

		// add up all of the behavior steering forces
		bool isFleeing = false;
		foreach (BehaviorWeightPair behavior in behaviors)
		{
			Vector3 force = behavior.behavior.CalculateForce(this) * behavior.weight;
			if (behavior.behavior.IsFleeing() && force != Vector3.zero) isFleeing = true;
			totalSteeringForce += force;
		}

		// clamp so they don't go crazy
		float maxForce = bugInfo.maxForce;
		if (targetReached) maxForce /= 4.0f;
		if (isFleeing) maxForce = bugInfo.maxForce * 2f;
		totalSteeringForce = Vector3.ClampMagnitude(totalSteeringForce, maxForce);

		// math 
		Vector3 acceleration = totalSteeringForce / bugInfo.mass;
		velocity += acceleration * Time.deltaTime;

		//float maxVelocity = bugInfo.maxVelocity;
		//if (targetReached) maxVelocity /= 4;
		velocity = Vector3.ClampMagnitude(velocity, bugInfo.maxVelocity);

		GetComponent<Bug_Manager>().CheckFlipSprite(velocity);

		// update position
		Vector3 newPos = transform.position + velocity * Time.deltaTime;
		transform.position = new Vector3(newPos.x, newPos.y, -5);
	}

	#region Flying Steering Behaviors
	public void TargetReached()
	{
		StartCoroutine(WaitThenTargetReached());
	}

	public bool CheckTargetReached()
	{
		// check if position is close enough to the target
		if (!targetReached && Vector2.Distance((Vector2)transform.position, (Vector2)currTargetPos) <= TARGET_DIST_THRESHOLD)
		{
			targetReached = true;
			TargetReached();
			return true;
		}

		return false;
	}

	private IEnumerator WaitThenTargetReached()
	{
		yield return new WaitForSeconds(3);

		if (currTargetObj == null)
		{
			currTargetObj = LevelManager.instance.GetRandomObject();
		}
		currTargetPos = currTargetObj.GetComponent<Object_Manager_Base>().GetRandomPointOnObj();

		foreach (BehaviorWeightPair behavior in behaviors)
		{
			behavior.behavior.SetNewTarget(currTargetPos);
		}

		StartCoroutine(WaitBeforeResettingTargetReached());
	}

	private IEnumerator WaitBeforeResettingTargetReached()
	{
		yield return new WaitForSeconds(2f);

		targetReached = false;
	}

	public void PauseSteeringBehaviors()
	{
		pauseBehaviors = true;
		foreach (BehaviorWeightPair pair in behaviors)
		{
			pair.weight = 0;
		}
	}

	public void RestoreSteeringBehaviors()
	{
		pauseBehaviors = false;
		for (int i = 0; i < behaviors.Length; i++)
		{
			behaviors[i].weight = behaviorWeights[i];
		}
	}

	#endregion

	#region Landed Steering Behaviors
	public void StartLandedBehaviors()
	{
		StartCoroutine(LandedBehavior());
	}

	private IEnumerator LandedBehavior()
	{
		int cycles = Random.Range(1, 5);

		for (int i = 0; i < cycles; i++)
		{
			yield return new WaitForSeconds(Random.Range(0f, 1f));

			// apply random force to the z-axis to rotate the fly
			float zForce = Random.Range(ROT_FORCE_MIN, ROT_FORCE_MAX);

			rigidbody.AddTorque(zForce, ForceMode2D.Impulse);
		}
	}

	#endregion

	#region Getters

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
	public float GetBugFleeingRadius()
	{
		return bugInfo.fleeingRadius;
	}
	public Vector3 GetVelocity()
	{
		return velocity;
	}
	public GameObject GetTarget()
	{
		return currTargetObj;
	}

	#endregion
}
