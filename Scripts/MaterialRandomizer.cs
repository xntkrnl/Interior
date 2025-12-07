using System;
using Unity.Netcode;
using UnityEngine;

namespace Interior.Scripts
{

    [Serializable]
    internal class Materials
    {
        internal Material[] materials;
    }

    internal class MaterialRandomizer : NetworkBehaviour
    {
        [SerializeField]
        internal Materials[] materials;
        internal MeshRenderer? meshRenderer;
        private NetworkVariable<int> current = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (materials.Length == 0 || meshRenderer == null) return;

            if (IsServer)
            {
                System.Random random = new System.Random();
                current.Value = random.Next(materials.Length);
            }

            meshRenderer.sharedMaterials = materials[current.Value].materials;
        }
    }
}
