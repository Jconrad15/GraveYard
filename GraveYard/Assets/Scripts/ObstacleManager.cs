using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public class ObstacleManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject rocksTallPrefab;

        [SerializeField]
        private GameObject cryptPrefab;

        public readonly float heightOffset = 0.5f;

        public GameObject CreateObstacle(int obstacleStartX, int obstacleStartZ)
        {
            // Random of two obstacles
            GameObject selectedPrefab = Random.value > 0.5 ? rocksTallPrefab : cryptPrefab;

            GameObject newObstacle = Instantiate(selectedPrefab, this.transform);
            newObstacle.name = "newObstacle";

            Vector3 obstaclePos = newObstacle.transform.position;
            obstaclePos.x = obstacleStartX;
            obstaclePos.y = heightOffset;
            obstaclePos.z = obstacleStartZ;
            newObstacle.transform.position = obstaclePos;

            return newObstacle;
        }


    }
}