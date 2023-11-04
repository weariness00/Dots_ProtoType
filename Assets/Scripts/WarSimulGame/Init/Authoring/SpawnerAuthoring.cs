using Unity.Entities;
using UnityEngine;

namespace WarSimulGame.Init.Authoring
{
    public class SpawnerAuthoringAuthoring : MonoBehaviour
    {
        public GameObject[] monster;
    }

    public class SpawnerAuthoringBaker : Baker<SpawnerAuthoringAuthoring>
    {
        public override void Bake(SpawnerAuthoringAuthoring authoring)
        {
        }
    }
}