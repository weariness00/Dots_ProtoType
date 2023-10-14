using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS_Benchmark
{
    public class CubeSpawnerMono : MonoBehaviour
    {
        public float3 Position;
        public GameObject CubeObject;
        public int SpawnCubeCount;
        public int SpawnedCubeCount;
        public float Radian;
        public float Speed;
    }

    public class CubeSpawnerBaker : Baker<CubeSpawnerMono>
    {
        public override void Bake(CubeSpawnerMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent<CubeSpawnerTag>(entity);
            AddComponent(entity, new CubeSpawnerAuthoring()
            {
                CubeEntity = GetEntity(authoring.CubeObject,TransformUsageFlags.Dynamic),
                SpawnCubeCount = authoring.SpawnCubeCount,
                SpawnedCubeCount = authoring.SpawnedCubeCount,
                Radian = authoring.Radian,
                Speed = authoring.Speed,
            });
        }
    }
}