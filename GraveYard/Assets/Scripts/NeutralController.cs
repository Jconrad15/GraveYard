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

        public List<NeutralEntity> placedNeutrals = new List<NeutralEntity>();

        public static readonly float heightOffset = 0.5f;

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
            MoveCharacters();

            EndTurn();
        }

        private void MoveCharacters()
        {
            // For each neutral character
            // TODO: Create struct/class for the neutral characters to
            // remember a path of cells on which the character moves.
            for (int i = 0; i < placedNeutrals.Count; i++)
            {
                placedNeutrals[i].Move();
            }
        }

        private void EndTurn()
        {
            Debug.Log("End Neutral Turn.");

            // Tell the turn manager to go to the next turn
            turnManager.NextTurn();
        }

        public GameObject CreateCharacter(int x, int z, Cell[] path, Cell startCell)
        {
            GameObject newCharacter = Instantiate(neutralPrefab, this.transform);
            newCharacter.name = "newNeutral";

            NeutralEntity ne = newCharacter.AddComponent<NeutralEntity>();
            placedNeutrals.Add(ne);
            ne.Initialize(x, z, path, mapGrid, startCell);

            Vector3 neutralPos = newCharacter.transform.position;
            neutralPos.x = x;
            neutralPos.y = heightOffset;
            neutralPos.z = z;
            newCharacter.transform.position = neutralPos;

            return newCharacter;
        }

    }
}