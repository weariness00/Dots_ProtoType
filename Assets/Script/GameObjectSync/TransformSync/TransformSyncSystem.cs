using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Script.GameObjectSync.TransformSync
{
    public partial struct TransformSyncSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TransformSyncTag>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (managedAuthoring, transformSyncRotateAuthoring, localTransform, entity) in SystemAPI.Query<GameObjectSyncManagedAuthoring, TransformSyncRotateAuthoring, LocalTransform>().WithEntityAccess().WithAll<TransformSyncTag>())
            {
                var transform = localTransform;
                transform.Rotation = quaternion.Euler(transformSyncRotateAuthoring.Rotation);

                managedAuthoring.Transform.position = transform.Position;
                managedAuthoring.Transform.rotation = transform.Rotation;
                managedAuthoring.Transform.localScale.Scale(new Vector3(transform.Scale,transform.Scale,transform.Scale));
                
                SystemAPI.SetComponent(entity, transform);
            }
        }
    }
}
