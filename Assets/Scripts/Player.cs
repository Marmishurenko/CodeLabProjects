using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public KeyCode upKey = KeyCode.UpArrow;
	public KeyCode downKey = KeyCode.DownArrow;
	public KeyCode leftKey = KeyCode.LeftArrow;
	public KeyCode rightKey = KeyCode.RightArrow;

	bool facingRight = false;
	Transform transform;

	public string[] sceneNames;
	int curSceneIndex = 0;

	public float moveSpeed = 10f;

	Rigidbody2D rb;

    public delegate void PlayerEvent();
    public PlayerEvent onCucumberCollide;
    public PlayerEvent onPrizeCollide;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		transform = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector2 newVelocity = new Vector2(0, 0);
		if (Input.GetKey(upKey)) {
			newVelocity.y += moveSpeed;
		}
		if (Input.GetKey(rightKey)) {
			newVelocity.x += moveSpeed;
		}
		if (Input.GetKey(downKey)) {
			newVelocity.y -= moveSpeed;
		}
		if (Input.GetKey(leftKey)) {
			newVelocity.x -= moveSpeed;
		}


		newVelocity = newVelocity.normalized*moveSpeed;
		rb.velocity = newVelocity;

		// get the current scale
		Vector3 localScale = transform.localScale;

		if (newVelocity.x > 0) // moving right so face right
		{
			facingRight = false;
		} else if (newVelocity.x < 0) { // moving left so face left
			facingRight = true;
		}

		// check to see if scale x is right for the player
		// if not, multiple by -1 which is an easy way to flip a sprite
		if (((facingRight) && (localScale.x<0)) || ((!facingRight) && (localScale.x>0))) {
			localScale.x *= -1;
		}

		// update the scale
		transform.localScale = localScale;
	}

	void OnTriggerEnter2D (Collider2D other){
		if (other.gameObject.tag =="prize") {
			Destroy (other.gameObject);
			// todo: Play animation
			float animationLength = 0.2f;
            StartCoroutine (DelayEvent ("prize", animationLength));
		}

        if (other.gameObject.tag == "cuc")
        {
			// todo: Play animation
			float animationLength = 0.2f;
            StartCoroutine (DelayEvent ("cuc", animationLength));
        }
    }

	IEnumerator DelayEvent(string tagName, float delay){
		yield return new WaitForSeconds (delay);
        switch(tagName){
            case "prize":{
                    if (onPrizeCollide!= null)
                        onPrizeCollide();
                    break;
                }

            case "cuc":{
                    if (onCucumberCollide != null)
                        onCucumberCollide();
                    break;    
                }

        }
	}

}
