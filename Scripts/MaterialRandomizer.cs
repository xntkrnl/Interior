using Dawn;
using Interior.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Interior.Scripts
{

    [Serializable]
    public class RendererWithMaterials
    {
        public Material[] materials;
        public Renderer renderer;
    }

    public class MaterialRandomizer : MonoBehaviour
    {
        [SerializeField]
        public RendererWithMaterials[] renderersAndMaterials;
        public MeshRenderer? meshRenderer;

        void Start()
        {
            if (renderersAndMaterials.Length == 0) return;

            if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();

            ServerRandomizer.Instance.NextServerRpc(renderersAndMaterials.Length, out int result);
            meshRenderer.sharedMaterials = renderersAndMaterials[result].materials;
        }
    }
}
