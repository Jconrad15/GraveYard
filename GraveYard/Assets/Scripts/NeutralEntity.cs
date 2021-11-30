using System;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public class NeutralEntity: MonoBehaviour
    {
        private int xStart;
        private int zStart;

        private Stack<Cell> path;

        public void Initialize(int x, int z, Cell[] newPath)
        {
            xStart = x;
            zStart = z;

            path = new Stack<Cell>();
            for (int i = newPath.Length - 1; i > 0; i--)
            {
                path.Push(newPath[i]);
            }
        }


    }
}