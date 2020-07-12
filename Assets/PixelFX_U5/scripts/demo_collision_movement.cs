using UnityEngine;
using System.Collections;

public class demo_collision_movement : MonoBehaviour {
	Vector3 dir;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.transform.Translate(Vector3.right/4f);

	}
}
