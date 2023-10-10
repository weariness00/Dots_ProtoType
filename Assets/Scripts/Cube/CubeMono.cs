using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public class CubeMonod : MonoBehaviour
    {
        
    }

    public class CubeBaker : Baker<CubeMonod>
    {
        public override void Bake(CubeMonod authoring)
        {
            var CubeEntity = GetEntity(authoring.gameObject,TransformUsageFlags.Dynamic);
            AddComponent<SpawnCubeTag>(CubeEntity);
            AddComponent<CubeTag>(CubeEntity);
            AddComponent(CubeEntity, new CubeTagAuthoring()
            {
                
            });
        }
    }
}