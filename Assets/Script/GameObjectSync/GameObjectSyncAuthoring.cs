using Unity.Entities;
using UnityEngine;

namespace Script.GameObjectSync
{
    public struct GameObjectSyncTag : IComponentData {}

    public class GameObjectSyncManagedAuthoring : IComponentData
    {
        public GameObject GameObject;
        public Transform Transform;
    }
}
