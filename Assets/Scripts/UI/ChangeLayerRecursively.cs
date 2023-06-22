using UnityEngine;

public class ChangeLayerRecursively : MonoBehaviour
{
    private string layerName; // The name of the layer to propagate

    void Start()
    {
        // Get the layer of the GameObject
        int layer = gameObject.layer;

        // Get the layer name from the layer index
        layerName = LayerMask.LayerToName(layer);

        ChangeLayerRecursive(transform);
    }

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