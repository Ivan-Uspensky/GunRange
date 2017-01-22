using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GunController))]
public class Player : MonoBehaviour {

	GunController gunController;

	void Start () {
		gunController = GetComponent<GunController>();
	}
	
	void Update () {
		if (Input.GetMouseButton(0)) {
			gunController.Shoot();
		}
	}
}
