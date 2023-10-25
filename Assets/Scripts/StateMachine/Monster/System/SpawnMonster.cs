using Unity.Burst;
using Unity.Entities;

namespace StateMachine.Monster.System
{
    public partial struct SpawnMonster : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MonsterSpawner>();
            state.RequireForUpdate<SpawnPlacePoint>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var spawnerBuffer = SystemAPI.GetSingletonBuffer<SpawnPlacePoint>();
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            // var newEntity = entityManager.Instantiate();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}