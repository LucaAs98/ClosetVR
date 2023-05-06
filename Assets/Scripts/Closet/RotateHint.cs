using UnityEngine;

public class RotateHint : MonoBehaviour
{
    [SerializeField] private float rotationVelocity = 2f;
    [SerializeField] private bool upAndDown = true; //Activate or deactivate the up and down animation
    [SerializeField] private float amplitude = 0.001f; //Amplitude of the sin
    [SerializeField] private float frequency = 3; //Frequency of the sin

    private Vector3 initPos; //Init position from where we have to move up and down


    void Start()
    {
        initPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //Rotation around y axis
        transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime * rotationVelocity);
        
        //Up and down animation
        if (upAndDown)
        {
            transform.localPosition = new Vector3(initPos.x, initPos.y + (Mathf.Sin(Time.time * frequency) * amplitude),
                initPos.z);
        }
    }
}