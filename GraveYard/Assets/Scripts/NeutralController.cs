using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public class NeutralController : MonoBehaviour
    {
        [SerializeField]
        private TurnManager turnManager;

        [SerializeField]
        private MapGrid mapGrid;

        [SerializeField]
        private GameObject neutralPrefab;

        public readonly float heightOffset = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
            turnManager.OnNeutralTurn += TurnManager_OnNeutralTurn;
        }

        private void TurnManager_OnNeutralTurn(object sender, System.EventArgs e)
        {
            StartNeutralTurn();
        }

        private void StartNeutralTurn()
        {
            Debug.Log("It is neutral's turn");

            // Do something

            EndTurn();
        }

        private void EndTurn()
        {
            Debug.Log("End Neutral Turn.");

            // Tell the turn manager to go to the next turn
            turnManager.NextTurn();
        }

        public GameObject CreateCharacter()
        {
            GameObject newCharacter = Instantiate(neutralPrefab, this.transform);
            newCharacter.name = "newNeutral";
            //newCharacter.AddComponent<Zombie>();

            Vector3 position = new Vector3(0, 20f, 0);
            newCharacter.transform.position = position;

            return newCharacter;
        }

    }
}