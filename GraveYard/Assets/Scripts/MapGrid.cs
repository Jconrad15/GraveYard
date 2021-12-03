using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public enum Direction { N, E, S, W };

    public struct MapData
    {
        public MapData(int totalCells, int obstacleCells, int neutralCharacterCount)
        {
            this.totalCells = totalCells;
            this.obstacleCells = obstacleCells;
            this.controllableCells = totalCells - obstacleCells;
            this.neutralCharacterCount = neutralCharacterCount;
        }
        public int totalCells;
        public int controllableCells;
        public int obstacleCells;
        public int neutralCharacterCount;
    }

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

        [SerializeField]
        private EnemyController enemyController;

        [SerializeField]
        private NeutralController neutralController;

        [SerializeField]
        private ObstacleManager obstacleManager;

        [SerializeField]
        private BorderGrid borderGrid;

        [SerializeField]
        private PathGrid pathGrid;

        public MapData InitializeGrid()
        {
            GenerateMap();
            DisplayMap();

            Vector2[] gates = borderGrid.CreateBorder(xSize, zSize);

            // Create player starting locations
            CreatePlayerStartCharacter();

            // Create Enemy starting location
            CreateEnemyStartCharacter();

            // Create obstacles
            int obstacleCount = CreateObstacles();

            // Create neutral charcter starting locations and paths
            int neutralCharacterCount = CreateNeutral(gates);

            return new MapData(xSize * zSize, obstacleCount, neutralCharacterCount);
        }

        private int CreateObstacles()
        {
            int obstacleStartX = 6;
            int obstacleStartZ = 4;

            GameObject obstacle = obstacleManager.CreateObstacle(obstacleStartX, obstacleStartZ);

            PlaceAtCell(GetCell(obstacleStartX, obstacleStartZ),
                        obstacle,
                        ObjectType.Obstacle);

            // Create second obstacle
            obstacleStartX = 3;
            obstacleStartZ = 5;

            obstacle = obstacleManager.CreateObstacle(obstacleStartX, obstacleStartZ);

            PlaceAtCell(GetCell(obstacleStartX, obstacleStartZ),
                        obstacle,
                        ObjectType.Obstacle);

            return 2;
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

        private void CreateEnemyStartCharacter()
        {
            int enemyStartX = xSize - 2;
            int enemyStartZ = zSize - 2;

            GameObject enemy = enemyController.CreateCharacter();

            Vector3 enemyPos = enemy.transform.position;
            enemyPos.x = enemyStartX;
            enemyPos.y = enemyController.heightOffset;
            enemyPos.z = enemyStartZ;
            enemy.transform.position = enemyPos;

            Cell cell = GetCell(enemyStartX, enemyStartZ);
            PlaceAtCell(cell, enemy, ObjectType.Enemy);

            // Also add enemy location to enemy controller
            enemyController.placedLocations.Add(cell);
        }

        private int CreateNeutral(Vector2[] gates)
        {
            // Select opposite gates to connect for path 1
            int startGate = 0;
            int endGate = 1;

            Cell[] path = CreateNeutralPath(gates[startGate], gates[endGate]);

            Cell cell = GetCell((int)gates[startGate].x, (int)gates[startGate].y);
            PlaceAtCell(
                cell,
                neutralController.CreateCharacter((int)gates[startGate].x, (int)gates[startGate].y, path, cell),
                ObjectType.Neutral);

            // Select opposite gates to connect for path 2
            startGate = 3;
            endGate = 2;

            path = CreateNeutralPath(gates[startGate], gates[endGate]);
            Cell cell2 = GetCell((int)gates[startGate].x, (int)gates[startGate].y);
            PlaceAtCell(
                cell2,
                neutralController.CreateCharacter((int)gates[startGate].x, (int)gates[startGate].y, path, cell2),
                ObjectType.Neutral);

            return 2;
        }

        private Cell[] CreateNeutralPath(Vector2 startGate, Vector2 endGate)
        {
            Cell currentCell = GetCell((int)startGate.x, (int)startGate.y);

            List<Cell> path = new List<Cell>();

            path.Add(currentCell);
            pathGrid.AddPathGO(currentCell.position);
            currentCell.SetAsPath();

            Direction direction;
            List<Direction> directions = new List<Direction>();

            int outerAbortCounter = 0;
            while (true)
            {
                outerAbortCounter += 1;
                if (outerAbortCounter > 5000) { Debug.LogWarning("OuterAbort"); break; }

                // Done break condition
                if (currentCell.position.x == endGate.x)
                {
                    if (currentCell.position.z == endGate.y)
                    {
                        break;
                    }
                }

                float xDirection = endGate.x - currentCell.position.x;
                float zDirection = endGate.y - currentCell.position.z;

                // determine directions towards gate
                directions.Clear();
                if (xDirection > 0)
                {
                    directions.Add(Direction.E);
                }
                else if (xDirection < 0)
                {
                    directions.Add(Direction.W);
                }

                if (zDirection > 0)
                {
                    directions.Add(Direction.N);
                }
                else if (zDirection < 0)
                {
                    directions.Add(Direction.S);
                }

                int innerAbortCounter = 0;
                bool isCellSelected = false;
                while (isCellSelected == false) 
                {
                    innerAbortCounter += 1;
                    if (innerAbortCounter > 2500) 
                    {
                        // Try other directions
                        directions.Clear();
                        directions.Add(Direction.N);
                        directions.Add(Direction.E);
                        directions.Add(Direction.S);
                        directions.Add(Direction.W);

                        // If this still doesn't work, break badly
                        if (innerAbortCounter > 5000)
                        {
                            Debug.LogError("why can't we make a path?");
                            break;
                        }
                    }

                    // Get random x or z direction towards the gate
                    direction = directions[Random.Range(0, directions.Count)];

                    Cell neighbor = GetNeighbor(currentCell, direction);

                    // If the cell doesn't exist, try again.
                    if (neighbor == null) { continue; }

                    // If the path already contains the cell, try again.
                    if (path.Contains(neighbor)) { continue; }

                    // If the cell includes something, try again.
                    if (neighbor.IsOpen == false) { continue; }

                    // Otherwise add cell to path
                    path.Add(neighbor);
                    pathGrid.AddPathGO(neighbor.position);
                    isCellSelected = true;

                    // Set new current cell
                    currentCell = neighbor;
                }
            }

            return path.ToArray();
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
            if (x > xSize - 1 || x < 0 || z > zSize - 1 || z < 0) { return null; }

            int index = (x * zSize) + z;

            if (index < 0 || index >= cells.Length) { Debug.Log("First check didn't catch this."); return null; }

            return cells[index];
        }

        public Cell GetNeighbor(Cell cell, Direction direction)
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

        public void PlaceAtCell(Cell selectedCell, GameObject characterGO, ObjectType objType)
        {
            selectedCell.SetClosed(objType);

            // Set parent
            characterGO.transform.parent = cellObjects[selectedCell.index].transform;
        }

        public bool TryRemoveAtCell(Cell selectedCell, ObjectType objType)
        {
            if (selectedCell.objectType == objType)
            {
                // Remove the gameobject there, and set to empty object type
                //TODO: make this better
                string cellName = "Cell " + selectedCell.index.ToString();

                GameObject cellGO = this.gameObject.transform.Find(cellName).gameObject;
                if (cellGO == null) { Debug.LogError("no named cell"); return false; }


                Zombie zombie = cellGO.GetComponentInChildren<Zombie>();
                if (zombie)
                {
                    Destroy(zombie.gameObject);
                    // Also remove from enemy placed locations
                    enemyController.RemoveLocation(selectedCell);
                }

                Ghost ghost = cellGO.GetComponentInChildren<Ghost>();
                if (ghost)
                {
                    Destroy(ghost.gameObject);
                }

                selectedCell.SetOpen();

                return true;
            }
            return false;
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
                if (neighbor == null) { continue; }
                if (neighbor.objectType == objType) { return true; }
            }

            return false;
        }

        /// <summary>
        /// Returns a list of vector3 where human characters are placed.
        /// </summary>
        /// <returns></returns>
        public List<Vector3> GetHumanLocations()
        {
            List<Vector3> humanLocations = new List<Vector3>();
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].objectType == ObjectType.Player)
                {
                    humanLocations.Add(cells[i].position);
                }
            }

            return humanLocations;
        }

        public int GetObjectCount(ObjectType objectType)
        {
            int count = 0;
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].objectType == objectType)
                {
                    count += 1;
                }
            }

            return count;
        }
    }
}