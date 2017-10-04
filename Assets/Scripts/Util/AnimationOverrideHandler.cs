namespace ShiftingDungeon.Util
{
    using System.Collections.Generic;
    using UnityEngine;

    public class AnimationOverrideHandler
    {
        /// <summary> The override controller for the given animator. </summary>
        public AnimatorOverrideController AnimatorOverrideController { get; private set; }

        private AnimationClipOverrides clipOverrides;

        /// <summary> Constructs an Animation Override Handler. </summary>
        /// <param name="anim"> The animator to override clips from. </param>
        public AnimationOverrideHandler(Animator anim)
        {
            this.AnimatorOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
            this.AnimatorOverrideController.name = "Overriden Controller";
            anim.runtimeAnimatorController = this.AnimatorOverrideController;
            this.clipOverrides = new AnimationClipOverrides(this.AnimatorOverrideController.overridesCount);
            this.AnimatorOverrideController.GetOverrides(clipOverrides);
        }

        /// <summary> Sets a clip to be overriden. </summary>
        /// <param name="clipName"> The clip to override. </param>
        /// <param name="clip"> The new clip. </param>
        public void OverrideClip(AnimationClip clipName, AnimationClip clip)
        {
            this.clipOverrides[clipName] = clip;
        }

        /// <summary> Sets the given clips to be overriden. </summary>
        /// <param name="clipName"> The clips to override. </param>
        /// <param name="clip"> The new clips. </param>
        public void OverrideClips(AnimationClip[] clipNames, AnimationClip[] clips)
        {
            for (int i = 0; i < clipNames.Length; i++)
                this.clipOverrides[clipNames[i]] = clips[i];
        }

        /// <summary> Applies the current overrides to the animator. </summary>
        public void ApplyOverrides()
        {
            this.AnimatorOverrideController.ApplyOverrides(this.clipOverrides);
        }

        private class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
        {
            public AnimationClipOverrides(int capacity) : base(capacity) { }

            public AnimationClip this[AnimationClip clip]
            {
                get { return this.Find(x => x.Key.Equals(clip)).Value; }
                set
                {
                    int index = this.FindIndex(x => x.Key.Equals(clip));
                    if (index != -1)
                        this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
                }
            }
        }
    }
}
