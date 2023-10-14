using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS_Benchmark
{
    public class CubeMono : MonoBehaviour
    {
    }

    public class CubeBaker : Baker<CubeMono>
    {
        public override void Bake(CubeMono authoring)
        {
            var CubeEntity = GetEntity(authoring.gameObject,TransformUsageFlags.Dynamic);
            AddComponent<SpawnCubeTag>(CubeEntity);
            AddComponent<CubeTag>(CubeEntity);
            AddComponent(CubeEntity, new CubeAuthoring()
            {
            });
            
        }
    }
}