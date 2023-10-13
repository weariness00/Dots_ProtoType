using Unity.Entities;
using UnityEngine;

namespace Game
{
    using Unity.Entities;
    using Unity.Mathematics;
    using UnityEngine;

    namespace Game
    {
        public class AniCubeMono : MonoBehaviour
        {
            public GameObject Prefab;
        }

        public class CubeBaker : Baker<AniCubeMono>
        {
            public override void Bake(AniCubeMono authoring)
            {
                var CubeEntity = GetEntity(authoring.gameObject,TransformUsageFlags.Dynamic);
                AddComponent<SpawnCubeTag>(CubeEntity);
                AddComponent<CubeTag>(CubeEntity);
                AddComponent(CubeEntity, new CubeAuthoring()
                {
                });
                AddComponentObject(CubeEntity, new AnimatorSyncObject() { gameObject = authoring.Prefab });
            }
        }
    }
}