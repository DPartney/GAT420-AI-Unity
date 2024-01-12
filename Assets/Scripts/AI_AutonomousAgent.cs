using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_AutonomousAgent : AI_Agent
{
	public AI_Preception seekPerception = null;
	public AI_Preception fleePerception = null;
	public AI_Preception flockPerception = null;

	public void Update()
	{
		// seek
		if (seekPerception != null)
		{
			var gameObjects = seekPerception.GetGameObjects();
			if (gameObjects.Length > 0)
			{
				movement.ApplyForce(Seek(gameObjects[0]));
			}
		}

		// flee
		if (fleePerception != null)
		{
			var gameObjects = fleePerception.GetGameObjects();
			if (gameObjects.Length > 0)
			{
				movement.ApplyForce(Flee(gameObjects[0]));
			}
		}

		// flock
		if (flockPerception != null)
		{
			var gameObjects = flockPerception.GetGameObjects();
			if (gameObjects.Length > 0)
			{
				movement.ApplyForce(Cohesion(gameObjects));
				//movement.ApplyForce(Seperation(gameObjects));
				movement.ApplyForce(Alignment(gameObjects));
			}
		}


		transform.position = Utilities.Wrap(transform.position, new Vector3(-10, -10, -10), new Vector3(10, 10, 10));
	}

	private Vector3 Seek(GameObject target)
	{
		Vector3 direction = target.transform.position - transform.position;
		return GetSteeringForce(direction);
	}

	private Vector3 Flee(GameObject target)
	{
		Vector3 direction = transform.position - target.transform.position;
		return GetSteeringForce(direction);
	}

	private Vector3 Cohesion(GameObject[] neighbors)
	{
		Vector3 positions = Vector3.zero;
		foreach (var neighbor in neighbors)
		{
			positions += neighbor.transform.position;
		}
		Vector3 center = positions / neighbors.Length;
		Vector3 direction = center - transform.position;
		return GetSteeringForce(direction);
	}
	/*
	private Vector3 Seperation(GameObject[] neighbors, float radius)
	{
		Vector3 seperation = Vector3.zero;
		foreach (var neighbor in neighbors)
		{
			Vector3 direction = transform.position - neighbor.transform.position;
			if (direction.magnitude * radius)
			{

			}
		}

	}
	*/
	private Vector3 Alignment(GameObject[] neighbors)
	{
		Vector3 velocities = Vector3.zero;
		foreach (var neighbor in neighbors)
		{
			velocities += neighbor.GetComponent<AI_Agent>().movement.Velocity;
		}

		Vector3 averageVelocity = velocities / neighbors.Length;

		return GetSteeringForce(averageVelocity);
	}

	public Vector3 GetSteeringForce(Vector3 direction)
	{
		Vector3 desired = direction.normalized * movement.maxSpeed;
		Vector3 steer = desired - movement.Velocity;
		Vector3 force = Vector3.ClampMagnitude(steer, movement.maxForce);

		return force;
	}
}
