using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraveYard
{
    public enum ObjectType { Player, Enemy, Obstacle, Empty, Neutral};

    public class Cell
    {
        public int index;
        public Vector3 position;
        public bool IsOpen { get; protected set; }
        public ObjectType objectType;

        public bool IsPath { get; protected set; }

        public Cell(int newIndex, Vector3 newPosition)
        {
            index = newIndex;
            position = newPosition;
            IsOpen = true;
            objectType = ObjectType.Empty;

            // Defaults to not a path
            IsPath = false;
        }

        public void SetOpen()
        {
            IsOpen = true;
            objectType = ObjectType.Empty;
        }

        public void SetClosed(ObjectType objType)
        {
            IsOpen = false;
            objectType = objType;
        }

        public void SetAsPath()
        {
            IsPath = true;
        }

    }
}