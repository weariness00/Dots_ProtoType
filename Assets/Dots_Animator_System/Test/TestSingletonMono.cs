using Unity.Entities;
using UnityEngine;

namespace Dots_Animator_System.Test
{
    public class TestSingletonMono : MonoBehaviour
    {
    
    }
    
    public class TestSingletonBaker : Baker<TestSingletonMono>
    {
        public override void Bake(TestSingletonMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            var buffer = AddBuffer<TestAuthoring>(entity);
            buffer.Add(new TestAuthoring { SomeValue = 10 });

        }
    }
}
