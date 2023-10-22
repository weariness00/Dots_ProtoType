using Unity.Entities;
using UnityEngine;

namespace StateMachine.Monster
{
    public class MonsterSpawnerAuthoring : MonoBehaviour
    {
        public GameObject Cube;
        
        public class MonsterSpawnerBaker : Baker<MonsterSpawnerAuthoring>
        {
            public override void Bake(MonsterSpawnerAuthoring authoring)
            {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.None);
                var cubeEntity = GetEntity(authoring.Cube, TransformUsageFlags.Dynamic);

                AddComponent(entity, new MonsterSpawner()
                {
                    MonsterGameObject = cubeEntity,
                });
            }
        }
    }
}