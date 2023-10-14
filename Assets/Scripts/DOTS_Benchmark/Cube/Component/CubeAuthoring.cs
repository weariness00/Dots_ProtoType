using Unity.Entities;
using Unity.Mathematics;

namespace DOTS_Benchmark
{
    public struct SpawnCubeTag : IComponentData {}
    public struct FirstSpawnCubeTag : IComponentData {}
    public struct CubeTag : IComponentData {}
    
    
    public struct CubeAuthoring : IComponentData
    {
        public float Time;
        public float Radian;
        public MoveVector Vector;
    }
    
    public struct MoveVector : IComponentData
    {
        public float X;
        public float Y;
        public float Z;
    }
}