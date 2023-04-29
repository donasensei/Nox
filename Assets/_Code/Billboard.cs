using UnityEngine;

namespace _Code
{
    public class Billboard : MonoBehaviour
    {
        // Billboard
        private Camera mainCamera;

        void Start()
        {
            // Find and assign the main camera
            mainCamera = Camera.main;
        }

        private void Update()
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }
}
