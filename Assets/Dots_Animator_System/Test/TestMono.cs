using Unity.Entities;
using UnityEngine;

namespace Dots_Animator_System.Test
{
    public class TestMono : MonoBehaviour
    {
    
    }
    
    public class TestBaker : Baker<TestMono>
    {
        public override void Bake(TestMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent<TestInitTag>(entity);
        }
    }
}
