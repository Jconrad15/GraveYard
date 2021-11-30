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

        public void AddPath(Vector3 position)
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
        }

    }
}