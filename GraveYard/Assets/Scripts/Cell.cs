using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public class Cell
    {
        public int index;
        public Vector3 position;

        public Cell(int newIndex, Vector3 newPosition)
        {
            index = newIndex;
            position = newPosition;
        }

    }
}