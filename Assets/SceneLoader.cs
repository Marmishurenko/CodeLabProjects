﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    public void loadScene(string name){
        SceneManager.LoadScene(name);
    }
}
