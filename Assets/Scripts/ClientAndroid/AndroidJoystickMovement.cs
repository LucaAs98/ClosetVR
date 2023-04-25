using UnityEngine;

public class AndroidJoystickMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera camera;
    [SerializeField] private FixedJoystick joystick;

    [SerializeField] private float velocity;

    private void FixedUpdate()
    {
        float x = joystick.Horizontal;
        float z = joystick.Vertical;

        Vector3 move = camera.transform.right * x + camera.transform.forward * z;

        characterController.Move(move * velocity * Time.deltaTime);
    }
}