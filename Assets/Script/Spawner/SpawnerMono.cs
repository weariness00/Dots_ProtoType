using Unity.Entities;
using UnityEngine;

namespace Script.Spawner
{
    public class SpawnerMono : MonoBehaviour
    {
        public GameObject spawnObject;
        
        public int spawnIndex;
        public float spawnInterval;
    }

    public class SpawnerBaker : Baker<SpawnerMono>
    {
        public override void Bake(SpawnerMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent<SpawnerTag>(entity);
            
            AddComponent(entity, new SpawnerAuthoring()
            {
                SpawnIndex = authoring.spawnIndex,
                SpawnInterval = authoring.spawnInterval,
            });
            AddComponent(entity, new SpawnerEntity()
            {
                SpawnObject = GetEntity(authoring.spawnObject, TransformUsageFlags.Dynamic),
                
                Position = authoring.spawnObject.transform.position,
                Rotate = authoring.spawnObject.transform.rotation,
                Scale = authoring.spawnObject.transform.localScale.x,
            });
        }
    }
}
