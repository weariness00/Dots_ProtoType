using Unity.Entities;
using UnityEngine;

namespace Gpu_Animation.Scripts
{
    public class GPUAnimationAuthoring : MonoBehaviour
    {
        private class GPUAnimationBaker : Baker<GPUAnimationAuthoring>
        {
            public override void Bake(GPUAnimationAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new GPUAnimationData()
                {
                    Frame = 0,
                });
            }
        }
    }
}