using Unity.Entities;
using UnityEngine;

namespace Script.Move
{
    public class MoveMono : MonoBehaviour
    {
        public float speed;
    }
    
    public class MoveBaker : Baker<MoveMono>
    {
        public override void Bake(MoveMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(entity, new MoveAuthoring()
            {
                Speed = authoring.speed,
                Time = 0f,
            });
            
            AddComponent<MoveTag>(entity);
        }
    }
}
