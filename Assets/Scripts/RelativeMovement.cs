using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour {

	[SerializeField] private Transform target;

	public float rotateSpeed = 15.0f;
	public float moveSpeed = 6.0f;
	public float jumpSpeed = 15.0f;
	public float gravity = -9.8f;
	public float terminalVelocity = -10.0f;
	public float minFall = -1.5f;

	private float vertSpeed;

	private CharacterController charController;
	private ControllerColliderHit contact;

	private Animator animator;

	void Start() {
		charController = GetComponent<CharacterController> ();
		animator = GetComponent<Animator> ();
		vertSpeed = minFall;
	}

	// Update is called once per frame
	void Update () {
		Vector3 movement = Vector3.zero;

		float horInput = Input.GetAxis ("Horizontal");
		float vertInput = Input.GetAxis ("Vertical");
		if (horInput != 0 || vertInput != 0) {
			movement.x = horInput * moveSpeed;
			movement.z = vertInput * moveSpeed;
			movement = Vector3.ClampMagnitude (movement, moveSpeed);

			Quaternion tmp = target.rotation;
			target.eulerAngles = new Vector3 (0, target.eulerAngles.y, 0);
			movement = target.TransformDirection (movement);
			target.rotation = tmp;

			// Linear Interpolate rotation movement to make it smoother
			Quaternion direction = Quaternion.LookRotation(movement);
			transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotateSpeed * Time.deltaTime);
		}

		animator.SetFloat ("Speed", movement.sqrMagnitude);

		// Check jump
		bool hitGround = false;
		RaycastHit hit;
		if (vertSpeed < 0 && Physics.Raycast (transform.position, Vector3.down, out hit)) {
			float check = (charController.height + charController.radius) / 1.9f;
			hitGround = hit.distance <= check;
		}
		if (hitGround) {
			if (Input.GetButton ("Jump")) {
				vertSpeed = jumpSpeed;
			} else {
				vertSpeed = minFall;
				animator.SetBool ("Jumping", false);
			}
		} else {
			vertSpeed += gravity * 5 * Time.deltaTime;
			if (vertSpeed < terminalVelocity) {
				vertSpeed = terminalVelocity;
			}

			if (contact != null) {
				animator.SetBool ("Jumping", true);
			}

			if (charController.isGrounded) {
				if (Vector3.Dot (movement, contact.normal) < 0) {
					movement = contact.normal * moveSpeed;
				} else {
					movement += contact.normal * moveSpeed;
				}
			}
		}
		movement.y = vertSpeed;			

		movement *= Time.deltaTime;
		charController.Move (movement);
	}

	void OnControllerColliderHit( ControllerColliderHit hit) {
		contact = hit;
	}
}
