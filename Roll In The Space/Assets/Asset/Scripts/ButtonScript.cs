using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonScript : MonoBehaviour {

	public List<GameObject> targets;
	bool used = false;

	public void ButtonClicked()
	{
		if(!used)
		{
			used = true;
			foreach( GameObject obj in targets )
			{
				BlockScript bs = obj.GetComponent<BlockScript>();
				bs.DoButtonLogic();
			}

		}
	}

//	void OnCollisionEnter(Collision collision)
//	{
//		if (collision.gameObject.CompareTag ("Player")) {
//			foreach( GameObject obj in targets )
//			{
//				BlockScript bs = obj.GetComponent<BlockScript>();
//				bs.DoButtonLogic();
//			}
//		}
//	}

}
