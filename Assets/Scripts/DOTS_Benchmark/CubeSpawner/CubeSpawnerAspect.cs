using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace DOTS_Benchmark
{
    public readonly partial struct CubeSpawnerAspect : IAspect
    {
        public readonly Entity Entity;

        public readonly RefRO<CubeSpawnerAuthoring> NoteSpawnerAuthoring;
            
        public readonly RefRO<CubeSpawnerTag> Tag;
        public readonly RefRW<LocalTransform> Transform;
    }
}