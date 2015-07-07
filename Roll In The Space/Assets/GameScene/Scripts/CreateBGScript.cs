using UnityEngine;
using System.Collections;

public class CreateBGScript : MonoBehaviour {
	private SystemController sys;
	
	void Awake()
	{
		sys = GameObject.FindGameObjectWithTag ("SystemController").GetComponent<SystemController> ();
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			sys.CreateBackGround (transform.position);
		}
	}
}
