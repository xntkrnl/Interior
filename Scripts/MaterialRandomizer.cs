using Interior.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Interior.Scripts
{
    public class MaterialRandomizer : MonoBehaviour
    {
        public List<Material> materials = new List<Material>();
        public MeshRenderer? meshRenderer;

        void Start()
        {
            if (materials.Count == 0) return;

            if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();

            ServerRandomizer.Instance.NextServerRpc(materials.Count, out int result);
            meshRenderer.sharedMaterial = materials[result];
        }
    }
}
