using Unity.Entities;

namespace Dots_Animator_System.Scripts
{
    public struct AnimatorSyncAuthoring : IComponentData
    {
    }

    public struct AnimatorSyncBlobAsset : IComponentData
    {
        public BlobAssetReference<AnimatorSyncAuthoring> Value;
    }
}
