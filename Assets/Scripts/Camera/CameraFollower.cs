using UnityEngine;

public class CameraFollower : MonoBehaviour 
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.4f;
    private Vector3 offset;
    

    void Awake()
    {
        if(target == null)
            target = FindObjectOfType<BallController>().transform;
        
        offset = transform.position - target.position;
    }

    void Update()
    {
        if (!target)
            return;

        transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothSpeed);
    }
}