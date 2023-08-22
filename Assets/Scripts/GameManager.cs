using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string fileName = "Assets/Data/level_data.json";
    public static GameManager GM { get; private set; }

    [SerializeField]
    public RopeController ropeController;
    public UIController uiController;

    public int lastClicked = 0;
    public GameObject lastClickedGem;
    public List<GameObject> gems = new List<GameObject>();
    public int currentLevel;

    public bool[] levelsCompleted = new bool[3];

    public Levels levelsContainer;

    public GameObject gemPrefab;
    public Transform gemParent;

    private void Awake()
    {
        if (GM != null && GM != this)
            Destroy(this.gameObject);
        else
        {
            GM = this;
            DontDestroyOnLoad(this); 
            string jsonString = File.ReadAllText(fileName);
            GM.levelsContainer = JsonUtility.FromJson<Levels>(jsonString);

            SceneManager.sceneLoaded += InitializeGame;
        }
    }

    public void InitializeGame(Scene scene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Menu":
            //    lastClicked = 0;
            //    lastClickedGem = null;
            //    gems = new List<GameObject>();
                currentLevel = 0;
                break;

            case "Game":
                lastClicked = 0;
                lastClickedGem = null;
                gems = new List<GameObject>();

                gemParent = GameObject.Find("Gems").transform;
                ropeController = GameObject.Find("Ropes").GetComponent<RopeController>();
                uiController = GameObject.Find("UIController").GetComponent<UIController>();

                LevelData currentLevelData = GM.levelsContainer.levels[currentLevel];
                for (int i = 0; i < currentLevelData.level_data.Length - 1; i += 2)
                {
                    int copy = i;
                    GameObject result = Instantiate(gemPrefab, new Vector2((currentLevelData.level_data[copy] / 125) - 4f, (currentLevelData.level_data[copy + 1] / 125) - 4f), gemPrefab.transform.rotation, gemParent);
                    gems.Add(result);
                }
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}