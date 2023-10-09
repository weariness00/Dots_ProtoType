using Unity.Entities;
using Unity.Mathematics;

namespace Script.GameObjectSync.TransformSync
{
    public struct TransformSyncTag : IComponentData{}

    public struct TransformSyncRotateAuthoring : IComponentData
    {
        public float3 Rotation;
    }
}
