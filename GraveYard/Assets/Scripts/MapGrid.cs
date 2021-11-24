using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public class MapGrid : MonoBehaviour
    {
        private int xSize = 10;
        private int zSize = 10;

        private Cell[] cells;
        private GameObject[] cellObjects;

        [SerializeField]
        private Material cellMaterial;

        public void InitializeGrid()
        {
            GenerateMap();
            DisplayMap();
        }

        private void GenerateMap()
        {
            cells = new Cell[xSize * zSize];

            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    int index = (x * zSize) + z;
                    cells[index] = new Cell(index, new Vector3(x, 0, z));
                }
            }

        }

        private void DisplayMap()
        {
            cellObjects = new GameObject[cells.Length];

            for (int i = 0; i < cells.Length; i++)
            {
                GameObject cell_go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cell_go.transform.parent = this.transform;
                cell_go.transform.position = cells[i].position;

                cell_go.name = "Cell " + i.ToString();

                cell_go.GetComponent<MeshRenderer>().material = cellMaterial;

                cellObjects[i] = cell_go;
            }
        }

        public bool IsCellOpen(Vector3 selectedLocation)
        {
            Cell selectedCell = GetCell((int)selectedLocation.x, (int)selectedLocation.z);
            if (selectedCell == null) { return false; }

            return selectedCell.isOpen;
        }

        private Cell GetCell(int x, int z)
        {
            int index = (x * zSize) + z;
            return cells[index];
        }

        public void PlaceAtCell(GameObject go)
        {


        }

    }
}