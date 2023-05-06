using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private float distanceBetweenObj; //Distance beetwen more clothes in this container
    [SerializeField] private Transform startingPoint; //Starting point of the container (always 0,0,0)
    [SerializeField] private Transform endingPoint; //Ending point of the container
    [SerializeField] private float clothDimension; //Dimension of the cloth we want to put in

    private float width; //Width of our space we have in this container
    private int maxNumberOfClothes; //Max number of clothes that fit in the container

    void Start()
    {
        //We calculate the width of our space
        width = endingPoint.localPosition.x - startingPoint.localPosition.x;

        //We calculate how many cloths fit
        maxNumberOfClothes = Mathf.FloorToInt(width / clothDimension);
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