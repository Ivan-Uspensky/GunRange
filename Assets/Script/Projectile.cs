
using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public LayerMask collisionMask;
	public Transform Spark;
	public Transform Dust;
	public Transform Splinter;
	public float SpreadRange;
	float speed = 10;
	float damage = 1;

	float lifetime = 3;
	float skinWidth = .1f;
	

	Vector3 spreadVector;
	
	void Start() {
		Destroy (gameObject, lifetime);

		float spread = Random.Range (-SpreadRange, SpreadRange);
		spreadVector = new Vector3(spread, spread, 1);

		Collider[] initialCollisions = Physics.OverlapSphere (transform.position, .1f, collisionMask);
		if (initialCollisions.Length > 0) {
			OnHitObject(initialCollisions[0], transform.position);
		}
	}
	
	public void SetSpeed(float newSpeed) {
		speed = newSpeed;
	}

	void Update () {
		float moveDistance = Time.deltaTime * speed;
		CheckCollisions (moveDistance);
		transform.Translate (spreadVector * moveDistance);
	}

	void CheckCollisions(float moveDistance) {
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)) {
			OnHitObject(hit.collider, hit.point);
		}
	}

	void OnHitObject(Collider c, Vector3 hitPoint) {
		GameObject.Destroy (gameObject);
		Transform hitDust = Instantiate(Dust, hitPoint, Quaternion.FromToRotation (Vector3.forward, -transform.forward)) as Transform;
		Transform hitParticle = Instantiate(Spark, hitPoint, Quaternion.FromToRotation (Vector3.forward, -transform.forward)) as Transform;
		Transform hitSplinter = Instantiate(Splinter, hitPoint, Quaternion.FromToRotation (Vector3.forward, -transform.forward)) as Transform;
		Destroy(hitParticle.gameObject, 1f);
		Destroy(hitDust.gameObject, 2f);
		Destroy(hitSplinter.gameObject, 2f);
	}
}
