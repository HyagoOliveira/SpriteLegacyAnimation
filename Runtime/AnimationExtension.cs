using UnityEngine;
using System.Collections;

namespace ActionCode.SpriteLegacyAnimation
{
    /// <summary>
    /// Extension class for <see cref="Animation"/>.
    /// </summary>
    public static class AnimationExtension
    {
        /// <summary>
        /// Plays the given animation (the default one) and waits until it finishes.
        /// </summary>
        /// <param name="animation"></param>
        /// <returns>An IEnumerator coroutine.</returns>
        public static IEnumerator PlayAndWait(this Animation animation)
        {
            animation.Play();
            yield return WaitAnimation(animation);
        }

        /// <summary>
        /// Plays the given animation name and waits until it finishes.
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="name">The animation name to be played.</param>
        /// <returns><inheritdoc cref="PlayAndWait(Animation)"/></returns>
        public static IEnumerator PlayAndWait(this Animation animation, string name)
        {
            animation.Play(name);
            yield return WaitAnimation(animation);
        }

        private static IEnumerator WaitAnimation(Animation animation) =>
            new WaitUntil(() => !animation.isPlaying);
    }
}