using Unity.Entities;
using UnityEngine;

namespace Script.GameObjectSync
{
    public class GameObjectSyncMono : MonoBehaviour
    {
        public GameObject go;
    }
    
    public class GameObjectSyncBaker : Baker<GameObjectSyncMono>
    {
        public override void Bake(GameObjectSyncMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<GameObjectSyncTag>(entity);

            AddComponentObject(entity, new GameObjectSyncManagedAuthoring()
            {
                GameObject = authoring.go,
                
            });
        }
    }
}
