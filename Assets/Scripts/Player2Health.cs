using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class Player2Health : MonoBehaviour {
 
	[SerializeField] private int startingHealth = 100;
	[SerializeField] private float timeSinceLastHit = 0.5f;
	[SerializeField] Slider healthSlider;

	private float timer = 0.5f;
	private Animator anim;
	private CharacterController cc;
	private int currentHealth;
	private bool isAlive = true;

	void Awake () {
		Assert.IsNotNull (healthSlider);
	}

	void Start () {
		anim = GetComponent<Animator> ();
		cc = GetComponent<CharacterController> ();
		currentHealth = startingHealth;
	}

	void Update () {
		timer += Time.deltaTime;
	}

	void OnTriggerEnter (Collider other) {
		if (timer >= timeSinceLastHit && currentHealth > 0) {
			if (other.tag == "PL1Weapon" && other.enabled) {
				takeHit ();
				timer = 0;
			}
		}
	}

	void takeHit () {
		if (currentHealth > 0) {
			anim.Play ("Hurt");
			currentHealth -= 10;
			healthSlider.value = currentHealth;
		}

		if (currentHealth <= 0) {
			killPlayer ();
			isAlive = false;
		}
	}

	void killPlayer() {
		anim.SetTrigger ("PlayerDeath");
		cc.enabled = false;
	}

	public bool GetIsAlive() {
		return isAlive;
	}
}
