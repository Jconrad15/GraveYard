using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public class BorderGrid : MonoBehaviour
    {
        private GameObject border;

        [SerializeField]
        private Material borderMaterial;

        public IEnumerator CreateBorder(int xSize, int zSize)
        {
            border = new GameObject("border");
            border.transform.parent = this.transform;

            for (int x = -1; x < xSize + 1; x++)
            {
                // Bottom z
                CreateBorderCube(x, -1);
                // Top z
                CreateBorderCube(x, zSize);
            }

            for (int z = 0; z < zSize; z++)
            {
                // Bottom x
                CreateBorderCube(-1, z);
                // Top x
                CreateBorderCube(xSize, z);
            }
            yield return null;
        }

        private void CreateBorderCube(int x, int z)
        {
            GameObject borderCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            borderCube.transform.parent = border.transform;
            borderCube.transform.position = new Vector3(x, 0, z);

            borderCube.name = "Border " + x.ToString() + ", " + z.ToString();

            borderCube.GetComponent<MeshRenderer>().material = borderMaterial;
        }
    }
}