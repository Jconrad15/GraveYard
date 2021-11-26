using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public class Zombie : MonoBehaviour
    {
        private Transform leftLeg;
        private Transform rightLeg;

        private float length;
        float bottomFloor;

        float timeScale;

        void OnEnable()
        {
            timeScale = Random.Range(40f, 50f);
            length = 60f;
            bottomFloor = 0 - (length / 2f);

            leftLeg = transform.Find("legLeft");
            rightLeg = transform.Find("legRight");
        }

        // Update is called once per frame
        void Update()
        {
            // Left Leg
            float leftRotaionAmount = Mathf.PingPong(Time.time * timeScale, length) + bottomFloor;
            leftLeg.rotation = Quaternion.Euler(new Vector3(leftRotaionAmount, 0, 0)); ;

            // Right Leg
            float rightRotaionAmount = -1 * (Mathf.PingPong(Time.time * timeScale, length) + bottomFloor);
            rightLeg.rotation = Quaternion.Euler(new Vector3(rightRotaionAmount, 0, 0));
        }
    }
}