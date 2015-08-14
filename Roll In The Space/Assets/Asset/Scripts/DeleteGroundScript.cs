using UnityEngine;
using System.Collections;

public class DeleteGroundScript : MonoBehaviour
{
	private SystemController sys;
	bool used = false;

	void Awake()
	{
		sys = GameObject.FindGameObjectWithTag ("SystemController").GetComponent<SystemController> ();
	}

	public void DeleteGround()
	{
		if(!used)
		{
			used = true;
			sys.DeleteGrounds();
		}

	}

//	void OnTriggerEnter (Collider other)
//	{
//		if (other.gameObject.CompareTag ("Player")) {
//			sys.DeleteGrounds ();
//		}
//	}
}
