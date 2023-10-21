using System;
using Unity.Entities;

namespace Dots_Animator_System.Test
{
    public struct TestInitTag : IComponentData{}

    public struct TestAuthoring : IBufferElementData
    {
        public int SomeValue;
    }
    
    public struct TestSharedAuthoring : ISharedComponentData, IEquatable<TestSharedAuthoring>
    {
        public int SomeValue;
        
        public bool Equals(TestSharedAuthoring other)
        {
            return SomeValue == other.SomeValue;
        }
        
        public override int GetHashCode()
        {
            return SomeValue.GetHashCode();
        }
    }
}
