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

        private float rotAmount = 90f;

        private bool canRotate;
        private float rotCooldownTime = 0.3f;

        void OnEnable()
        {
            cam = GetComponent<Camera>();
            cam.transform.position = startPos;
            cam.transform.rotation = Quaternion.Euler(startRot);
            canRotate = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (canRotate == true)
            {
                transform.LookAt(targetPos);

                float rotDirection = Input.GetAxisRaw("Horizontal");

                if (rotDirection != 0)
                {
                    cam.transform.RotateAround(targetPos, Vector3.up, -rotDirection * rotAmount);
                    canRotate = false;
                    StartCoroutine(RotationCoolDown());
                }
            }
        }

        private IEnumerator RotationCoolDown()
        {
            yield return new WaitForSeconds(rotCooldownTime);
            canRotate = true;
        }
    }
}