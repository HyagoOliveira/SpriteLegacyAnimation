using UnityEngine;

namespace ActionCode.SpriteLegacyAnimation
{
    /// <summary>
    /// Abstract class to provide legacy animation behaviour.
    /// Animates the <see cref="index"/> property, allowing legacy animations.
    /// </summary>
    [ExecuteAlways]
    [DisallowMultipleComponent]
#if ANIMATION_MODULE
    [RequireComponent(typeof(Animation))]
#endif
    public abstract class AbstractLegacyAnimation : MonoBehaviour
    {
#if ANIMATION_MODULE
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        [Tooltip("The local legacy Animation component.")]
        public Animation animation;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
#endif
        [Tooltip("The index used to animate the Sprite list.")]
        public int index;
        [Tooltip("The Sprites used on animation.")]
        public Sprite[] sprites = new Sprite[0];

        /// <summary>
        /// The Sprite been rendered.
        /// </summary>
        public abstract Sprite Sprite { get; protected set; }

        /// <summary>
        /// Whether it has Sprites set.
        /// </summary>
        public bool HasSprites => sprites.Length > 0;

        protected virtual void Reset()
        {
#if ANIMATION_MODULE
            animation = GetComponent<Animation>();
#endif
        }

#if ANIMATION_MODULE
        private void OnEnable()
        {
            if (animation) animation.enabled = true;
        }

        private void OnDisable() => animation.enabled = false;
#endif

        private void LateUpdate()
        {
            var isValidIndex = index > -1 && index < sprites.Length;
            if (!isValidIndex) return;

            Sprite = sprites[index];
        }

        private void OnValidate() => index = Mathf.Clamp(index, 0, sprites.Length - 1);
    }
}
