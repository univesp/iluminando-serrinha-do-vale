using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void Update()
    {
        if(transform.position.x >= 0 && transform.position.x <= 19.3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_target.position.x, transform.position.y, transform.position.z), 0.3f);            
        }

        if(transform.position.x < 0)
        {
            transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
        }
        if(transform.position.x > 19.3f)
        {
            transform.position = new Vector3(19.3f, transform.position.y, transform.position.z);
        }
    }
}
