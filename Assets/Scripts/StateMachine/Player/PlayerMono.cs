using Unity.Entities;
using UnityEngine;

namespace StateMachine
{
    public class PlayerMono : MonoBehaviour
    {
        
    }
    
    public class PlayerMonoBaker : Baker<PlayerMono>
    {
        public override void Bake(PlayerMono authoring)
        {
            var playerEntity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
            
            AddComponent<PlayerTag>(playerEntity);
            SystemAPI.ManagedAPI.GetComponent<StateMachine>(playerEntity);
        }
    }
}