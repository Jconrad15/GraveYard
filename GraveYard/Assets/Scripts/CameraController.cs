using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public class CameraController : MonoBehaviour
    {
        private Camera cam;

        private Vector3 startPos = new Vector3(-3f, 10f, -3f);
        private Vector3 startRot = new Vector3(45f, 45f, 0f);
        private Vector3 targetPos = new Vector3(4.5f, 0, 4.5f);

        private float rotSpeed = 10f;

        void OnEnable()
        {
            cam = GetComponent<Camera>();
            cam.transform.position = startPos;
            cam.transform.rotation = Quaternion.Euler(startRot);
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(targetPos);

            float rotMovement = Input.GetAxisRaw("Horizontal") * rotSpeed;
            cam.transform.Translate(rotMovement * Time.deltaTime * Vector3.right);
        }
    }
}