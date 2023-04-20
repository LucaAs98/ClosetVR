using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDiTest : MonoBehaviour
{

    void FixedUpdate()
    {
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,
            this.gameObject.transform.position.y + 0.01f, this.gameObject.transform.position.z);
    }
}
