using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToMoveSpecificCloth : MonoBehaviour
{
    //Component where we can find the container of the specific cloth
    private ManageSpecificCloth manageSpecificClothComponent;

    private Transform parentOfSpecificCloth; //Parent of the obj we want to rotate when dragging


    //DragToMove variables
    private Touch screenTouch;
    private float speedModifier = 0.3f;

    void Start()
    {
        manageSpecificClothComponent = this.transform.root.GetComponent<ManageSpecificCloth>();
    }

    //Every frame we check if the person is dragging with his finger
    void Update()
    {
        CheckDragToMove();
    }

    //Check if the client is dragging. When is dragging we change (only in this client) the rotation of the model
    private void CheckDragToMove()
    {
        if (Input.touchCount > 0)
        {
            screenTouch = Input.GetTouch(0);
            if (screenTouch.phase == TouchPhase.Moved)
            {
                parentOfSpecificCloth = manageSpecificClothComponent.GetContainer3dRepresentation();
                //We take the transform of the obj we want to rotate
                Transform objToRotate = parentOfSpecificCloth.GetChild(0);
                objToRotate.Rotate(0f, -screenTouch.deltaPosition.x * speedModifier, 0f);
            }
        }
    }
}