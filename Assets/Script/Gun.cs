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

	float heatTimer = 0;
	float smokeDeathTimer = 2.6f;

	float nextShotTime;
	GunSmoke smoke;

	void Start() {
		smoke = Instantiate(gunSmoke, muzzle.position, muzzle.rotation) as GunSmoke;
		smoke.GetComponent<Transform>().parent = muzzle;
		renderer = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
        material = renderer.material;
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
	}

	void Heat() {
		print("heatTimer: " + heatTimer);
		finalColor = baseColor * heatTimer;
		print("finalColor: " + finalColor);
		material.SetColor("_EmissionColor", finalColor);
	}

	public void Shoot() {
		if (Time.time > nextShotTime) {
			nextShotTime = Time.time +msBetweenShots / 1000;
			Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
			newProjectile.SetSpeed (muzzleVelocity);
			smokeDeathTimer = 0;
			
			if (heatTimer < 3) {
				heatTimer += 0.1f;
				Heat();	
			}
		}
	}
}
