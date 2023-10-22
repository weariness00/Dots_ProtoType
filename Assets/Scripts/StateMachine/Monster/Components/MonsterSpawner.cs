using Unity.Entities;
using UnityEngine;

namespace StateMachine.Monster
{
    public struct MonsterSpawner : IComponentData
    {
        public Entity MonsterGameObject;
    }
}