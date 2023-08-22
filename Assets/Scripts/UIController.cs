using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Class responsible for UI interactions
public class UIController : MonoBehaviour
{
    //Level select button array used to change level buttons to interactable
    public Button[] levelButtons = new Button[3];

    //Level completed text show in a panel after level is completed
    public TextMeshProUGUI levelCompletedText;

    //Canvas that is enabled after level completed to show level select buttons
    public GameObject levelCompletedCanvas;

    //On Menu scene load checks which levels have been completed, makes following one buttons interactable
    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            UpdateButtons();
        }
    }

    //Based on level completed array in GameManager singleton, updates level select buttons to be interactable
    public void UpdateButtons()
    {
        //First button is omitted from loop as it is always unlocked
        for (int i = 0; i < 3; i++)
        {
            if (GameManager.GM.levelsCompleted[i])
            {
                levelButtons[i].interactable = true;
            }
        }
    }

    //Method called by Quit Button
    public void Quit()
    {
        //If game is running from editor window, end play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        //In other cases quit application
        Application.Quit();
    }

    //Set current level in GameManager to selected and load "Game" scene
    //Method call can be found on level select buttons in "Menu" and "Game" scenes
    public void LoadGameScene(int level)
    {
        GameManager.GM.currentLevel = level;
        SceneManager.LoadScene("Game");
    }

    //Changes scene to "Menu", is attached to "Back to Menu" button in "Game" scene
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    //Method is called after level is completed, shows completion canvas including level select buttons
    public void ShowLevelCompletedCanvas()
    {
        //Update level completed text to reflect current level
        levelCompletedText.text = "Level " + (GameManager.GM.currentLevel+1) + " completed!";
        //Update level select buttons to be interactable based on levels completed
        UpdateButtons();
        //Enable canvas to be visible
        levelCompletedCanvas.SetActive(true);
    }
}