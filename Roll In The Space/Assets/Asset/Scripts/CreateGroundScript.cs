using UnityEngine;
using System.Collections;

public class CreateGroundScript : MonoBehaviour
{
	private SystemController sys;

	void Awake()
	{
		sys = GameObject.FindGameObjectWithTag ("SystemController").GetComponent<SystemController> ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			Vector3 gVec = transform.position;
			gVec.z += 100;
			sys.CreateGrounds (gVec);
		}
	}
}
