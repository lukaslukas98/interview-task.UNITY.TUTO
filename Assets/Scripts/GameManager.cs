using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

//Singleton class used to connect all system controllers
public class GameManager : MonoBehaviour
{
    //Singleton object accessible from anywhere
    public static GameManager GM { get; private set; }

    //Rope and UI system controllers
    public RopeController ropeController;
    public UIController uiController;

    //Path to data file, alternatively it could be a TextAsset set in inspector
    public string fileName = "Assets/Data/level_data.json";
    //Variable holding all level data
    public Levels levelsContainer;

    //List of all gems in current level
    public List<GameObject> gems = new List<GameObject>();
    public GameObject lastClickedGem;

    //Array used to track completed levels for level selection buttons
    public bool[] levelsCompleted = new bool[3];
    public int currentLevel;

    //Set in inspector as component of "Game State Manager" prefab
    public GameObject gemPrefab;
    //Initialized during runtime
    public Transform gemParent;

    //Initializes singleton object if it has not yet been initialized
    //Retrieves data from JSON file on initial run
    private void Awake()
    {
        //If another GameManager exists that is singleton, destroy this
        if (GM != null && GM != this)
            Destroy(gameObject);
        //If there are no GameManagers and singleton is not set, set this as main GameManager
        else
        {
            GM = this;
            //Makes this GameObject not unload during scene transitions
            DontDestroyOnLoad(this);

            //Reads data from JSON, creates objects using it
            string jsonString = File.ReadAllText(fileName);
            levelsContainer = JsonUtility.FromJson<Levels>(jsonString);

            //Subscribe InitializeGame method to sceneLoaded event, to initialize objects after scene change
            SceneManager.sceneLoaded += InitializeGame;
        }
    }

    //Initializes game state
    public void InitializeGame(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Game")
        {
            //Reset game variables
            lastClickedGem = null;
            gems = new List<GameObject>();

            //Attach controllers
            ropeController = GameObject.Find("Ropes").GetComponent<RopeController>();
            uiController = GameObject.Find("UIController").GetComponent<UIController>();

            //Attach gem parent GameObject to store newly instantiated gems
            gemParent = GameObject.Find("Gems").transform;

            //Read current level data
            LevelData currentLevelData = levelsContainer.levels[currentLevel];
            //Loop through level data two at a time, setting first coordinate to x and second to y
            for (int i = 0; i < currentLevelData.level_data.Length - 1; i += 2)
            {
                //Instantiate gems
                GameObject result = Instantiate(gemPrefab, new Vector2((currentLevelData.level_data[i] / 125) - 4f, (currentLevelData.level_data[i + 1] / 125) - 4f), gemPrefab.transform.rotation, gemParent);
                //Add gems to list
                gems.Add(result);
            }
        }
    }

    private void Update()
    {
        //If Escape is pressed in "Game" scene, load "Menu" scene
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "Game")
        {
            SceneManager.LoadScene("Menu");
        }
    }

    //Method triggers after final rope fades in to final gem
    //Sets current level to completed
    //Shows level completed canvas
    public void SetLevelCompleted()
    {
        //If this is not the final (4) level, set it to completed
        if (currentLevel < 3)
        {
            levelsCompleted[currentLevel] = true;
        }
        if (lastClickedGem.GetComponent<Gem>().number == gems.Count)
        {
            uiController.ShowLevelCompletedCanvas();
        }
    }
}