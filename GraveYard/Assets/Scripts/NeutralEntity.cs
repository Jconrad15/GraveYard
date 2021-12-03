using System;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public class NeutralEntity: MonoBehaviour
    {
        private int xStart;
        private int zStart;

        private Stack<Cell> path;
        private Stack<Cell> reversePath;

        bool isForward;

        private MapGrid mapGrid;

        private Cell currentCell;

        public void Initialize(int x, int z, Cell[] newPath, MapGrid mG)
        {
            xStart = x;
            zStart = z;

            isForward = true;
            mapGrid = mG;

            path = new Stack<Cell>();
            reversePath = new Stack<Cell>();

            for (int i = newPath.Length - 1; i > 0; i--)
            {
                path.Push(newPath[i]);

                
            }

            // Peek for current cell
            currentCell = path.Peek();
        }


        public void Move()
        {
            
            if (isForward)
            {
                Cell nextCell = path.Pop();
                if (TryMoveNeutralEntity(this.gameObject, nextCell))
                {
                    // If moved, add cell to reverse path
                    reversePath.Push(nextCell);
                    // Also set current cell to open
                    currentCell.SetOpen();
                    currentCell = nextCell;
                    
                }
                else
                {
                    // If couldn't move, replace cell
                    path.Push(nextCell);
                }

                // Check if the path was emptied
                if (path.Count == 0)
                {
                    isForward = false;
                }
            }
            else
            {
                Cell nextCell = reversePath.Pop();
                if (TryMoveNeutralEntity(this.gameObject, nextCell))
                {
                    // If moved, add cell to path
                    path.Push(nextCell);
                    // Also set current cell to open
                    currentCell.SetOpen();
                    currentCell = nextCell;
                }
                else
                {
                    // If couldn't move, replace cell
                    reversePath.Push(nextCell);
                }

                // Check if the reverse path was emptied
                if (reversePath.Count == 0)
                {
                    isForward = true;
                }

            }
        }


        public bool TryMoveNeutralEntity(GameObject neutralEntity, Cell targetCell)
        {
            if (targetCell == null)
            {
                Debug.LogError("Placing at null cell");
                return false;
            }

            // Check if the cell is open
            if (targetCell.IsOpen == false)
            {
                // Something is in the way, check what
                ObjectType obj = targetCell.objectType;
                if (obj == ObjectType.Player || obj == ObjectType.Enemy)
                {
                    // Delete the player or enemy
                    mapGrid.TryRemoveAtCell(targetCell, obj);
                }
                else if (obj == ObjectType.Empty)
                {
                    // Do nothing, just move
                }
                else if (obj == ObjectType.Neutral)
                {
                    // Don't move the neutral character
                    return false;
                }
                else if (obj == ObjectType.Obstacle)
                {
                    // This should't happen
                    Debug.LogError("Neutral character walking into obstacle");
                    return false;
                }

            }

            // Place the neutral entity at the target cell
            mapGrid.PlaceAtCell(targetCell, neutralEntity, ObjectType.Neutral);

            Vector3 pos = neutralEntity.transform.position;
            pos = targetCell.position;
            pos.y += NeutralController.heightOffset;
            neutralEntity.transform.position = pos;
            
            return true;
        }
    }
}