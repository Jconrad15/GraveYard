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

        [SerializeField]
        private GameObject pumpkinGroup;

        [SerializeField]
        private GameObject debrisCollection;

        private float treeChance = 0.1f;
        private float pumpkinGroupChance = 0.05f;
        private float debrisCollectionChance = 0.1f;

        private int xSize;
        private int zSize;

        public Vector2[] CreateBorder(int xSize, int zSize)
        {
            border = new GameObject("border");
            border.transform.parent = this.transform;

            this.xSize = xSize;
            this.zSize = zSize;

            // Create one gate per side
            Vector2 gate1 = new Vector2(Random.Range(1, xSize - 1), -1);
            Vector2 gate2 = new Vector2(Random.Range(1, xSize - 1), zSize);
            Vector2 gate3 = new Vector2(-1, Random.Range(1, zSize - 1));
            Vector2 gate4 = new Vector2(xSize, Random.Range(1, zSize - 1));

            Vector2[] gates = new Vector2[4] 
            { 
                new Vector2(gate1.x, 0),
                new Vector2(gate2.x, zSize - 1),
                new Vector2(0, gate3.y),
                new Vector2(xSize - 1, gate4.y)
            };

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

            return gates;
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

            // No decor locations
            if (fenceGO == ironFenceBorderCurve ||
                fenceGO == ironFenceBorderGate) 
            { 
                return; 
            }

            // Check create tree
            if (Random.value < treeChance) 
            {
                // Choose random pine tree                
                GameObject placedTree =
                    CreateDecoration(borderCube, Random.value > 0.5f ? pine : pineCrooked);

                // Random rotation
                placedTree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                return;
            }

            // Check create pumpkings
            if (Random.value < pumpkinGroupChance)
            {
                GameObject pumpkin = CreateDecoration(borderCube, pumpkinGroup);
                Vector3 lookAtPos = new Vector3(xSize / 2, pumpkin.transform.position.y, zSize / 2);
                pumpkin.transform.LookAt(lookAtPos);
                return;
            }

            // Check create debris collection
            if (Random.value < debrisCollectionChance)
            {
                GameObject debris = CreateDecoration(borderCube, debrisCollection);
                // Random rotation
                debris.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                // only create one, so set chance to zero
                debrisCollectionChance = 0f;
                return;
            }
            
        }

        private GameObject CreateDecoration(GameObject borderCube, GameObject prefab)
        {
            // Create decoration
            GameObject decoration = Instantiate(prefab, borderCube.transform);
            Vector3 decorationPos = decoration.transform.position;
            decorationPos.y += borderCube.transform.localScale.y / 2f;
            decoration.transform.position = decorationPos;

            return decoration;
        }


    }
}