using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Transform muzzle;
	public Projectile projectile;
	public GunSmoke gunSmoke;
	public float msBetweenShots = 100;
	public float muzzleVelocity = 53;
	public float gunSmokeDeathTime = 2.5f;

	float smokeDeathTimer = 2.6f;
	float nextShotTime;
	GunSmoke smoke;

	void Start() {
		smoke = Instantiate(gunSmoke, muzzle.position, muzzle.rotation) as GunSmoke;
		smoke.GetComponent<Transform>().parent = muzzle;
	}

	void Update() {
		smokeDeathTimer += Time.deltaTime;
		if (smokeDeathTimer > gunSmokeDeathTime) {
			smoke.StopSmoke();
		} else {
			smoke.DrawSmoke();
		}
	}

	public void Shoot() {
		if (Time.time > nextShotTime) {
			nextShotTime = Time.time +msBetweenShots / 1000;
			Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
			newProjectile.SetSpeed (muzzleVelocity);

			smokeDeathTimer = 0;

		}
	}
}
