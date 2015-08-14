using UnityEngine;
using System.Collections;

public class CreateBGScript : MonoBehaviour {
	private SystemController sys;
	public bool used = false;

	void Awake()
	{
		sys = GameObject.FindGameObjectWithTag ("SystemController").GetComponent<SystemController> ();
	}

	public void CreateBackGround()
	{
		if(!used)
		{
			used = true;
			sys.CreateBackGround(transform.position);
		}
	}

//	void OnTriggerEnter (Collider other)
//	{
//		if (other.gameObject.CompareTag ("Player")) {
//			sys.CreateBackGround (transform.position);
//		}
//	}
}
