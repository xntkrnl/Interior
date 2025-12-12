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
                if (doorway == null || doorway.connectedDoorway == null) continue;

                if (doorway.tile.placement.tileSet == doorway.connectedDoorway.tile.placement.tileSet || !HabitatConfig.Instance.habitatAssets.GetConfig<bool>("Enable forcefields").Value)
                    Destroy(doorway.ConnectorSceneObjects[0]);
            }
        }
    }
}
