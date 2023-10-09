using Script.GameObjectSync.TransformSync;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Script.GameObjectSync
{
    public partial struct GameObjectSyncSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameObjectSyncTag>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (unmanagedAuthoring, entity) in SystemAPI.Query<GameObjectSyncManagedAuthoring>().WithEntityAccess().WithAll<GameObjectSyncTag>())
            {
                var newGameObject = Object.Instantiate(unmanagedAuthoring.GameObject);

                unmanagedAuthoring.GameObject = newGameObject;
                unmanagedAuthoring.Transform = newGameObject.transform;

                ecb.AddComponent(entity, new TransformSyncRotateAuthoring()
                {
                    Rotation = float3.zero
                });
                
                ecb.AddComponent<TransformSyncTag>(entity);
                ecb.RemoveComponent<GameObjectSyncTag>(entity);
            }
        }
    }
}
