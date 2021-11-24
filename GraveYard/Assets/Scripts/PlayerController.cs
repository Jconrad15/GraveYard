using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private TurnManager turnManager;
        public bool isPlayerTurn = false;

        [SerializeField]
        private MapGrid mapGrid;

        [SerializeField]
        private GameObject characterPrefab;

        private GameObject characterContainer;

        private float heightOffset = 1f;

        // Start is called before the first frame update
        private void Start()
        {
            // Subscribe to on player turn event
            turnManager.OnPlayerTurn += TurnManager_OnPlayerTurn;

            // Create game object to hold placed characters
            characterContainer = new GameObject("Character Container");
            characterContainer.transform.parent = this.transform;
        }

        private void TurnManager_OnPlayerTurn(object sender, EventArgs e)
        {
            StartPlayerTurn();
        }

        private void StartPlayerTurn()
        {
            // Set the player turn bool
            isPlayerTurn = true;

            // It is the players turn
            Debug.Log("Player turn. Press Button to End Turn.");

            StartCoroutine(LocationSelection());
        }

        private IEnumerator LocationSelection()
        {
            // Get new character instance
            GameObject newCharacter = CreateCharacter();

            bool isSelected = false;
            while (isSelected == false)
            {
                // Determine mouse location
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // If ray hits something
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 characterPos = hit.transform.position;
                    characterPos.y += heightOffset;

                    newCharacter.transform.position = characterPos;
                }

                // If click to place the character
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("click");
                    // Check if location is available
                    if (mapGrid.IsCellOpen(newCharacter.transform.position))
                    {
                        Debug.Log("Location selected");
                        isSelected = true;
                    }
                    else
                    {
                        Debug.Log("Cell is not open.");
                    }
                }

                yield return null;
            }

            mapGrid.PlaceAtCell(newCharacter);
        }

        private GameObject CreateCharacter()
        {
            GameObject newCharacter = Instantiate(characterPrefab, this.transform);
            newCharacter.name = "newCharacter";
            newCharacter.transform.parent = characterContainer.transform;

            Vector3 position = new Vector3(0, heightOffset, 0);
            newCharacter.transform.position = position;

            return newCharacter;
        }

        public void EndTurnSelected()
        {
            if (isPlayerTurn)
            {
                // This is called when the next turn button is pushed
                Debug.Log("End Turn Selected.");
                isPlayerTurn = false;

                // Tell the battle controller to go to the next turn
                turnManager.NextTurn();


            }
            else
            {
                Debug.Log("It is not the player's turn.");
            }

        }

    }
}