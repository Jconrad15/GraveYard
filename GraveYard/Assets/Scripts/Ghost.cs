using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public class Ghost : MonoBehaviour
    {
        private Vector3 startPosition;

        private float floatDistance;
        private float length;
        float bottomFloor;

        float timeScale;

        void OnEnable()
        {
            startPosition = transform.position;

            timeScale = Random.Range(3f, 5f);
            floatDistance = Random.Range(0.1f, 0.15f);
            // Desired length of the ping-pong
            length = floatDistance * 2;
            // The low position of the ping-pong
            bottomFloor = startPosition.y + floatDistance + (transform.localScale.y / 2f);
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 currentPos = transform.position;
            currentPos.y = Mathf.PingPong(Time.time / timeScale, length) + bottomFloor;
            transform.position = currentPos;
        }
    }
}