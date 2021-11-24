using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public enum TurnState { SETUP, PLAYERTURN, ENEMYTURN, WON, LOST };

    public class TurnManager : MonoBehaviour
    {
        [SerializeField]
        private MapGrid mapGrid;

        public event EventHandler OnPlayerTurn;
        public event EventHandler OnEnemyTurn;

        private TurnState state;

        // Start is called before the first frame update
        void OnEnable()
        {
            state = TurnState.SETUP;
            StartCoroutine(Setup());
        }

        IEnumerator Setup()
        {
            // Do stuff for the setup
            Debug.Log("setup");
            mapGrid.InitializeGrid();

            yield return new WaitForSeconds(1f);

            // Code to determine who goes first.  For now, player goes first

            StartPlayerTurn();
        }

        public void StartPlayerTurn()
        {
            // Player perfroms their turn
            // Raise event that states that is is the player's turn

            state = TurnState.PLAYERTURN;
            OnPlayerTurn?.Invoke(this, EventArgs.Empty);
        }

        public void StartEnemyTurn()
        {
            // Enemy performs their turn
            // Raise event that states that is is the enemy's turn

            Debug.Log("Raise Enemy turn event");

            state = TurnState.ENEMYTURN;
            OnEnemyTurn?.Invoke(this, EventArgs.Empty);
        }

        private void EndBattle(bool playerWin, bool enemyWin)
        {
            if (state == TurnState.WON)
            {
                Debug.Log("You won the battle!");
            }
            else if (state == TurnState.LOST)
            {
                Debug.Log("You were defeated.");
            }
        }

        public void NextTurn(bool playerWin = false, bool enemyWin = false)
        {
            Debug.Log("Next turn is triggered");

            if (playerWin == true || enemyWin == true)
            {
                EndBattle(playerWin, enemyWin);  // Need to tell EndBattle if win or lose
            }
            else
            {
                if (state == TurnState.PLAYERTURN)
                {
                    StartEnemyTurn();
                }
                else if (state == TurnState.ENEMYTURN)
                {
                    StartPlayerTurn();
                }
                else
                {
                    Debug.LogError("Something bad happened with the turn order.");
                }
            }
        }
    }
}