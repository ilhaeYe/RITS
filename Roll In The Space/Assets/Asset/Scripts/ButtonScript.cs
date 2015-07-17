using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonScript : MonoBehaviour {

	public List<GameObject> targets;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag ("Player")) {
			foreach( GameObject obj in targets )
			{
				BlockScript bs = obj.GetComponent<BlockScript>();
				bs.DoButtonLogic();
			}
		}
	}

}
