using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour {

	[SerializeField] private float moveSpeed = 5.0f;
	[SerializeField] private float jumpForce = 13.0f;
	private CharacterController characterController;
	private Animator anim;
	private Player2Health health;
	private float dir;
	private BoxCollider[] swordColliders;
	private Vector3 jumpDir;

	void Start () {
		characterController = GetComponent<CharacterController>();
		anim = GetComponent<Animator> ();
		swordColliders = GetComponentsInChildren<BoxCollider> ();
		health = GetComponent<Player2Health> ();
	}

	void Update () {

		//Vector3 moveDirection = new Vector3 (Input.GetAxisRaw ("JS2Horizontal"), 0, 0);

		if (!health.GetIsAlive()) {
			foreach (var weapon in swordColliders) {
				weapon.enabled = false;
			}
			return;
		}

		if (Input.GetAxis ("JS2Horizontal") < 0) {
			dir = -1;
		} else if (Input.GetAxis ("JS2Horizontal") > 0) {
			dir = 1;
		} else {
			dir = 0;
		}

		characterController.SimpleMove (new Vector3 (dir, 0, 0) * moveSpeed);

		if (dir == 0) {
			anim.SetBool ("IsRunning", false);
		} else {
			anim.SetBool ("IsRunning", true);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (new Vector3 (dir, 0, 0)), 0.20f);
		}

		if (Input.GetKeyDown(KeyCode.Joystick2Button2)) {
			foreach (var weapon in swordColliders) {
				weapon.enabled = false;
			}
			anim.Play ("DoubleChop");
		}

		if (Input.GetKeyDown(KeyCode.Joystick2Button3)) {
			foreach (var weapon in swordColliders) {
				weapon.enabled = false;
			}
			anim.Play ("SpinAttack");
		}
	
		if (Input.GetKeyDown(KeyCode.Joystick2Button0)) {
			if (characterController.isGrounded) {
				foreach (var weapon in swordColliders) {
					weapon.enabled = false;
				}
				anim.Play ("Jump");
				jumpDir = new Vector3 (Input.GetAxis ("JS1Horizontal"), jumpForce, 0);
				characterController.Move(jumpDir * Time.deltaTime);
				anim.SetBool ("IsGrounded", false);
			}
		}

		if (jumpDir.y > 0) {
			characterController.Move(jumpDir * Time.deltaTime);
			jumpDir.y -= jumpForce / 20;
		} else {
			anim.SetBool ("IsGrounded", true);
		}

		transform.position = new Vector3 (transform.position.x, transform.position.y, -6);

	}

	public void BeginAttack() {
		foreach (var weapon in swordColliders) {
			weapon.enabled = true;
			Debug.Log (weapon.enabled);
		}
	}

	public void EndAttack() {
		foreach (var weapon in swordColliders) {
			weapon.enabled = false;
			Debug.Log (weapon.enabled);
		}
	}

}