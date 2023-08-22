using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Individual gem class responsible for gem click functionality
public class Gem : MonoBehaviour
{
    //This gems number displayed next to gem GameObject
    public int number;

    //Number text component used to change number text to gems number
    public TextMeshProUGUI numberText;
    //Number text animator component used to trigger number fade-out after click
    public Animator numberAnimator;

    //Component used to change gem visual after click
    public SpriteRenderer spriteRenderer;

    //Sprite set in prefab through inspector of a blue gem
    public Sprite blueGem;

    //Button component responsible for click action, initialized during runtime
    public Button button;


    public void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

        //As all gems are stored under an empty GameObject, their sibling index correlates to gem number
        number = transform.parent.GetSiblingIndex() + 1;

        //Retrieve gem number GameObject based on predetermined object hierarchy
        GameObject numberTextGameObject = transform.parent.GetChild(1).GetChild(0).gameObject;

            numberAnimator = numberTextGameObject.GetComponentInChildren<Animator>();

            numberText = numberTextGameObject.GetComponentInChildren<TextMeshProUGUI>();
            numberText.text = number.ToString();

        //Initialize button component and add on click listener for OnClick method
        button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    //Method is called when gem is clicked
    public void OnClick()
    {
        //If the first gem is clicked and no other gems were clicked before, set it to clicked
        if (number == 1 && GameManager.GM.lastClickedGem == null)
        {
            SetGemToClicked();
        }
        //Check if clicked gem is one number higher than previous successful click
        else
        if (number == GameManager.GM.lastClickedGem.GetComponent<Gem>().number + 1)
        {
            //Rope must be created before setting this gem as previously clicked
            GameManager.GM.ropeController.CreateRope(GameManager.GM.lastClickedGem.transform.position, transform.position);
            SetGemToClicked();

            //Check if this gem is the last in current level
            if (number == GameManager.GM.gems.Count)
            {
                //Draw additional rope from last to first gem
                GameManager.GM.ropeController.CreateRope(transform.position, GameManager.GM.gems[0].transform.position);
            }
        }
    }

    //Method is triggered by successful gem click
    //Runs number fade-out animation
    //Changes gem sprite to blue
    //Sets this gem as last one clicked successfully
    private void SetGemToClicked()
    {
        numberAnimator.SetTrigger("Clicked");
        spriteRenderer.sprite = blueGem;
        GameManager.GM.lastClickedGem = gameObject;
    }
}
