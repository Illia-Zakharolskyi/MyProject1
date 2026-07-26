using UnityEditor;
using UnityEngine;

namespace Task.Raycast.Second
{
    [CustomEditor(typeof(Target))]
    public class TargetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Target targetScript = (Target)target;

            EditorGUILayout.Space(15);
            EditorGUILayout.LabelField("Prefabs for targets (Aim Trainer)", EditorStyles.boldLabel);

            if (GUILayout.Button("Prefab: Close Target"))
            {
                ApplyPreset(targetScript, scale: 1.2f, speed: 30f, amplitude: 48f);
            }

            if (GUILayout.Button("Prefab: Medium Target"))
            {
                ApplyPreset(targetScript, scale: 2f, speed: 12f, amplitude: 20f);
            }

            if (GUILayout.Button("Prefab: Long Target"))
            {
                ApplyPreset(targetScript, scale: 4.5f, speed: 6f, amplitude: 12f);
            }

            if (GUILayout.Button("Prefab: Hard Target"))
            {
                ApplyPreset(targetScript, scale: 1.5f, speed: 16f, amplitude: 3f);
            }
        }

        private void ApplyPreset(Target target, float scale, float speed, float amplitude)
        {
            Undo.RecordObject(target, "Apply Target Preset");
            Undo.RecordObject(target.transform, "Apply Target Scale");

            target.transform.localScale = new Vector3(scale, scale, 1f);
            target.SetPhysicsParams(speed, amplitude);

            EditorUtility.SetDirty(target);
        }

        private void OnSceneGUI()
        {
            Target targetScript = (Target)target;
            if (targetScript == null) return;

            Vector3 center = targetScript.transform.position;

            float amp = targetScript.MoveAmplitude;

            Vector3 leftBound = center - targetScript.transform.right * amp;
            Vector3 rightBound = center + targetScript.transform.right * amp;

            Handles.color = Color.green;
            Handles.DrawLine(leftBound, rightBound);

            Handles.DrawWireCube(leftBound, Vector3.one * 0.5f);
            Handles.DrawWireCube(rightBound, Vector3.one * 0.5f);
        }
    }
}