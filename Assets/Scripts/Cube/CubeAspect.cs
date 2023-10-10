using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.Rendering;

namespace Game
{
    public readonly partial struct CubeAspect : IAspect
    {
        public readonly Entity Entity;

        public readonly RefRO<CubeTag> NoteTag;
        public readonly RefRO<SpawnCubeTag> SpawnNoteTag;
        
        private readonly RefRW<LocalTransform> Transform;
        private readonly RefRW<CubeTagAuthoring> NoteAuthoring;        

        public float3 Position
        {
            get => Transform.ValueRO.Position;
            set => Transform.ValueRW.Position = value;
        }
    }
}