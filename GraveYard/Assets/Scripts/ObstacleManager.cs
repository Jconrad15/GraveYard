using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public class ObstacleManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject obstaclePrefab;

        public readonly float heightOffset = 0.5f;

        public GameObject CreateObstacle(int obstacleStartX, int obstacleStartZ)
        {
            GameObject newObstacle = Instantiate(obstaclePrefab, this.transform);
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