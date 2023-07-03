using UnityEngine;

public class ChangeLayerRecursively : MonoBehaviour
{
    private string layerName; // The name of the layer to propagate

    void Awake()
    {
        //Get the layer of the GameObject
        int layer = gameObject.layer;

        //Get the layer name from the layer index
        layerName = LayerMask.LayerToName(layer);

        //Change the layer of all the children of the parameter "transform" with the same of him
        ChangeLayerRecursive(transform);
    }

    //Change the layer of all the children of the parameter "parent" with the same of the parent
    void ChangeLayerRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Change the layer of the current child object
            child.gameObject.layer = LayerMask.NameToLayer(layerName);

            // Recursively call the function for all children of the current child object
            ChangeLayerRecursive(child);
        }
    }
}