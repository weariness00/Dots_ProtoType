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
            foreach (var (animatorSync, entity) in SystemAPI.Query<AnimatorController>().WithEntityAccess().WithNone<AnimatorControllerBlobAssetReference>())
            {
                ref var animatorSyncBlob = ref blobBuilder.ConstructRoot<AnimatorControllerBlob>();
                animatorSyncBlob.MakeBlob(SystemAPI.GetBuffer<BoneInfo>(entity), animatorSync, blobBuilder);

                var blobAssetReferenceBuffer = ecb.AddBuffer<AnimatorControllerBlobAssetReference>(entity);
                var blobReference = blobBuilder.CreateBlobAssetReference<AnimatorControllerBlob>(Allocator.Persistent);
                for (int i = 0; i < blobReference.Value.Layers.Length; i++)
                {
                    blobAssetReferenceBuffer.Add(new AnimatorControllerBlobAssetReference()
                    {
                        Animator = blobReference,
                        LayerIndex = i,
                        LayerName = blobReference.Value.Layers[i].Name,
                        LayerWeight = blobReference.Value.Layers[i].Weight,
                        
                        CurrenStateBlob = blobReference.Value.Layers[i].GetStateBlob(blobReference.Value.Layers[i].CurrentStateHashCode),
                    });
                }
                
                ecb.AddComponent<AnimatorTag>(entity);
                ecb.RemoveComponent<AnimatorController>(entity);
            }
        }
    }
}
