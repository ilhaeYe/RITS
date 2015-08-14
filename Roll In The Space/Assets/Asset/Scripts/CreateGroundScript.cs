using UnityEngine;
using System.Collections;

public class CreateGroundScript : MonoBehaviour
{
	private SystemController sys;
	bool used = false;

	void Awake()
	{
		sys = GameObject.FindGameObjectWithTag ("SystemController").GetComponent<SystemController> ();
	}

	public void CreateGround()
	{
		if(!used)
		{
			used = true;
			Vector3 gVec = transform.position;
			gVec.z += 100;
			sys.CreateGrounds (gVec);
		}
	}

//	void OnTriggerEnter (Collider other)
//	{
//		if (other.gameObject.CompareTag ("Player")) {
//			Vector3 gVec = transform.position;
//			gVec.z += 100;
//			sys.CreateGrounds (gVec);
//		}
//	}
}
