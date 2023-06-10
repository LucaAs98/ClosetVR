using UnityEngine;

//Put this script in te object you want to rotate with drag
public class DragToMoveSpecificCloth : MonoBehaviour
{
    [SerializeField] private RectTransform touchArea; //Touch area we want to use for dragging

    private Touch screenTouch;
    private float speedModifier = 0.3f;

    private Quaternion startingRotation;

    //Saving the starting rotation for a future reset of it
    void Start()
    {
        startingRotation = this.transform.rotation;
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

            //Check if the touch is inside the specified rect
            if (RectTransformUtility.RectangleContainsScreenPoint(touchArea, screenTouch.position))
            {
                if (screenTouch.phase == TouchPhase.Moved)
                {
                    this.transform.Rotate(0f, -screenTouch.deltaPosition.x * speedModifier, 0f);
                }
            }
        }
    }


    //Useful for the specific cloth view. Closing it will reset the rotation of his mannequin
    public void ResetObjRotation()
    {
        this.transform.rotation = startingRotation;
    }
}