using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (_mainCamera != null)
        {
            // Make the object look towards the camera, but flip it by 180 degrees
            Vector3 direction = _mainCamera.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(-direction);
        }
    }
}
