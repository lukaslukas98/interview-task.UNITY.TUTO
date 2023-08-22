using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{

    public Queue<LineRenderer> ropes= new Queue<LineRenderer>();

    public bool fadeInRunning = false;
    public GameObject ropePrefab;
    public void CreateRope(Vector2 startPos, Vector2 endPos)
    {
        LineRenderer rope = Instantiate(ropePrefab,transform).GetComponent<LineRenderer>();
        rope.positionCount = 11;

        //rope.SetPosition(0, startPos);
        //rope.SetPosition(1, endPos);



        rope.SetPosition(0, startPos);
        for (float i = 1; i <= 10; i++)
        {
            float xPosChange = startPos.x* ((10 - i) / 10f) + endPos.x * (i / 10f)
                , yPosChange = startPos.y * ((10 - i) / 10f) + endPos.y * (i / 10f);

            Vector2 ropeSegmentPos = new Vector2(xPosChange, yPosChange);
            rope.SetPosition((int)i, ropeSegmentPos);
        }
        ropes.Enqueue(rope);
        FadeInRopes();
    }


    public void FadeInRopes()
    {
        if (!fadeInRunning)
        {
            StartCoroutine(FadeInLineRenderer());
        }
    }

    IEnumerator FadeInLineRenderer()
    {
        fadeInRunning = true;
        while (ropes.Count != 0)
        {
            LineRenderer rope = ropes.Dequeue();
            Gradient lineRendererGradient = new Gradient();
            float fadeSpeed = 0.6f;
            float timeElapsed = 0f;
            float alpha = 0f;

            while (timeElapsed < fadeSpeed)
            {
                alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeSpeed);


                lineRendererGradient.SetKeys
                (
                    rope.colorGradient.colorKeys,
                    new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0f),new GradientAlphaKey(alpha * alpha, 0.5f),new GradientAlphaKey(alpha* alpha * alpha, 1f) }
                );
                rope.colorGradient = lineRendererGradient;

                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        fadeInRunning = false;
            if (GameManager.GM.lastClicked == GameManager.GM.gems.Count)
            {
                GameManager.GM.uiController.ShowLevelCompletedPanel();
            }
        }
}
