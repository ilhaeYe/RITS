using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float speed;
	//public float jumpSpeed;
	public float radians;
	public float rotateSpeed;
	public float ballRotateSpeed;
	public float blockingSpeed;
	public float minusSpeed;
	public float minusBallRotateSpeed;
	public float touchSpeed;
	public float rotatePosition;
	public float speedRunSpeed;
	//private bool canJump = true;
	public bool isLive = true;
	public bool speedRun = false;
	public Material normalMaterial;
	public Material itemUpMaterial;
	SphereCollider sphereCollider;
	Rigidbody rb;
	Animator anim;
	SystemController sys;
	Renderer render;

	// ray
	public float rayDistance;
		
	void Start ()
	{
		radians = 0f;
		sphereCollider = GetComponent<SphereCollider> ();
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		sys = GameObject.Find ("SystemController").GetComponent<SystemController> ();
		render = GetComponent<Renderer> ();
	}
	
	void Update ()
	{
		RaycastHit hit;
		Ray detectRay = new Ray(transform.position, Vector3.back);
		
		//Debug.DrawRay(transform.position, Vector3.back * rayDistance);
		
		if(Physics.Raycast(detectRay, out hit))
		{
			string tagStr = hit.collider.tag;
			if(tagStr == "CreateEvent")
			{
				CreateGroundScript objCon = hit.transform.GetComponent<CreateGroundScript>();
				objCon.CreateGround();
			}
			if (tagStr == "DeleteEvent")
			{
				DeleteGroundScript objCon = hit.transform.GetComponent<DeleteGroundScript>();
				objCon.DeleteGround();
			}
			if (tagStr == "CreateBGEvent")
			{
				CreateBGScript objCon = hit.transform.GetComponent<CreateBGScript>();
				objCon.CreateBackGround();
			}
			if ( tagStr == "ButtonClickEvent")
			{
				ButtonScript objCon = hit.transform.GetComponent<ButtonScript>();
				objCon.ButtonClicked();
			}
		}

	}

	void FixedUpdate ()
	{
		if (isLive && sys.isStartingPause) {
			if (Input.GetKey ("space")) {
				sys.StartGame ();
			}
		} else if (isLive && !sys.isStartingPause) {
			if (speedRun) {
				speed -= Time.deltaTime * minusSpeed;
				ballRotateSpeed -= Time.deltaTime * minusBallRotateSpeed;
				if (speed <= 15) {
					speedRun = false;
					speed = 15;
					sphereCollider.isTrigger = false;
					ballRotateSpeed = 100;
				} else if (speed <= 20 && speed >= 19) {
					render.material = normalMaterial;
					anim.SetTrigger ("Blink");
				}
			}
			// toward move
			float moveSpeed = speed * Time.deltaTime;
			Vector3 moveDes = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 1);
			transform.position = Vector3.MoveTowards (transform.position, moveDes, moveSpeed);

			// toward rotation
			Vector3 vec = new Vector3 (Mathf.Cos (radians * Mathf.Deg2Rad) * ballRotateSpeed, -Mathf.Sin (radians * Mathf.Deg2Rad) * ballRotateSpeed, 0);
			transform.Rotate (vec * Time.deltaTime,Space.World);

			// left & right move
			float movement = rotateSpeed * Time.deltaTime * Input.GetAxisRaw ("Horizontal");
			radians += movement;
//			transform.position = new Vector3 (Mathf.Sin (radians * Mathf.Deg2Rad), Mathf.Cos (radians * Mathf.Deg2Rad), transform.position.z);
			transform.position = new Vector3 (Mathf.Sin (radians * Mathf.Deg2Rad) * rotatePosition, Mathf.Cos (radians * Mathf.Deg2Rad) * rotatePosition, transform.position.z);
		} else if (!isLive) {
			Invoke ("StopMove", 1);
		}
	}

	void StopMove ()
	{
		rb = GetComponent<Rigidbody> ();
		rb.velocity = Vector3.zero;
	}

	public void SpeedRun ()
	{
		speed = speedRunSpeed;
		ballRotateSpeed = 500;
		speedRun = true;
		sphereCollider.isTrigger = true;
		render.material = itemUpMaterial;
	}
}
