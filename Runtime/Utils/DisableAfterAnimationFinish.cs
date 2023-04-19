using UnityEngine;
using System.Collections;

namespace ActionCode.SpriteLegacyAnimation
{
    /// <summary>
    /// Disable this GameObject after the local Animation component finishes to play.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animation))]
    public sealed class DisableAfterAnimationFinish : MonoBehaviour
    {
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        [SerializeField] private Animation animation;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        private void Reset() => animation = GetComponent<Animation>();
        private void OnEnable() => StartCoroutine(DisableGameObjectAfterAnimationIsFinished());

        private IEnumerator DisableGameObjectAfterAnimationIsFinished()
        {
            yield return animation.PlayAndWait();
            gameObject.SetActive(false);
        }
    }
}