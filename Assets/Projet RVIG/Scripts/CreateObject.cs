using System.Collections;
using System.Collections.Generic;
using Leap;
using UnityEngine;

public class CreateObject : MonoBehaviour {

    public GameObject modele;
    private GameObject newObject;
    private Renderer renderer;
    private Vector3 position, position_obj;

    private bool canCreate;

    Controller controller;

    private HandList hands;
    // Use this for initialization
    void Start () {
        //modele.SetActive(false);
        controller = new Controller();
    }

    // Update is called once per frame
    void Update () {



        Frame frame = controller.Frame();
        hands = frame.Hands;
        if (frame.Hands.Count == 2)
        {
            //List<Hand> hands = new List<Hand>();

            Hand firstHand = hands[0];
            Hand secondHand = hands[1];
            print("2 hands");
            if (firstHand.SphereRadius < 33 && secondHand.SphereRadius < 33)
            {
                canCreate = true;
                StartCoroutine(Createobject(hands[0]));
            }
        }


       /* if (Input.GetButtonDown("Fire2"))
        {
            position = Input.mousePosition;
            position.z = 8.0f;
            position_obj = Camera.main.ScreenToWorldPoint(position);
            newObject = Instantiate(modele, position_obj, Quaternion.identity) as GameObject;

            renderer = newObject.GetComponent<Renderer>();
            //renderer.material.color = new Color(Random.value, Random.value, Random.value, Random.value);
        }*/
    }

    private IEnumerator Createobject(Hand h)
    {
        yield return new WaitForSeconds(1.0f);
        if (hands[0].SphereRadius > 33 && hands[1].SphereRadius > 33 && canCreate)
        {
            Vector position = h.PalmPosition;
            newObject =
                Instantiate(modele, Vector3.down , Quaternion.identity) as GameObject;
            renderer = newObject.GetComponent<Renderer>();
            canCreate = false;
        }
    }
}
