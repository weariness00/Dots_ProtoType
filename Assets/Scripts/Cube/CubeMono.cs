using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public class CubeMono : MonoBehaviour
    {
        public float3 Position;
    }

    public class CubeBaker : Baker<CubeMono>
    {
        public override void Bake(CubeMono authoring)
        {
            var CubeEntity = GetEntity(authoring.gameObject,TransformUsageFlags.Dynamic);
            AddComponent<SpawnCubeTag>(CubeEntity);
            AddComponent<CubeTag>(CubeEntity);
            AddComponent(CubeEntity, new CubeTagAuthoring()
            {
                //NoteType = authoring.NoteType,
                Position = authoring.Position
            });
            AddComponent(CubeEntity, new RotationComponent()
            {
                x = 0,
                y = 0,
                z = 0
            });
        }
    }
}