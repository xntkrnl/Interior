using DunGen;
using System.Collections.Generic;
using UnityEngine;

namespace Interior.Scripts
{
    public class SphereBlockerChecker : MonoBehaviour
    {
        public List<Doorway> doorways = new List<Doorway>();
        void Start()
        {
            foreach (Doorway doorway in doorways)
            {
                if (!doorway || !doorway.connectedDoorway) continue;

                if (doorway.tile.placement.tileSet == doorway.connectedDoorway.tile.placement.tileSet)
                    Destroy(doorway.ConnectorSceneObjects[0]);
            }
        }
    }
}
