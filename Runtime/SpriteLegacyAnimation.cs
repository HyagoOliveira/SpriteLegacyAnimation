using UnityEngine;

namespace ActionCode.SpriteLegacyAnimation
{
    /// <summary>
    /// Animation component for legacy Sprite Animations Clips. 
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class SpriteLegacyAnimation : AbstractLegacyAnimation
    {
        [SerializeField, Tooltip("Sprite Renderer used to animate Sprite frames.")]
        private SpriteRenderer spriteRenderer;

        public override Sprite Sprite
        {
            get => spriteRenderer.sprite;
            protected set => spriteRenderer.sprite = value;
        }

        protected override void Reset()
        {
            base.Reset();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}