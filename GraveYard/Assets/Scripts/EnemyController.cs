using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            PlaceCharacter();
            
            EndTurn();
        }

        /// <summary>
        /// Determines a location to place a new character
        /// </summary>
        private void PlaceCharacter()
        {
            // Get available locations
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

            // Create new character instance
            GameObject newCharacter = CreateCharacter();

            bool isPlaced = false;
            while (isPlaced == false)
            {
                // Select a potential location
                Cell selectedLocation = potentialLocations[Random.Range(0, potentialLocations.Count)];

                // Set character info
                Vector3 characterPos = selectedLocation.position;
                characterPos.y += heightOffset;
                newCharacter.transform.position = characterPos;

                // Try to place character
                if(mapGrid.TryPlaceObject(newCharacter, ObjectType.Enemy))
                {
                    isPlaced = true;

                    // Add to placed locations
                    placedLocations.Add(selectedLocation);
                }
            }
        }

        public void EndTurn()
        {
            // This is called when the next turn button is pushed
            Debug.Log("End Enemy Turn.");

            // Tell the battle controller to go to the next turn
            turnManager.NextTurn();
        }

        public GameObject CreateCharacter()
        {
            GameObject newCharacter = Instantiate(enemyPrefab, this.transform);
            newCharacter.name = "newEnemy";

            Vector3 position = new Vector3(0, 20f, 0);
            newCharacter.transform.position = position;

            return newCharacter;
        }
    }
}