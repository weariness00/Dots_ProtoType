using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Gpu_Animation.Scripts
{
    [CreateAssetMenu(fileName = "GPU", menuName = "Animation Instancing", order = 0)]
    public class GPUAnimationAsset : ScriptableObject
    {
        public string path = "GPU Animation/Animation Asset";
        [ReadOnly] public GameObject Model;
        public AnimationClip[] Clips;

        public void Bake()
        {
            Mesh mesh = GetMesh();
            GameObject newObject = new GameObject();
            RenderTexture renderTexture = new RenderTexture(512,512, GraphicsFormat.D32_SFloat_S8_UInt,GraphicsFormat.RGB_BC6H_UFloat);
        }

        Mesh GetMesh()
        {
            if (Model.TryGetComponent<MeshFilter>(out var meshFilter))
                return meshFilter.mesh;

            var skinnedMeshRenderers = Model.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRenderers.Length != 0)
            {
                Mesh mesh = new Mesh();
                CombineInstance[] combine = new CombineInstance[skinnedMeshRenderers.Length];
                for (int i = 0; i < combine.Length; i++)
                {
                    combine[i].mesh = skinnedMeshRenderers[i].sharedMesh;
                    combine[i].transform = skinnedMeshRenderers[i].localToWorldMatrix;
                }
                mesh.CombineMeshes(combine);
                UnityEditor.AssetDatabase.CreateAsset(mesh, $"Assets/{path}/Combine_{Model.name}.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                return mesh;
            }
            
            Debug.LogError($"Not Have Mesh : {Model.name}");
            return null;
        }
    }
}   