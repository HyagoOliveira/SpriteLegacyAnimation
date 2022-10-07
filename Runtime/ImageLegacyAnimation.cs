#if UI_MODULE
using UnityEngine;
using UnityEngine.UI;

namespace ActionCode.SpriteLegacyAnimation
{
    /// <summary>
    /// UI Image Legacy Animation component for legacy Sprite Animations Clips. 
    /// </summary>
    [RequireComponent(typeof(Image))]
    public sealed class ImageLegacyAnimation : AbstractLegacyAnimation
    {
        [SerializeField, Tooltip("Image Renderer component used to animate Sprite frames.")]
        private Image imageRenderer;

        public override Sprite Sprite
        {
            get => imageRenderer.sprite;
            protected set => imageRenderer.sprite = value;
        }

        protected override void Reset()
        {
            base.Reset();
            imageRenderer = GetComponent<Image>();
        }
    }
}
#endif