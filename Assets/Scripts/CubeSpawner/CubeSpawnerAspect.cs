using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace Game
{
    public readonly partial struct NoteSpawnerAspect : IAspect
    {
        public readonly Entity Entity;

        public readonly RefRO<CubeSpawnerAuthoring> NoteSpawnerAuthoring;
            
        public readonly RefRO<CubeSpawnerTag> Tag;
        public readonly RefRW<LocalTransform> Transform;
    }
}