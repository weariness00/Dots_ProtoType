using UnityEngine;
using Unity.Entities;

namespace Game
{
    public class DotsAnimatorMono : MonoBehaviour
    {
        public GameObject Prefab;
    }

    public class AnimatorBaker : Baker<DotsAnimatorMono>
    {
        public override void Bake(DotsAnimatorMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // Class Component을 추가를 할때는 AddComponentObject함수를 사용한다.
            // AddComponent함수는 Struct Component을 추가를 할때 사용을 한다.
            // AddComponentObject(entity, new DotsAnimatorAuthoring() { ani = authoring.GetComponent<Animator>() });
            AddComponentObject(entity, new AnimatorSyncObject() { gameObject = authoring.Prefab });
        }
    }
}