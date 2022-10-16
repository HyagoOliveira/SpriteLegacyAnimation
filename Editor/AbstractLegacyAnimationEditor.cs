using UnityEditor;
using UnityEngine;

namespace ActionCode.SpriteLegacyAnimation.Editor
{
    /// <summary>
    /// Custom editor for <see cref="AbstractLegacyAnimation"/> and its children.
    /// </summary>
    [CustomEditor(typeof(AbstractLegacyAnimation), editorForChildClasses: true)]
    public sealed class AbstractLegacyAnimationEditor : UnityEditor.Editor
    {
        private AbstractLegacyAnimation animation;
        private SerializedProperty spritesProperty;

        private readonly string spritesPropertyName;

        public AbstractLegacyAnimationEditor()
        {
            spritesPropertyName = nameof(AbstractLegacyAnimation.sprites);
        }

        private void OnEnable()
        {
            animation = target as AbstractLegacyAnimation;
            spritesProperty = serializedObject.FindProperty(spritesPropertyName);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, spritesPropertyName);
            // Draws Sprites property in the Inspector end.
            EditorGUILayout.PropertyField(spritesProperty);

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(!animation.HasSprites);

            if (GUILayout.Button("Create Animation Clip")) CreateClip(animation);

            EditorGUI.EndDisabledGroup();
        }

        private void CreateClip(AbstractLegacyAnimation animation)
        {
            const int framesBySprite = 3;
            const float frameRate = 60F;
            const float framesBySecond = 1F / frameRate;
            const float timeBySprite = framesBySprite * framesBySecond;

            var currentKeyTime = 0F;
            var sprites = animation.sprites;
            var gameObject = animation.gameObject;
            var indexPropertyName = nameof(animation.index);
            var keyFrames = new Keyframe[sprites.Length + 1];
            var animationClip = new AnimationClip { frameRate = frameRate, legacy = true, wrapMode = WrapMode.Loop };
            var hierarchyPath = AnimationUtility.CalculateTransformPath(animation.transform, gameObject.transform);
            var binding = EditorCurveBinding.FloatCurve(
                hierarchyPath,
                animation.GetType(),
                indexPropertyName
            );

            static Keyframe CreateKeyframe(float time, float value)
            {
                return new Keyframe(
                    Truncate(time),
                    value,
                    inTangent: float.PositiveInfinity,
                    outTangent: float.PositiveInfinity,
                    inWeight: 0,
                    outWeight: 0
                )
                {
                    weightedMode = WeightedMode.None
                };
            }

            for (int i = 0; i < sprites.Length; i++)
            {
                keyFrames[i] = CreateKeyframe(currentKeyTime, i);
                currentKeyTime += timeBySprite;
            }

            var lastKeyframeTime = currentKeyTime - framesBySecond;
            keyFrames[^1] = CreateKeyframe(lastKeyframeTime, sprites.Length - 1);

            var curve = new AnimationCurve(keyFrames);
            for (int i = 0; i < keyFrames.Length; i++)
            {
                AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
                AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
            }

            AnimationUtility.SetEditorCurve(animationClip, binding, curve);

            var wasClipSaved = TrySaveClip(animationClip);
            if (!wasClipSaved) return;

            if (animation.animation == null)
            {
                animation.animation = gameObject.GetComponent<Animation>();
                if (animation.animation == null) gameObject.AddComponent<Animation>();
            }

            animation.animation.clip = animationClip;
            AnimationUtility.SetAnimationClips(
                animation.animation,
                new AnimationClip[] { animationClip }
            );
        }

        private static bool TrySaveClip(AnimationClip clip)
        {
            const string extension = "anim";
            const string defaultPath = "Assets/Animations";

            var path = EditorUtility.SaveFilePanelInProject(
               title: "New Animation",
               defaultName: "NewAnimation." + extension,
               extension,
               message: "Creates a new animation asset",
               defaultPath
            ).Trim();

            var hasInvalidPath = string.IsNullOrEmpty(path);
            if (hasInvalidPath) return false;

            AssetDatabase.CreateAsset(clip, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return true;
        }

        private static float Truncate(float value) => Mathf.Floor(value * 100F) / 100F;
    }
}