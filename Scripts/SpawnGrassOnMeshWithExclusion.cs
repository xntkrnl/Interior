using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace Interior.Scripts
{
    public class SpawnGrassOnMeshWithExclusion : SpawnGrassOnMesh
    {
        public Collider[] exclusionZones = null!;
        public bool onlyRenderIndoors = false;
        private Camera camera = null!;
        private RenderParams renderParams;
        private readonly List<Bounds> ChunkBounds = new();
        //public Vector3 meshRotationOffset;


        private new void Start()
        {
            terrainMesh = GetComponent<MeshFilter>().mesh;
            if (mesh == null || terrainMesh == null)
            {
                Plugin.mls.LogError("GrassSpawner requires a Mesh component");
                base.enabled = false;
                return;
            }

            renderParams = new RenderParams(material)
            {
                layer = gameObject.layer,
                //worldBounds = new Bounds(Vector3.zero, Vector3.one * 10000f),

                lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes,
                receiveShadows = true,
                shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off
            };

            if (!spawnManually)
            {
                SpawnGrass();
            }
        }

        private new void Update()
        {
            //editor testing
            if (GameNetworkManager.Instance == null) RenderBatches();
            else
            {
                if (!spawnedGrass) return;
                if (GameNetworkManager.Instance.localPlayerController?.isInsideFactory == true && !onlyRenderIndoors) return;
                if (GameNetworkManager.Instance.localPlayerController?.isInsideFactory == false && !onlyRenderOutdoors) return;

                RenderBatches();
            }
        }

        private bool IsPointInExclusionZone(Vector3 point)
        {
            foreach (Collider exclZone in exclusionZones)
            {
                if (exclZone == null) continue;

                var closestPoint = exclZone.ClosestPoint(point);
                if (closestPoint == point) return true;
            }

            return false;
        }

        [ContextMenu("SpawnGrass")]
        private new void SpawnGrass()
        {
            Batches.Clear();
            ChunkPositions.Clear();
            ChunkBounds.Clear();

            List<Vector3> verts = terrainMesh.vertices.ToList();
            List<int> tris = terrainMesh.triangles.ToList();

            int randomTriangleIndex = 0;
            List<Matrix4x4> grassMatrixList = new List<Matrix4x4>();

            for (int k = 0; k < numberOfBlades; k++)
            {
                randomTriangleIndex = Random.Range(0, tris.Count / 3);
                int index = tris[randomTriangleIndex * 3];
                int index2 = tris[randomTriangleIndex * 3 + 1];
                int index3 = tris[randomTriangleIndex * 3 + 2];
                Vector3 vector = transform.TransformPoint(verts[index]);
                Vector3 vector2 = transform.TransformPoint(verts[index2]);
                Vector3 vector3 = transform.TransformPoint(verts[index3]);
                Vector3 randomPointInTriangle = GetRandomPointInTriangle(vector, vector2, vector3);

                if (IsPointInExclusionZone(randomPointInTriangle)) continue;

                Vector3 normalized = Vector3.Cross(vector2 - vector, vector3 - vector).normalized;
                if (Vector3.Angle(normalized, Vector3.up) > maxSlope) continue;
                else
                {
                    grassMatrixList.Add(Matrix4x4.TRS(
                        randomPointInTriangle,
                        //Quaternion.Euler(meshRotationOffset) * Quaternion.FromToRotation(Vector3.up, normalized),
                        Quaternion.FromToRotation(Vector3.up, normalized),
                        Vector3.Scale(Vector3.one, new Vector3(1f, Random.Range(minVerticalScale, 1f), 1f)) * Random.Range(minScale, maxScale))
                    );
                }
            }

            Bounds localBounds = terrainMesh.bounds;

            Vector3 center = transform.TransformPoint(localBounds.center);
            Vector3 worldSize = Vector3.Scale(localBounds.size, transform.lossyScale);
            var bounds = new Bounds(center, worldSize);

            Vector3 firstChunkCenter = bounds.min + new Vector3(cellSize / 2f, 0f, cellSize / 2f);
            Plugin.mls.LogInfo($"bounds sizes {bounds.size.x}; {bounds.size.z} ; {cellSize}");
            int chunkCountX = Mathf.CeilToInt(bounds.size.x / cellSize);
            int chunkCountZ = Mathf.CeilToInt(bounds.size.z / cellSize);
            for (int l = 0; l < chunkCountX; l++)
            {
                for (int m = 0; m < chunkCountZ; m++)
                {
                    Vector3 chunkPosition = firstChunkCenter + new Vector3(l * cellSize, 0f, m * cellSize);
                    ChunkPositions.Add(chunkPosition);

                    ChunkBounds.Add(new Bounds(chunkPosition, new Vector3(cellSize, 50f, cellSize)));
                }
            }

            Vector3 grassPosition = Vector3.zero;

            for (int i = 0; i < ChunkPositions.Count; i++)
            {
                Batches.Add(new List<Matrix4x4>());
            }

            foreach (var grassMatrix in grassMatrixList)
            {
                grassPosition = grassMatrix.GetPosition();

                for (int i = 0; i < ChunkBounds.Count; i++)
                {
                    if (!ChunkBounds[i].Contains(grassPosition)) continue;

                    if (Batches[i].Count < 1023)
                    {
                        Batches[i].Add(grassMatrix);
                    }

                    break;
                }
            }

            spawnedGrass = true;
        }

        private new void RenderBatches()
        {
            camera = ((StartOfRound.Instance == null) || (StartOfRound.Instance.activeCamera == null)) ? Camera.main : StartOfRound.Instance.activeCamera;
            if (!camera) return;

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            float maxDistSqr = chunkDrawDistance * chunkDrawDistance;

            for (int i = 0; i < Batches.Count && i < ChunkPositions.Count; i++)
            {
                var batch = Batches[i];

                if (batch == null || batch.Count == 0) continue;

                Vector3 chunkCenter = ChunkPositions[i];
                //Vector3 flatCamPos = new Vector3(camera.transform.position.x, chunkCenter.y, camera.transform.position.z);

                Vector3 delta = new Vector3(camera.transform.position.x - chunkCenter.x, 0f, camera.transform.position.z - chunkCenter.z);

                if (delta.sqrMagnitude > maxDistSqr)
                    continue;

                if (!GeometryUtility.TestPlanesAABB(planes, ChunkBounds[i]))
                    continue;

                renderParams.worldBounds = ChunkBounds[i];

                for (int sub = 0; sub < mesh.subMeshCount; sub++)
                    Graphics.RenderMeshInstanced(renderParams, mesh, sub, batch);
            }
        }
    }
}
