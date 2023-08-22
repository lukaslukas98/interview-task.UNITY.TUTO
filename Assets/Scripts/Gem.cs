using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Gem : MonoBehaviour
{
    public bool clicked;
    public TextMeshProUGUI numberText;
    public Animator numberAnimator;
    public int number;
    public Sprite blueGem;
    public SpriteRenderer spriteRenderer;
    public Button button;
    public LineRenderer rope;


    public void Start()
    {


        spriteRenderer = GetComponent<SpriteRenderer>();
        // number = ((i / 2) + 1);
        //  SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        //  transform.GetChild(0).GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = ((i / 2) + 1).ToString(); ;
        number = transform.parent.GetSiblingIndex() + 1;
        numberAnimator = transform.parent.GetChild(1).GetChild(0).GetComponentInChildren<Animator>();
        numberText = transform.parent.GetChild(1).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        numberText.text = number.ToString();


        rope = transform.parent.GetChild(2).GetComponentInChildren<LineRenderer>();


        //button = GetComponent<Button>();
        int num = number;
        //button.onClick.AddListener(() => onclick(num));

        button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(onclick);
    }


    public void ChangeNumberText()
    {
    }


    public void onclick()
    {
        //  numberAnimation.

        // int tempNum = number;
        // Debug.Log(tempNum);
        // Debug.Log("3");
        if (number == 1 && GameManager.GM.lastClickedGem == null)
        {
            numberAnimator.SetTrigger("Clicked");
            spriteRenderer.sprite = blueGem;
            GameManager.GM.lastClicked = number;
            GameManager.GM.lastClickedGem = gameObject;
        }
        else
        if (number - 1 == GameManager.GM.lastClicked)
        {
            numberAnimator.SetTrigger("Clicked");
            GameManager.GM.ropeController.CreateRope(GameManager.GM.lastClickedGem.transform.position, transform.position);
            spriteRenderer.sprite = blueGem;
            GameManager.GM.lastClicked = number;
            GameManager.GM.lastClickedGem = gameObject;

            if(number == GameManager.GM.gems.Count)
            {
                GameManager.GM.ropeController.CreateRope(transform.position, GameManager.GM.gems[0].transform.position);
                if (GameManager.GM.currentLevel < 3)
                {
                    GameManager.GM.levelsCompleted[GameManager.GM.currentLevel] = true;
                }
            }
        }
        


    }










    //public void onclick(int previouseClicked)
    //{
    //    Debug.Log("triggered");
    //    if (number == 1)
    //    {
    //        Debug.Log("1");
    //        spriteRenderer.sprite = blueGem;
    //    }
    //    else if(previouseClicked+1 == number)
    //    {
    //        spriteRenderer.sprite = blueGem;
    //    }

    //}
}
