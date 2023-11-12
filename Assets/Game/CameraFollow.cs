using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target; 

    [SerializeField]
    private float smoothSpeed = 0.125f;

    [SerializeField]
    private Vector3 offset; 

    void Update()
    {
       var newPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, -10f);
       transform.position = Vector3.Slerp(transform.position, newPosition, smoothSpeed*Time.deltaTime);
    }
}
