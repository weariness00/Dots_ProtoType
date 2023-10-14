using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace StateMachine
{
    public partial struct SystemChanger : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            var PlayerState = SystemAPI.ManagedAPI.GetComponent<StateMachine>(playerEntity);
            
            PlayerState.CurrentState = new IdleState();
            // PlayerState.GlobalState = new IdleState();
            // PlayerState.PreviousState = new IdleState();
        }
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        
        }
    }
}