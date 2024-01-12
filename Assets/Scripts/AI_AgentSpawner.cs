using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_AgentSpawner : MonoBehaviour
{
	[SerializeField] AI_Agent[] agents;
	[SerializeField] LayerMask layerMask;

	int index = 0;

	void Update()
	{
		// tab switches between agents
		if (Input.GetKeyDown(KeyCode.Tab)) index = (++index % agents.Length);

		// click to spawn or left control plus click for multiple
		if (Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl)))
		{
			// get ray from camera to screen position
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// raycast and see if it hits an object
			if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layerMask))
			{
				// spawn agent at hit point and random rotation
				Instantiate(agents[index], hitInfo.point, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
			}
		}
	}
}
