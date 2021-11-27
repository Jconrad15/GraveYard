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

        [SerializeField]
        private GameObject ironFenceBorder;

        [SerializeField]
        private GameObject ironFenceBorderGate;

        [SerializeField]
        private GameObject ironFenceBorderCurve;

        public IEnumerator CreateBorder(int xSize, int zSize)
        {
            border = new GameObject("border");
            border.transform.parent = this.transform;

            for (int x = 0; x < xSize; x++)
            {
                // Bottom z
                CreateBorderSection(x, -1, ironFenceBorder, 180f);
                // Top z
                CreateBorderSection(x, zSize, ironFenceBorder, 0f);
            }

            for (int z = 0; z < zSize; z++)
            {
                // Bottom x
                CreateBorderSection(-1, z, ironFenceBorder, -90f);
                // Top x
                CreateBorderSection(xSize, z, ironFenceBorder, 90f);
            }
            yield return null;
        }

        private void CreateBorderSection(int x, int z, GameObject fenceGO, float rotation)
        {
            // Create border cube
            GameObject borderCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            borderCube.transform.parent = border.transform;
            borderCube.transform.position = new Vector3(x, 0, z);

            borderCube.name = "Border " + x.ToString() + ", " + z.ToString();
            borderCube.GetComponent<MeshRenderer>().material = borderMaterial;

            // remove collider
            Destroy(borderCube.GetComponent<Collider>());

            // Create border decoration
            GameObject newFence = Instantiate(fenceGO, borderCube.transform);
            Vector3 fencePos = newFence.transform.position;
            fencePos.y += borderCube.transform.localScale.y / 2f;
            newFence.transform.position = fencePos;

            Vector3 fenceRotation = newFence.transform.rotation.eulerAngles;
            fenceRotation.y = rotation;
            newFence.transform.rotation = Quaternion.Euler(fenceRotation);

        }
    }
}