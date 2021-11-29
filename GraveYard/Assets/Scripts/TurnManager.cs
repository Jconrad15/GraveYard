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

        private int passCount = 0;

        private MapData md;

        // Start is called before the first frame update
        private void OnEnable()
        {
            state = TurnState.SETUP;
            StartCoroutine(Setup());
        }

        private IEnumerator Setup()
        {
            // Do stuff for the setup
            Debug.Log("setup");
            md = mapGrid.InitializeGrid();

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

        public void NextTurn(bool pass = false)
        {
            if (pass) { passCount += 1; }

            bool mustEnd = false;
            // If multiple passes
            if (passCount >= 2)
            {
                mustEnd = true;
            }

            Debug.Log("Next turn is triggered");

            // Determine if winner
            if (CheckPlayerWin(mustEnd))
            {
                EndBattle();
            }
            else if (CheckEnemyWin(mustEnd))
            {
                EndBattle();
            }
            else
            {
                // Next turn
                if (state == TurnState.PLAYERTURN)
                {
                    StartEnemyTurn();
                }
                else if (state == TurnState.ENEMYTURN)
                {
                    // Enemy just went, reset pass count
                    passCount = 0;

                    StartPlayerTurn();
                }
                else
                {
                    Debug.LogError("Something bad happened with the turn order.");
                    StartPlayerTurn();
                }
            }
        }

        private void EndBattle()
        {
            if (state == TurnState.WON)
            {
                Debug.Log("You won the battle!");
            }
            else if (state == TurnState.LOST)
            {
                Debug.Log("You were defeated.");
            }
            else
            {
                Debug.LogError("How did we get to endbattle when no one won?");
                // Just set to win
                state = TurnState.WON;
                EndBattle();
            }
        }

        private bool CheckPlayerWin(bool mustEnd)
        {
            int playerCount = mapGrid.GetObjectCount(ObjectType.Player);
            int enemyCount = mapGrid.GetObjectCount(ObjectType.Enemy);

            if (mustEnd)
            {
                if (playerCount > enemyCount)
                {
                    // Player wins
                    state = TurnState.WON;
                    return true;
                }
            }
            else
            {
                // If player has half of controllable tiles
                if (playerCount >= md.controllableCells / 2f)
                {
                    // Player wins
                    state = TurnState.WON;
                    return true;
                }
            }

            return false;
        }

        private bool CheckEnemyWin(bool mustEnd)
        {
            int playerCount = mapGrid.GetObjectCount(ObjectType.Player);
            int enemyCount = mapGrid.GetObjectCount(ObjectType.Enemy);

            if (mustEnd)
            {
                if (enemyCount > playerCount)
                {
                    // Enemy wins
                    state = TurnState.LOST;
                    return true;
                }
            }
            else
            {
                // If Enemy has half of controllable tiles
                if (enemyCount >= md.controllableCells / 2f)
                {
                    // Enemy wins
                    state = TurnState.LOST;
                    return true;
                }
            }

            return false;
        }
    }
}