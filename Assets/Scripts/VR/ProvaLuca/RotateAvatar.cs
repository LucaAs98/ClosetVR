using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAvatar : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private Transform rig;

    // Update is called once per frame
    void Update()
    {
        this.transform.forward = new Vector3(camera.forward.x, 0, camera.forward.z);
    }


    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(newPos, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.forward);
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(camera.position, camera.forward);
    }
}
