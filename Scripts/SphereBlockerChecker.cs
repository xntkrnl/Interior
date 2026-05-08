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
                if (doorway == null || doorway.ConnectedDoorway == null) continue;

                if (doorway.Tile.Placement.TileSet == doorway.ConnectedDoorway.Tile.Placement.TileSet || !HabitatContentHandler.Instance.habitatAssets.GetConfig<bool>("Enable forcefields").Value)
                    Destroy(doorway.ConnectorSceneObjects[0]);
            }
        }
    }
}
