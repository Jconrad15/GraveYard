using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public enum Direction { N, E, S, W };

    public class MapGrid : MonoBehaviour
    {
        private int xSize = 10;
        private int zSize = 10;

        private Cell[] cells;
        private GameObject[] cellObjects;

        [SerializeField]
        private Material cellMaterial;

        [SerializeField]
        private PlayerController playerController;

        public void InitializeGrid()
        {
            GenerateMap();
            DisplayMap();

            // Create player starting locations
            CreatePlayerStartCharacter();

            // Create Enemy starting location
            int enemyStartX = xSize - 2;
            int enemyStartZ = zSize - 2;
            //PlaceAtCell(  ENEMY  )


            // Create obstacles

        }

        private void CreatePlayerStartCharacter()
        {
            int playerStartX = 1;
            int playerStartZ = 1;

            GameObject player = playerController.CreateCharacter();

            Vector3 playerPos = player.transform.position;
            playerPos.x = playerStartX;
            playerPos.y = playerController.heightOffset;
            playerPos.z = playerStartZ;
            player.transform.position = playerPos;

            PlaceAtCell(GetCell(playerStartX, playerStartZ),
                        player,
                        ObjectType.Player);
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

        private bool IsCellOpen(Vector3 selectedLocation)
        {
            Cell selectedCell = GetCell((int)selectedLocation.x, (int)selectedLocation.z);
            if (selectedCell == null) { return false; }

            return selectedCell.IsOpen;
        }

        public bool TryPlaceObject(GameObject characterGO, ObjectType objType)
        {
            Vector3 selectedLocation = characterGO.transform.position;

            // First check if the cell is open
            if (IsCellOpen(selectedLocation) == false) { return false; }

            Cell selectedCell = GetCell(
                (int)characterGO.transform.position.x, (int)characterGO.transform.position.z);

            if (selectedCell == null)
            {
                Debug.LogError("Placing at null cell");
                return false;
            }

            // Check if cell is neighboring a correct character cell
            if (CheckNeighborObjectType(selectedCell, objType) == false) { return false; }

            // Place the character gameobject
            PlaceAtCell(selectedCell, characterGO, objType);
            return true;
        }

        private Cell GetCell(int x, int z)
        {
            // Check out of bounds
            if (x > xSize || x < 0 || z > zSize || z < 0) { return null; }

            int index = (x * zSize) + z;

            // TODO: maybe delete following line, redundant
            if (index < 0 || index > cells.Length) { Debug.Log("First check didn't catch this."); return null; }

            return cells[index];
        }

        private Cell GetNeighbor(Cell cell, Direction direction)
        {
            int x = (int)cell.position.x;
            int z = (int)cell.position.z;

            switch (direction)
            {
                case Direction.N:
                    return GetCell(x, z + 1);                    

                case Direction.E:
                    return GetCell(x + 1, z);

                case Direction.S:
                    return GetCell(x, z - 1);

                case Direction.W:
                    return GetCell(x - 1, z);

                default:
                    return null;
            }
        }

        private void PlaceAtCell(Cell selectedCell, GameObject characterGO, ObjectType objType)
        {
            selectedCell.SetClosed(objType);

            // Set parent
            characterGO.transform.parent = cellObjects[selectedCell.index].transform;
        }

        /// <summary>
        /// Returns true if the cell has a neighbor with the same objectType 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        private bool CheckNeighborObjectType(Cell cell, ObjectType objType)
        {
            // Check each neighbor for objType
            for (int i = 0; i < 4; i++)
            {
                Cell neighbor = GetNeighbor(cell, (Direction)i);
                if (neighbor.objectType == objType) { return true; }
            }

            return false;
        }

    }
}