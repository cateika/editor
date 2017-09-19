using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(LevelsList))]
public class LevelsEditor : Editor {

    private LevelsList m_levels;
    private LevelEditWindow m_editWindow;
    private void OnEnable()
    {
        m_levels = (LevelsList)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal ();
        if (GUILayout.Button("Open Levels Editor"))
        {
            OpenLevelEditorWindow();
        }
        GUILayout.EndHorizontal();

        DrawDefaultInspector();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(m_levels);
            serializedObject.ApplyModifiedProperties();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }

    private void OpenLevelEditorWindow()
    {
        if(m_editWindow == null)
        {
            m_editWindow = EditorWindow.CreateInstance<LevelEditWindow>();
            EditorWindow.GetWindow<LevelEditWindow>();
            m_editWindow.SetValues(m_levels);
        }
    }


}
