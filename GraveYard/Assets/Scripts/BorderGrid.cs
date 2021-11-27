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

        [SerializeField]
        private GameObject pine;

        [SerializeField]
        private GameObject pineCrooked;

        private float treeChance = 0.1f;

        public IEnumerator CreateBorder(int xSize, int zSize)
        {
            border = new GameObject("border");
            border.transform.parent = this.transform;

            // Create one gate per side
            Vector2 gate1 = new Vector2(Random.Range(0, xSize), -1);
            Vector2 gate2 = new Vector2(Random.Range(0, xSize), zSize);
            Vector2 gate3 = new Vector2(-1, Random.Range(0, zSize));
            Vector2 gate4 = new Vector2(xSize, Random.Range(0, zSize));

            for (int x = 0; x < xSize; x++)
            {
                // TODO: FIX and simplify gate code
                // Check for gate
                if (x == gate1.x)
                {
                    // Create gate
                    CreateBorderSection(x, -1, ironFenceBorderGate, 0f);
                    // Create symmetric section
                    CreateBorderSection(x, zSize, ironFenceBorder, 180f);
                    continue;
                }
                else if (x == gate2.x)
                {
                    // Create gate
                    CreateBorderSection(x, zSize, ironFenceBorderGate, 180f);
                    // Create symmetric section
                    CreateBorderSection(x, -1, ironFenceBorder, 0f);
                    continue;
                }

                // Bottom z
                CreateBorderSection(x, -1, ironFenceBorder, 0f);
                
                // Top z
                CreateBorderSection(x, zSize, ironFenceBorder, 180f);

            }

            for (int z = 0; z < zSize; z++)
            {
                // Check for gate
                if (z == gate3.y)
                {
                    // Create gate
                    CreateBorderSection(-1, z, ironFenceBorderGate, 90f);
                    // Create symmetric section
                    CreateBorderSection(xSize, z, ironFenceBorder, -90f);
                    continue;
                }
                else if (z == gate4.y)
                {
                    // Create gate
                    CreateBorderSection(xSize, z, ironFenceBorderGate, -90f);
                    // Create symmetric section
                    CreateBorderSection(-1, z, ironFenceBorder, 90f);
                    continue;
                }

                // Bottom x
                CreateBorderSection(-1, z, ironFenceBorder, 90f);
                
                // Top x
                CreateBorderSection(xSize, z, ironFenceBorder, -90f);

            }

            // Create four corners
            CreateBorderSection(-1, zSize, ironFenceBorderCurve, -90f);
            CreateBorderSection(xSize, zSize, ironFenceBorderCurve, 0f);
            CreateBorderSection(xSize, -1, ironFenceBorderCurve, 90f);
            CreateBorderSection(-1 , -1, ironFenceBorderCurve, 180f);

            yield return null;
        }

        private void CreateBorderSection(int x, int z, GameObject fenceGO, float rotation = 0f)
        {
            // Create border cube
            GameObject borderCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            borderCube.transform.parent = border.transform;
            borderCube.transform.position = new Vector3(x, 0, z);

            borderCube.name = "Border " + x.ToString() + ", " + z.ToString();
            borderCube.GetComponent<MeshRenderer>().material = borderMaterial;

            // remove collider
            Destroy(borderCube.GetComponent<Collider>());

            if (fenceGO == null) { return; }

            // Create border decoration
            GameObject newFence = Instantiate(fenceGO, borderCube.transform);
            Vector3 fencePos = newFence.transform.position;
            fencePos.y += borderCube.transform.localScale.y / 2f;
            newFence.transform.position = fencePos;

            Vector3 fenceRotation = newFence.transform.rotation.eulerAngles;
            fenceRotation.y = rotation;
            newFence.transform.rotation = Quaternion.Euler(fenceRotation);

            // Create tree
            if (Random.value > treeChance) { return; }
            CreateTree(x, z, borderCube);
        }

        private void CreateTree(int x, int z, GameObject borderCube)
        {
            // Choose random pine tree
            GameObject treePrefab = Random.value > 0.5f ? pine : pineCrooked;

            // Create tree decoration
            GameObject newTree = Instantiate(treePrefab, borderCube.transform);
            Vector3 treePos = newTree.transform.position;
            treePos.y += borderCube.transform.localScale.y / 2f;
            newTree.transform.position = treePos;

            // Random rotation
            newTree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        }


    }
}