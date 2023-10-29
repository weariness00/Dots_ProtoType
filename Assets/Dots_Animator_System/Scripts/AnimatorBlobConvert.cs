using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Dots_Animator_System.Scripts
{
    public partial struct AnimatorBlobConvert : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndInitializationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            BlobBuilder blobBuilder = new BlobBuilder(Allocator.Persistent);
            foreach (var (animatorSync, entity) in SystemAPI.Query<AnimatorSync>().WithEntityAccess().WithNone<AnimatorSyncBlobAssetReference>())
            {
                // var animatorSyncBlob = new AnimatorSyncBlob(animatorSync, blobBuilder);
                //
                // var animatorSyncBlobAssetReference = new AnimatorSyncBlobAssetReference()
                // {
                //     Animator = blobBuilder.CreateBlobAssetReference<AnimatorSyncBlob>(Allocator.Persistent),
                // };
                //
                // ecb.AddComponent(entity, animatorSyncBlobAssetReference);
            }
        }
    }
}
