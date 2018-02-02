using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballControl : MonoBehaviour {
	public GameObject colBox;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMouseDown (){
		print ("touch");
		gameObject.GetComponent<Mover> ().enabled = false;
		gameObject.GetComponent<Rigidbody> ().isKinematic = false;

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject == colBox) {
			Destroy (gameObject);
		}
	}
}
