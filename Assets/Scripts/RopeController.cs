using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class responsible for rope drawing between gems and fade-in effect
public class RopeController : MonoBehaviour
{
    //Stores ropes that need to be faded in
    public Queue<LineRenderer> ropes = new Queue<LineRenderer>();

    //Variable used to determine if fade-in coroutine is running
    public bool fadeInRunning = false;

    //Rope prefab consisting of empty game object with LineRenderer component attached is selected from inspector
    public GameObject ropePrefab;

    //Number of segments each rope consists of
    //Each rope has multiple segments to achieve a more consistent and gradual fade-in transition
    public float ropeSegments = 10f;

    //Determines fade-in duration of a rope (1f = 1 second)
    private float fadeDuration = 0.6f;

    //Method creates rope objects, sets their LineRenderer positions in between given parameter start and end Vectors
    //Adds created rope to fade-in queue
    //Attempts to start fade-in process if it is not running
    public void CreateRope(Vector2 startPos, Vector2 endPos)
    {
        //Creates new rope GameObject based on prefab at default location, as rope is drawn from LineRenderer position
        //Individual ropes over one containing all points was also chosen to keep
        LineRenderer rope = Instantiate(ropePrefab, transform).GetComponent<LineRenderer>();
        //Vertex count is one higher than edge for a line
        rope.positionCount = (int)ropeSegments + 1;

        //Sets all rope points
        for (float i = 0; i <= ropeSegments; i++)
        {
            //Rope midpoints are calculated using this formula:
            //x (fraction of point to be found) = x (start coordinate) * (fraction of point to be found) + x(end coordinate) * (inverted fraction of point to be found)
            //Example: x(1/3)= start x * (2/3)+ end x * (1/3)
            //Same applies to Y axis
            float xPosChange = startPos.x * ((ropeSegments - i) / ropeSegments) + endPos.x * (i / ropeSegments)
                , yPosChange = startPos.y * ((ropeSegments - i) / ropeSegments) + endPos.y * (i / ropeSegments);

            //Create vector2 variable from midpoint coordinates and set it as a position in LineRenderer
            Vector2 ropeSegmentPos = new Vector2(xPosChange, yPosChange);
            rope.SetPosition((int)i, ropeSegmentPos);
        }
        //After rope with its positions is created, add it to queue for fade-in
        ropes.Enqueue(rope);
        //Attempt to start rope fade-in animation
        FadeInRopes();
    }

    //Starts coroutine responsible for rope fade-in if it is not running
    public void FadeInRopes()
    {
        if (!fadeInRunning)
        {
            StartCoroutine(FadeInLineRenderer());
        }
    }

    //Fade in rope LineRenderer using gradient keys over time
    private IEnumerator FadeInLineRenderer()
    {
        //Set coroutine as running to avoid multiple instances at the same time
        fadeInRunning = true;

        //Run coroutine until there are elements in queue remaining
        while (ropes.Count != 0)
        {
            //Take out first added element from the queue
            LineRenderer rope = ropes.Dequeue();
            Gradient lineRendererGradient = new Gradient();
            float timeElapsed = 0f;
            float alpha;

            while (timeElapsed < fadeDuration)
            {
                //Sets alpha value from 0f to 1f based on fraction of time elapsed compared to fade duration
                alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);

                lineRendererGradient.SetKeys
                (
                    //Color keys remain the same
                    rope.colorGradient.colorKeys,
                    //Gradient is also split into multiple segments, to make fade-in transition smoother
                    //As alpha value are (0 =< Alpha <= 1) their powers are lowered until Alpha=1
                    new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0f), new GradientAlphaKey(alpha * alpha, 0.5f), new GradientAlphaKey(alpha * alpha * alpha, 1f) }
                );
                rope.colorGradient = lineRendererGradient;

                //Increments time
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        fadeInRunning = false;

        //After all ropes are faded in, checks if last clicked gem is last in the level
        //Level is completed when last rope fades in to first gem
        //Set this level as completed
        GameManager.GM.SetLevelCompleted();
    }
}