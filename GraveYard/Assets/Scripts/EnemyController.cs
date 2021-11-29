using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GraveYard
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private TurnManager turnManager;

        [SerializeField]
        private MapGrid mapGrid;

        [SerializeField]
        private GameObject enemyPrefab;

        public List<Cell> placedLocations = new List<Cell>();

        public readonly float heightOffset = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
            turnManager.OnEnemyTurn += TurnManager_OnEnemyTurn;
        }

        private void TurnManager_OnEnemyTurn(object sender, System.EventArgs e)
        {
            StartEnemyTurn();
        }

        private void StartEnemyTurn()
        {
            // It is the enemys turn
            Debug.Log("Enemy turn");

            // Determine where to place new character
            bool isPlaced = PlaceCharacter();
            
            EndTurn(!isPlaced);
        }

        /// <summary>
        /// Determines a location to place a new character, returns true if placed.
        /// </summary>
        private bool PlaceCharacter()
        {
            // Get available locations
            List<Cell> potentialLocations = GetPotentialLocations();

            if (potentialLocations.Count == 0)
            {
                Debug.Log("Enemy passes");
                return false;
            }

            // Create new character instance
            GameObject newCharacter = CreateCharacter();

            bool isPlaced = false;
            int counter = 0;
            int counterLimit = 1000;
            while (isPlaced == false)
            {
                // Check limit
                if (counter > counterLimit) { break; };

                // Select a potential location
                Cell selectedLocation = SelectLocationCell(potentialLocations);
                
                // Set character info
                Vector3 characterPos = selectedLocation.position;
                characterPos.y += heightOffset;
                newCharacter.transform.position = characterPos;

                // Try to place character
                if (mapGrid.TryPlaceObject(newCharacter, ObjectType.Enemy))
                {
                    isPlaced = true;

                    // Add to placed locations
                    placedLocations.Add(selectedLocation);
                }

                counter += 1;
            }

            // Check is placed
            if (isPlaced == false)
            {
                Debug.Log("Enemy passes second try.");
                Destroy(newCharacter);
                return false;
            }
            return true;
        }

        private List<Cell> GetPotentialLocations()
        {
            List<Cell> potentialLocations = new List<Cell>();
            // For each placed location
            for (int i = 0; i < placedLocations.Count; i++)
            {
                // For each direction
                for (int j = 0; j < 4; j++)
                {
                    Cell neighbor = mapGrid.GetNeighbor(placedLocations[i], (Direction)j);

                    // Add neighbor cell to potential list if
                    // not null
                    // not already in list
                    // isOpen
                    if (neighbor != null)
                    {
                        if (potentialLocations.Contains(neighbor) == false && neighbor.IsOpen)
                        {
                            potentialLocations.Add(neighbor);
                        }
                    }
                }
            }
            return potentialLocations;
        }

        /// <summary>
        /// Returns a selected cell to place a character.
        /// </summary>
        /// <param name="potentialLocations"></param>
        /// <returns></returns>
        private Cell SelectLocationCell(List<Cell> potentialLocations)
        {
            float[] weights = new float[potentialLocations.Count];
            List<Vector3> humanLocations = mapGrid.GetHumanLocations();

            for (int i = 0; i < potentialLocations.Count; i++)
            {
                // Initial weight of each cell to zero
                weights[i] = 0;

                float distance = GetNearestHumanCellDistance(
                    potentialLocations[i].position,
                    humanLocations);

                weights[i] -= distance;
            }

            // Select location with first or second largest weight
            float weight;
            if (Random.value < 0.9)
            {
                weight = weights.Max();
            }
            else
            {
                float[] arr = weights.ToArray();
                Array.Sort(arr);
                weight = arr[arr.Length - 2];
            }

            int maxIndex = weights.ToList().IndexOf(weight);
            return potentialLocations[maxIndex];
        }

        /// <summary>
        /// Returns the smallest distance to a human placed location.
        /// </summary>
        /// <param name="potentialLocation"></param>
        /// <param name="humanLocations"></param>
        /// <returns></returns>
        private float GetNearestHumanCellDistance(Vector3 potentialLocation, List<Vector3> humanLocations)
        {
            float distance = float.MaxValue;
            for (int i = 0; i < humanLocations.Count; i++)
            {
                float d = Vector3.Distance(humanLocations[i], potentialLocation);

                if (d < distance)
                {
                    distance = d;
                }
            }

            return distance;
        }

        public void EndTurn(bool passed)
        {
            Debug.Log("End Enemy Turn.");

            // Tell the turn manager to go to the next turn
            turnManager.NextTurn(passed);
        }

        public GameObject CreateCharacter()
        {
            GameObject newCharacter = Instantiate(enemyPrefab, this.transform);
            newCharacter.name = "newEnemy";
            newCharacter.AddComponent<Zombie>();

            Vector3 position = new Vector3(0, 20f, 0);
            newCharacter.transform.position = position;

            return newCharacter;
        }
    }
}