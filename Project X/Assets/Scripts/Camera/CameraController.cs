using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float Speed;

    private float _inputX;
    private float _inputY;

    private Vector3 _movement;

    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

    private Vector3 _newPosition;

    private void Update()
    {
        //get input from keyboard axes (WASD/Arrow Keys)
        _inputX = Input.GetAxis("Horizontal");
        _inputY = Input.GetAxis("Vertical");

        _movement = new Vector3(_inputX, _inputY, 0) * Speed * Time.deltaTime;

        //calculate the new position
        _newPosition = transform.position + _movement;

        //clamp the new position within the defined boundaries
        _newPosition.x = Mathf.Clamp(_newPosition.x, minX, maxX);
        _newPosition.y = Mathf.Clamp(_newPosition.y, minY, maxY);

        //apply the bounded position to the camera's transform
        transform.position = _newPosition;
    }

}
