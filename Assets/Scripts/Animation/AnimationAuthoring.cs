using Unity.Entities;
using UnityEngine;

namespace Game
{
    public struct PlayerTag : IComponentData {}
    // public class DotsAnimatorAuthoring : IComponentData
    // {
    //     public Animator ani;
    // }

    // ICleanupComponentData이 Class을 상속을 받으면 객체나 다른 Component을 삭제할때와 다르게 완전히 삭제가 되지않고 
    // 나중에 수행할 요소를 위해서 수동으로 제거하기 전까지는 해당 Entity에 존재하고 있다.
    // ==> IComponentData을 상속받는 Component는 Remove하면 해당 Entity에서 완전히 삭제가 된다.
    public class DotsAnimatorAuthoring : ICleanupComponentData
    {
        public Animator ani;
    }
    
    public class AnimatorSyncObject : IComponentData
    {
        public GameObject gameObject;
    }
}