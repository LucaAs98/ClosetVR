using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class Container : MonoBehaviour
{
    [Range(0.1f, 3)] [SerializeField]
    private float distanceBetweenObj = 1; //Distance beetwen more clothes in this container

    [Range(0f, 10)] [SerializeField] private float startPadding = 0.2f; //Real startingPoint
    [Range(0.1f, 10)] [SerializeField] private float endPadding = 0.2f; //Real endingPoint

    [SerializeField] private Transform startingPoint; //Starting point of the container (always 0,0,0)
    [SerializeField] private Transform endingPoint; //Ending point of the container

    [SerializeField] private bool showGizmo; //Show Gizmo in scene

    private float width; //Width of our space we have in this container
    private int maxNumberOfClothes; //Max number of clothes that fit in the container

    void Start()
    {
        //Start padding cannot be greater then the end padding
        if (startPadding < endPadding)
        {
            //Calculation of the container's width
            width = endPadding - startPadding;

            //Calculation of how many cloths fit (+1 because it starts as far to the left as possible)
            maxNumberOfClothes = Mathf.FloorToInt(width / distanceBetweenObj) + 1;
        }
        else
        {
            Debug.LogError("Start padding cannot be greater then the end padding");
        }
    }

    //Used for the visualization of the container's limiters
    private void OnDrawGizmos()
    {
        Vector3 startLocalPos = startingPoint.localPosition;
        startingPoint.localPosition = new Vector3(startPadding, startLocalPos.y, startLocalPos.z);

        Vector3 endLocalPos = endingPoint.localPosition;
        endingPoint.localPosition = new Vector3(endPadding, endLocalPos.y, endLocalPos.z);

        if (showGizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(startingPoint.position, 0.03f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(endingPoint.position, 0.03f);
        }
    }

    //---------- GET ----------
    public float GetDistanceBetweenObjInContainer()
    {
        return distanceBetweenObj;
    }

    public int GetMaxNumberOfClothes()
    {
        return maxNumberOfClothes;
    }

    public Transform GetStartingPoint()
    {
        return startingPoint;
    }
}