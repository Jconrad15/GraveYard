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

        public readonly float heightOffset = 1f;

        // Start is called before the first frame update
        private void Start()
        {
            // Subscribe to on player turn event
            turnManager.OnPlayerTurn += TurnManager_OnPlayerTurn;
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

            bool isPlaced = false;
            while (isPlaced == false)
            {
                // Determine mouse location
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // If ray hits something, move the character
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 characterPos = hit.transform.position;
                    characterPos.y += heightOffset;

                    newCharacter.transform.position = characterPos;
                }

                // Try to place character when click
                if (Input.GetMouseButtonDown(0))
                {
                    if (mapGrid.TryPlaceObject(newCharacter, ObjectType.Player))
                    {
                        Debug.Log("Location selected");
                        isPlaced = true;
                    }
                    else
                    {
                        Debug.Log("Cell is not valid.");
                    }
                }

                yield return null;
            }

            // Automate turn end here
            //EndTurnSelected();
        }

        public GameObject CreateCharacter()
        {
            GameObject newCharacter = Instantiate(characterPrefab, this.transform);
            newCharacter.name = "newCharacter";
            newCharacter.AddComponent<Ghost>();

            Vector3 position = new Vector3(0, 20f, 0);
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