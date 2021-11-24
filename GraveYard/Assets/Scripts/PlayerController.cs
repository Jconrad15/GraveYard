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
            // Set the player turn bool in this 
            isPlayerTurn = true;

/*            // Set the player turn bool in each player unit
            foreach (PlayerUnit unit in playerUnits)
            {
                unit.isUnitActive = true;
                unit.TurnStartSetup();
            }*/

            // It is the players turn
            Debug.Log("Player turn. Press Button to End Turn.");
        }

        public void EndTurnSelected()
        {
            if (isPlayerTurn)
            {
                // This is called when the next turn button is pushed
                Debug.Log("End Turn Selected.");
                isPlayerTurn = false;

/*                // Set the player turn bool in each player unit
                foreach (PlayerUnit unit in playerUnits)
                {
                    unit.isUnitActive = false;
                    unit.TurnIsOver();
                }*/

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