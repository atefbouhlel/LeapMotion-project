using System.Collections;
using System.Collections.Generic;
using Leap;
using UnityEngine;

public class Swipe : MonoBehaviour {

	Controller controller;
    public GameObject Object1;
    public GameObject Object2;
    public GameObject Object3;
    public GameObject Object4;

    private int index;
    private List<GameObject> TabObjects;
    private float _lastGestureTime;
    private float _lastCircleGestureTime;
    

    private GameObject newObject;
    private Renderer renderer;
    void Start ()
    {
		controller = new Controller();
        controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
        controller.Config.SetFloat("Gesture.Swipe.MinLength", 200.0f);
        controller.Config.SetFloat("Gesture.Swipe.MinVelocity", 750f);
        controller.Config.Save();

        

        TabObjects = new List<GameObject>();
        TabObjects.Add(Object1);
        TabObjects.Add(Object2);
        TabObjects.Add(Object3);
        TabObjects.Add(Object4);

        index = 0;
        //Object1.SetActive(false);
        Object2.SetActive(false);
        Object3.SetActive(false);
        Object4.SetActive(false);
    }

    private IEnumerator SwipeToLeft()
    {
        TabObjects[index].SetActive(false);
        yield return new WaitForSeconds(.5f);

        if (index > 2) index = -1;
        index++;
        TabObjects[index].SetActive(true);
    }

    private IEnumerator SwipeToRight()
    {
        TabObjects[index].SetActive(false);
        yield return new WaitForSeconds(.5f);
        index--;
        if (index < 0) index = 3;
        TabObjects[index].SetActive(true);
    }

    void Update()
    {
        Frame frame = controller.Frame();
        if (frame.Hands.Count == 1)
        {
            print("hand = 1");
            GestureList gestures = frame.Gestures();
            Hand hand = frame.Hands.Frontmost;
            if (hand.IsRight)
            {
                for (int i = 0; i < gestures.Count; i++)
                {
                    Gesture gesture = gestures[i];
                    if (gesture.Type == Gesture.GestureType.TYPE_SWIPE)
                    {
                        SwipeGesture swipeGesture = new SwipeGesture(gesture);
                        Vector swipeDirection = swipeGesture.Direction;
                        if (Time.time - _lastGestureTime > 0.75f && swipeDirection.x < 0)
                        {
                            Debug.Log("left" );
                            _lastGestureTime = Time.time;
                            StartCoroutine(SwipeToLeft());
                        }
                        if (Time.time - _lastGestureTime > 0.75f && swipeDirection.x > 0)
                        {
                            Debug.Log("right");
                            _lastGestureTime = Time.time;
                            StartCoroutine(SwipeToRight());
                        }
                    }
                }
            }
        }
    }

}
