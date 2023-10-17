using Unity.Entities;
using UnityEngine;

namespace StateMachine
{
    public class PlayerMono : MonoBehaviour
    {
        public float Speed = 5.0f;
    }
    
    public class PlayerMonoBaker : Baker<PlayerMono>
    {
        public override void Bake(PlayerMono authoring)
        {
            var playerEntity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
            
            // AddComponentObject();
            
            AddComponent<PlayerTag>(playerEntity);
            AddComponent(playerEntity, new UserInputData());
            AddComponent(playerEntity, new MovementSpeed()
            {
                MeterPerSecond = authoring.Speed,
            });
        }
    }
}