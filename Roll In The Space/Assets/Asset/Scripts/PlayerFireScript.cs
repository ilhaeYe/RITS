using UnityEngine;
using System.Collections;

public class PlayerFireScript : MonoBehaviour {

	public float speed = 200.0f;
	Rigidbody rig;
	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody>();
		Destroy(gameObject,2.0f);

	}
	
	// Update is called once per frame
	void Update () {
		rig.AddForce(Vector3.forward * speed);

	}
}
