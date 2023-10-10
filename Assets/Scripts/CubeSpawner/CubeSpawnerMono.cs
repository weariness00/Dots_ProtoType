using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public class CubeSpawnerMono : MonoBehaviour
    {
        public float3 Position;
        public GameObject CubeObject;
        public int CubeCount;
        public int Radian;
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
                CubeCount = authoring.CubeCount,
                Radian = authoring.Radian,
            });
        }
    }
}