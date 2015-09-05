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
	public float speedRunSpeed = 30.0f;
	public float speedRunRotation = 500.0f;
	public float normalSpeed = 15.0f;
	public float normalRotation = 100.0f;
	//private bool canJump = true;
	public bool isLive = true;
	//public bool speedRun = false;
	//public bool fireEnable = false;
	public Material normalMaterial;
	public Material itemUpMaterial;
	public Material fireUpMaterial;
	SphereCollider sphereCollider;
	Rigidbody rb;
	Animator anim;
	SystemController sys;
	Renderer render;

	// ray
	public float rayDistance;

	public bool isFireEnable = false;
	public GameObject fireBall;
	public float nextFire = 0.0f;
	public float fireRate = 0.5f;
		
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
//			if (speedRun) {
//				speed -= Time.deltaTime * minusSpeed;
//				ballRotateSpeed -= Time.deltaTime * minusBallRotateSpeed;
//				if (speed <= 15) {
//					speedRun = false;
//					speed = 15;
//					sphereCollider.isTrigger = false;
//					ballRotateSpeed = 100;
//				} else if (speed <= 20 && speed >= 19) {
//					render.material = normalMaterial;
//					anim.SetTrigger ("Blink");
//				}
//			}
			if (isFireEnable && Input.GetKey("space") && Time.time > nextFire)
			{
				nextFire = Time.time + fireRate;
				Vector3 pos = transform.position;
				GameObject ball = (GameObject) Instantiate(fireBall, new Vector3(pos.x,pos.y,pos.z+2), Quaternion.identity);

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
		ballRotateSpeed = speedRunRotation;
		//speedRun = true;
		sphereCollider.isTrigger = true;
		render.material = itemUpMaterial;
		Invoke("BlankPlayer", 3.0f);
		Invoke("SpeedDown", 5.0f);
	}
	public void SpeedDown()
	{
		//speedRun = false;
		speed = normalSpeed;
		ballRotateSpeed = normalRotation;
		sphereCollider.isTrigger = false;
		render.material = normalMaterial;
	}
	public void FireUp()
	{
		isFireEnable = true;
		render.material = fireUpMaterial;
		Invoke("BlankPlayer", 3.0f);
		Invoke("FireDown", 5.0f);
		// spacebar
	}
	public void FireDown()
	{
		isFireEnable = false;
		render.material = normalMaterial;
		// spacebar
	}
	public void BlankPlayer()
	{
		anim.SetTrigger ("Blink");
	}
}
