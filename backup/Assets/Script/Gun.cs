using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Transform muzzle;
	public Projectile projectile;
	public GunSmoke gunSmoke;
	public float msBetweenShots = 100;
	public float muzzleVelocity = 53;
	public float gunSmokeDeathTime = 2.5f;
	Renderer renderer;
	Material material;
	Color baseColor = Color.white;
	Color finalColor;
	Animator animator;
	bool muzzleFlag = false;

	float heatTimer = 0;
	float smokeDeathTimer = 2.6f;

	float nextShotTime;
	GunSmoke smoke;

	bool onetime = true;

	void Start() {
		if (onetime) {
			smoke = Instantiate(gunSmoke, muzzle.position, muzzle.rotation) as GunSmoke;
			onetime = false;
		}
		
		smoke.GetComponent<Transform>().parent = muzzle;
		renderer = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
        material = renderer.material;
        animator = muzzle.gameObject.transform.GetChild(0).GetComponent<Animator>();

    }

	void Update() {
		smokeDeathTimer += Time.deltaTime;
		if (smokeDeathTimer > gunSmokeDeathTime) {
			smoke.StopSmoke();
		} else {
			smoke.DrawSmoke();
		}
		if (heatTimer > 0) {
			heatTimer -= Time.deltaTime / 10;
			Heat();
		}
		
		if (muzzleFlag == true) {
			animator.Play("MuzzleFlash");
			StartCoroutine(MuzzleStop());
			muzzleFlag = false;
		}
	}

	void Heat() {
		finalColor = baseColor * heatTimer;
		material.SetColor("_EmissionColor", finalColor);
	}

	public void Shoot() {
		if (Time.time > nextShotTime) {
			nextShotTime = Time.time +msBetweenShots / 1000;
			Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
			newProjectile.SetSpeed (muzzleVelocity);
			smokeDeathTimer = 0;
			muzzleFlag = true;

			if (heatTimer < 3) {
				heatTimer += 0.1f;
				Heat();	
			}
		}
	}

	IEnumerator MuzzleStop() {
		yield return new WaitForSeconds (.2f);
	}
}
