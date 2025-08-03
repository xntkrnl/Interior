using Interior.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
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

            meshRenderer.material = materials[Randomizer.Instance.rnd.Next(materials.Count + 1)];
        }
    }
}
