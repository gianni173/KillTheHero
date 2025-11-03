using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float Speed;

    private float _inputX;
    private float _inputY;

    private Vector3 _movement;

    private void Update()
    {
        //get input from keyboard axes (WASD/Arrow Keys)
        _inputX = Input.GetAxis("Horizontal");
        _inputY = Input.GetAxis("Vertical");

        //calculate movement direction
        _movement = new Vector3(_inputX, _inputY, 0);

        // Move the camera
        transform.position += _movement * Speed * Time.deltaTime;
    }

}
