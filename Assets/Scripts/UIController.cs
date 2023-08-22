using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Button[] levelButtons = new Button[3];

    public TextMeshProUGUI levelCompletedText;

    public GameObject levelCompletedCanvas;

    // Start is called before the first frame update
    private void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Menu":
                UpdateButtons();
                break;

            case "Game":
                break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void button_exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    //I copied this to GameUIController, not sure if there is a better solution
    public void LoadGameScene(int level)
    {
        GameManager.GM.currentLevel = level;
        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < 3; i++)
        {
            if (GameManager.GM.levelsCompleted[i])
            {
                levelButtons[i].interactable = true;
            }
        }
    }

    public void ShowLevelCompletedPanel()
    {
        //Incorrect level if after level completed
        levelCompletedText.text = "Level " + (GameManager.GM.currentLevel+1) + " completed!";
        UpdateButtons();
        levelCompletedCanvas.SetActive(true);
    }
}