using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour {
	
	public GameObject playerPrefab;
	public GameObject wallPrefab;
	public GameObject enemyPrefab;
	public GameObject prizePrefab;

	public float tileWidth = 2f;
	public float tileHeight = 2f;

	public string levelFile = "level1.txt";

	// Use this for initialization
	void Start () {

		// Reading the file into string.
		string levelString = File.ReadAllText(Application.dataPath + Path.DirectorySeparatorChar + levelFile);

		// Splitting the string into lines.
		string[] levelLines = levelString.Split('\n');
		int width = 0;
		// Iterating over the lines.
		for (int row = 0; row < levelLines.Length; row++) {
			string currentLine = levelLines[row];
			width = currentLine.Length;
			// Iterating over all the chars in a line.
			for (int col = 0; col < currentLine.Length; col++) {
				char currentChar = currentLine[col];
				if (currentChar == 'x') {
					// Make a wall!
					GameObject wallObj = Instantiate(wallPrefab);
					wallObj.transform.parent = transform;
					wallObj.transform.position = new Vector3(col*tileWidth, -row*tileHeight, 0);
				}
				else if (currentChar == 'p') {
					// Make the player!
					GameObject playerObj = Instantiate(playerPrefab);
					playerObj.transform.parent = transform;
					playerObj.transform.position = new Vector3(col*tileWidth, -row*tileHeight, 0);
				}
				else if (currentChar == 'e') {
					// We flip a coin
					if (Random.value <= 0.5f) {
						GameObject enemyObj = Instantiate(enemyPrefab);
						enemyObj.transform.parent = transform;
						enemyObj.transform.position = new Vector3(col*tileWidth, -row*tileHeight, 0);
					}
				}
				else if (currentChar == 'a') {
					// Make the prize
					GameObject prizeObj = Instantiate(prizePrefab);
					prizeObj.transform.parent = transform;
					prizeObj.transform.position = new Vector3(col*tileWidth, -row*tileHeight, 0);
				}
			}
		}

		float myX = -(width*tileWidth)/2f + tileWidth/2f;
		float myY = (levelLines.Length*tileHeight)/2f - tileHeight/2f;
		transform.position = new Vector3(myX, myY, 0);

		// If we were centering the level by moving the camera
//		float cameraY = -(levelLines.Length*tileHeight)/2f + tileHeight/2f;
//		float cameraX = (width*tileWidth)/2f - tileWidth/2f;
//		Camera.main.transform.position = new Vector3(cameraX, cameraY, -10);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (BoxCollider2D col){
		if (col.CompareTag ("player")) {
			Destroy (gameObject);
		}
		
	}


}
