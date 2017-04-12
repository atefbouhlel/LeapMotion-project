using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script simplifié pour RVIG
/// Fournit la position d'une main (1ère main gauche ou droite reconnue) à chaque frame grâce au HandController
/// Utilise cette position pour déplacer l'objet courant
/// Une touche permet d'afficher/masquer la main virtuelle d'origine
/// </summary>
public class Virtual3DTrackerLeap : MonoBehaviour
{
    //leap motion controller
    [Tooltip("Must be in the scene")] public HandController handController;

    private Hand hand;
    private HandModel handModel;
    private bool isSelected;
    private bool canGrab;
    private bool sayab;

    private bool canCreate;
    private float diff ;
    private int x;

    public Vector3 Position { get; protected set; }

    public Quaternion Rotation { get; set; }

    //visible default leap hand model attributes
    public KeyCode visibleHandKey = KeyCode.V;

    public bool defaultVisibleHand = true;
    private bool visibleHand;


    protected void Start()
    {
        isSelected = false;
        canGrab = false;
        sayab = false;
        canCreate = false;
        Position = new Vector3();
        Position = Vector3.zero;
        visibleHand = defaultVisibleHand;
        Rotation = new Quaternion();
        

        x = 0;
        diff = 0.0f;
    }

    protected void Update()
    {
        if (handController.GetAllGraphicsHands().Length != 0)
        {
            handModel = handController.GetAllGraphicsHands()[0];
            handModel.transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = visibleHand;
            hand = handModel.GetLeapHand();
        }

    }
    
    
    void OnTriggerEnter(Collider other)
    {
        print("enter");
        if (hand != null)
        {
            print("!null");
            if (hand.SphereRadius < 33 && !sayab)
            {
                isSelected = true;
                print("msakra");
                StartCoroutine(SelectCoroutine());
            }
        }
        else
        {
            print("null");
        }
    }

    

    private IEnumerator SelectCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        if (isSelected && hand.SphereRadius > 33)
            canGrab = true;

        
        
        while (canGrab )
        {
            this.transform.position = handModel.GetPalmPosition();
            this.transform.rotation = handModel.GetPalmRotation();
            if (hand.SphereRadius < 33)
            {
                canGrab = false;
                isSelected = false;
                sayab = true;
                this.transform.position = handModel.GetPalmPosition();
                this.transform.rotation = handModel.GetPalmRotation();
            }
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);
        sayab = false;
        //StopCoroutine(TestCoroutine());
    }

    protected void UpdateTracker()
    {
        //get the 1st hand in the frame
        if (handController.GetAllGraphicsHands().Length != 0)
        {
            handModel = handController.GetAllGraphicsHands()[0];
            handModel.transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = visibleHand;
            hand = handModel.GetLeapHand();
            Position = handModel.GetPalmPosition();
            Rotation = handModel.GetPalmRotation();
            

            // Déplacer l'objet avec la main ouverte
            if (isSelected && hand.GrabStrength <= 0.11f)
            {
                //Debug.Log("BeginGrab");
                this.transform.position = Position;
                this.transform.rotation = Rotation;
            }
            else if (isSelected && hand.GrabStrength >= 0.95f) // Déselectionner l'objet et le déposer
            {   
                //Debug.Log("PauseGrab");
                isSelected = false;
                this.transform.position = Position;
                this.transform.rotation = Rotation;

            }
            //Sélectionner un objet
            // Test sur la collision de la main (ou souris) avec un objet

            // Déselectionner un objet
            /* if (hand.GrabStrength >= 0.95f)
             {
                 if (isSelected)
                 {
                     Debug.Log("selectG");
                     isSelected = false;
                     this.transform.position = Position;
                     this.transform.rotation = Rotation;
                 }
                 else
                 {
                     Debug.Log("nselectG");
                     // Test sur la collision de la main (ou souris) avec un objet
                     if (this.transform.position == Position)
                     {
                         isSelected = true;
                     }
                 }
             }

             if (isSelected)
             {
                 Debug.Log("select");
                 this.transform.position = Position;
                 this.transform.rotation = Rotation;
             }*/
        }
        /*else if (handController.GetAllGraphicsHands().Length == 2)
        {
            Debug.Log("okey");
            handModel1 = handController.GetAllGraphicsHands()[0];
            handModel.transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = visibleHand;
            hand1 = handModel.GetLeapHand();
            Position1 = handModel.GetPalmPosition();
        }*/
        //mask/display the graphical hand on key down
        if (Input.GetKeyDown(visibleHandKey))
            {
                var smr = handModel.transform.GetComponentInChildren<SkinnedMeshRenderer>();
                visibleHand = !visibleHand;
            }
        
    }
}