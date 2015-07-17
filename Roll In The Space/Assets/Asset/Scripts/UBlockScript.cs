using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UBlockScript : BlockScript {

	public List<GameObject> blocks;

	public override void Awake()
	{
		base.Awake();
		foreach(GameObject obj in blocks)
		{
			BlockScript bs = obj.GetComponent<BlockScript>();
			bs.color = color;
		}
	}

	public override void DoButtonLogic()
	{
		moveDes = new Vector3( 0, 0, transform.position.z );
	}

}
