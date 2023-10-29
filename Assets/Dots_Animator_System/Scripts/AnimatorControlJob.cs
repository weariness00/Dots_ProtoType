using Unity.Entities;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public partial struct AnimatorStateJob : IJobEntity
    {
        private void Execute(AnimatorSync animatorSync, [ChunkIndexInQuery]int sortkey)
        {
            foreach (var state in animatorSync.LayerAuthorings[0].AnimatorStates)
            {
                if (state.Equals(animatorSync.LayerAuthorings[0].DefaultStateHashCode))
                {
                    foreach (var clip in animatorSync.LayerAuthorings[0].AnimationClip)
                    {
                        if (clip.Equals(state.AniamtionClipHashCode))
                        {
                            // 이 클립에 해당하는 키 프레임들의 해당 본을 움직이게 해주기
                            // 현재는 테스트 단계임
                            
                            break;
                        }
                    }
                }
            }
        }
    }
}
