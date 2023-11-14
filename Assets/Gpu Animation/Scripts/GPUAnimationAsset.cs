using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Gpu_Animation.Scripts
{
    [CreateAssetMenu(fileName = "GPU", menuName = "Animation Instancing", order = 0)]
    public class GPUAnimationAsset : ScriptableObject
    {
        public string path = "GPU Animation/Animation Asset";
        [ReadOnly] public GameObject Model;
        public AnimationClip[] clips;

        private List<Texture2D> _textures = new List<Texture2D>();
        private List<GameObject _gameObjects = new List<GameObject>();
        private int width = 0;
        private int height = 0;

        public void Bake()
        {
            // Mesh mesh = GetMesh();
            // Material[] materials = GetMaterials();
            //
            // if (SystemInfo.maxTextureSize < mesh.vertexCount)
            // {
            //     Debug.LogWarning($"{SystemInfo.maxTextureSize}보다 버텍스 갯수가 더 많습니다.\nVertextCount : {mesh.vertexCount}");
            //     return;
            // }
            //
            // var prefab = new GameObject();
            // prefab.AddComponent<MeshFilter>().sharedMesh = mesh;
            // prefab.AddComponent<MeshRenderer>().sharedMaterials = materials;
            //
            // width = mesh.vertexCount;
            // foreach (var clip in clips)
            //     height += (int)(clip.length * clip.frameRate);
            //
            // // RenderTexture 생성
            // Camera myCamera = Camera.main;
            // RenderTexture rt = new RenderTexture(width, height, 24);
            // myCamera.targetTexture = rt;
            // RenderTexture.active = rt;
            // int sumFrame = 0;
            //
            // Texture2D frameTexture = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
            //
            // foreach (AnimationClip clip in clips)
            // {
            //     for (int frame = 0; frame < clip.length * clip.frameRate; frame += 1)
            //     {
            //         clip.SampleAnimation(Model, frame);
            //
            //         // 프레임 렌더링
            //         myCamera.Render();  
            //
            //         // RenderTexture를 Texture2D로 변환
            //         frameTexture.ReadPixels(new Rect( 0,frame + sumFrame, rt.width,frame + sumFrame + 1), 0, frame + sumFrame);
            //         frameTexture.Apply();
            //
            //         // Clear the active RenderTexture
            //     }
            //     sumFrame += (int)clip.length;
            // }
            // System.IO.File.WriteAllBytes($"Assets/{path}/RenderTexture_{Model.name}.png", frameTexture.EncodeToPNG());
            // RenderTexture.active = null;
            //
            // PrefabUtility.SaveAsPrefabAsset(prefab, $"Assets/{path}/Obj_{Model.name}.prefab");
            // DestroyImmediate(prefab);
            //
            // myCamera.targetTexture = null;
            var meshRenderers = Model.GetComponentsInChildren<MeshRenderer>();
            if (meshRenderers.Length > 0)
            {
                foreach (var meshRenderer in meshRenderers)
                {
                    MakeAnimationTexture(meshRenderer);
                }

                return;
            }
            var skinned = Model.GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        void MakeAnimationTexture(MeshRenderer meshRenderer)
        {
            var mesh = meshRenderer.gameObject.GetComponent<MeshFilter>().mesh;
            if (SystemInfo.maxTextureSize < mesh.vertexCount)
            {
                Debug.LogWarning($"{SystemInfo.maxTextureSize}보다 버텍스 갯수가 더 많습니다.\nMeshName : {mesh.name}\nVertextCount : {mesh.vertexCount}");
                return;
            }

            var material = meshRenderer.material;

            Camera myCamera = Camera.main;
            RenderTexture rt = new RenderTexture(width, height, 24);
            myCamera.targetTexture = rt;
            RenderTexture.active = rt;
            int sumFrame = 0;

            Texture2D frameTexture = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);

            foreach (AnimationClip clip in clips)
            {
                for (int frame = 0; frame < clip.length * clip.frameRate; frame += 1)
                {
                    clip.SampleAnimation(Model, frame);

                    // 프레임 렌더링
                    myCamera.Render();

                    // RenderTexture를 Texture2D로 변환
                    frameTexture.ReadPixels(new Rect(0, frame + sumFrame, rt.width, frame + sumFrame + 1), 0,
                        frame + sumFrame);
                    frameTexture.Apply();

                    // Clear the active RenderTexture
                }

                sumFrame += (int)clip.length;
            }

            _textures.Add(frameTexture);

            RenderTexture.active = null;
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

        Material[] GetMaterials()
        {
            if (Model.TryGetComponent<MeshRenderer>(out var meshRenderer))
                return meshRenderer.materials;

            var skinnedMeshRenderers = Model.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRenderers.Length != 0)
            {
                Material[] materials = new Material[0];
                foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
                    materials = materials.Concat(skinnedMeshRenderer.sharedMaterials).ToArray();
                HashSet<Material> set = new HashSet<Material>(materials);
                return set.ToArray();
            }

            return null;
        }
    }
}