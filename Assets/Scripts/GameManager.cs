using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [System.Serializable]
    public class LevelData{
        public string levelPath;
    }

	public GameObject playerPrefab;
	public GameObject wallPrefab;
	public GameObject enemyPrefab;
	public GameObject prizePrefab;
    GameObject sceneRoot;

	public float tileWidth = 2f;
	public float tileHeight = 2f;

    public LevelData[] levels;

    string curLevelPath{ get{ return levels[curLevel].levelPath; }}

    public int curLevel = 0;

	private void Awake()
	{
        LoadSettings();
        CreateSceneRoot();
        CreateLevel(curLevelPath);
	}

	// Use this for initialization
	void Start () {
        
	}

    void LoadNextLevel(){
        curLevel++;

        if(curLevel > levels.Length - 1 ){
            // todo : load start scene
            curLevel = 0;
            SceneManager.LoadScene("YouWon");
            return;
        }

        CreateSceneRoot();
        CreateLevel(curLevelPath);
    }

   

    void LoadStartScene(){
        SceneManager.LoadScene("YouLost");
    }

    void SaveSettings(){
        string settingsPath = Application.dataPath + Path.DirectorySeparatorChar + "settings.txt";
        // to save other settings sw.WriteLine("{0},{1},{2}", var1, var2, var3);
        File.Delete(settingsPath); // overwrite settings
        StreamWriter sw = File.AppendText(settingsPath);
        sw.WriteLine("{0},{1}", curLevel, tileWidth);
        print("Settings are saved to " + settingsPath);
        sw.Close();
    }

    void LoadSettings(){
        string settingsPath = Application.dataPath + Path.DirectorySeparatorChar + "settings.txt";

        if (File.Exists(settingsPath)){
            string text = File.ReadAllText(settingsPath);
            string[] lines = text.Split(',');
            int.TryParse(lines[0], out curLevel);
            print("Settings are loaded from " + settingsPath);
        }else{
            SaveSettings();
        }
    }

	private void OnDestroy()
	{
        SaveSettings();
	}

    void CreateSceneRoot()
    {
        if (sceneRoot != null)
        {
            Destroy(sceneRoot);
        }
        sceneRoot = new GameObject();
        sceneRoot.name = "sceneRoot";
    }

	void CreateLevel(string levelFileName){

        // Reading the file into string.
        string levelString = File.ReadAllText(Application.dataPath + Path.DirectorySeparatorChar + levelFileName);

        // Splitting the string into lines.
        string[] levelLines = levelString.Split('\n');
        int width = 0;
        // Iterating over the lines.
        for (int row = 0; row < levelLines.Length; row++)
        {
            string currentLine = levelLines[row];
            width = currentLine.Length;
            // Iterating over all the chars in a line.
            for (int col = 0; col < currentLine.Length; col++)
            {
                char currentChar = currentLine[col];
                if (currentChar == 'x')
                {
                    // Make a wall!
                    GameObject wallObj = Instantiate(wallPrefab);
                    wallObj.transform.parent = sceneRoot.transform;
                    wallObj.transform.position = new Vector3(col * tileWidth, -row * tileHeight, 0);
                }
                else if (currentChar == 'p')
                {
                    // Make the player!
                    GameObject playerObj = Instantiate(playerPrefab);
                    Player player = playerObj.GetComponent<Player>();
                    player.onPrizeCollide += LoadNextLevel;
                    player.onCucumberCollide += LoadStartScene;
                    playerObj.transform.parent = sceneRoot.transform;
                    playerObj.transform.position = new Vector3(col * tileWidth, -row * tileHeight, 0);
                }
                else if (currentChar == 'e')
                {
                    // We flip a coin
                    if (Random.value <= 0.5f)
                    {
                        GameObject enemyObj = Instantiate(enemyPrefab);
                        enemyObj.transform.parent = sceneRoot.transform;
                        enemyObj.transform.position = new Vector3(col * tileWidth, -row * tileHeight, 0);
                    }
                }
                else if (currentChar == 'a')
                {
                    // Make the prize
                    GameObject prizeObj = Instantiate(prizePrefab);
                    prizeObj.transform.parent = sceneRoot.transform;
                    prizeObj.transform.position = new Vector3(col * tileWidth, -row * tileHeight, 0);
                }
            }
        }

        float myX = -(width * tileWidth) / 2f + tileWidth / 2f;
        float myY = (levelLines.Length * tileHeight) / 2f - tileHeight / 2f;
        sceneRoot.transform.position = new Vector3(myX, myY, 0);

        // If we were centering the level by moving the camera
        //      float cameraY = -(levelLines.Length*tileHeight)/2f + tileHeight/2f;
        //      float cameraX = (width*tileWidth)/2f - tileWidth/2f;
        //      Camera.main.transform.position = new Vector3(cameraX, cameraY, -10);
    }
	// Update is called once per frame
	void Update () {
        
	}


		
}



