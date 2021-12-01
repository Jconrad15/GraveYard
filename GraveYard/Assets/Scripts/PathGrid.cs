using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public class PathGrid : MonoBehaviour
    {
        [SerializeField]
        private GameObject pathPrefab;

        private GameObject paths;

        private int[] rotations = new int[] { 0, 90, 180, 270 };

        public void AddPathGO(Vector3 position)
        {
            // Create paths parent gameobject
            if (paths == null)
            {
                paths = new GameObject("paths");
                paths.transform.parent = this.transform;
            }

            // Create path decoration
            GameObject decoration = Instantiate(pathPrefab, paths.transform);
            position.y += 0.5f;
            decoration.transform.position = position;

            // Random rotation
            decoration.transform.rotation =
                Quaternion.Euler(0, rotations[Random.Range(0, rotations.Length)], 0);
        }

    }
}