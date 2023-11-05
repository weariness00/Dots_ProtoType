using Unity.Entities;
using UnityEngine;
using UnityEngine.VFX;

namespace VFX
{
    public class VFXAuthoring : MonoBehaviour
    {
        public VisualEffect VFX;
    }
    public class VFXBaker : Baker<VFXAuthoring>
    {
        public override void Bake(VFXAuthoring authoring)
        {
            var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
            
            
        }
    }
}