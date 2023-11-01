using Dots_Animator_System.Test;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct TestSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        var shared = new TestSharedAuthoring() { SomeValue = 10 };
        foreach (var (tag, entity) in SystemAPI.Query<TestInitTag>().WithEntityAccess())
        {
            if(SystemAPI.HasBuffer<Child>(entity) == false) continue;
            var childs = SystemAPI.GetBuffer<Child>(entity);
            foreach (var child in childs)
            {
                Debug.Log(child.Value);
            }
        }
    }
}
