using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class LevelEditWindow : EditorWindow
{

    private Level m_currentlevel;
    private LevelsList m_levelsListObj;
    private Sprite[] m_images = new Sprite[4];

    private int m_currentLevelIndex = 0;
    private int m_lastLevelIndex = 0;

    private string[] m_options;
    private string m_currentLevelName = string.Empty;
    private Answer[] m_currentLevelAnswer;

    private string m_lastError = string.Empty;

    private void OnGUI()
    {
        Draw();
    }

    public void SetValues(LevelsList levels)
    {
        m_levelsListObj = levels;

        //SelectLevel(0);
        Answer ansRus = new Answer("", AnswerLang.Russian);
        Answer ansEng = new Answer("", AnswerLang.English);

        m_currentLevelAnswer = new Answer[2];
        m_currentLevelAnswer[0] = ansRus;
        m_currentLevelAnswer[1] = ansEng;

        SetLevelOptionsString();

        if (m_levelsListObj.m_levels.Count > 0)
        {
            SelectLevel(0);
        }
    }

    private void Draw()
    {
        DrawLevelsSelectButtons();
        DrawImagesField();
        DrawEditSettings();
        DrawHelpLabel();
    }


    private void DrawHelpLabel()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(m_lastError);
        GUILayout.EndHorizontal();
    }
    private void DrawLevelsSelectButtons()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<--"))
        {
            PreviousLevel();
        }

        m_currentLevelIndex = EditorGUILayout.Popup(m_currentLevelIndex, m_options);
        if(m_lastLevelIndex != m_currentLevelIndex)
        {
            SelectLevel(m_currentLevelIndex);
        }

        if (GUILayout.Button("-->"))
        {
            NextLevel();
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        for (int i = 0; i < m_currentLevelAnswer.Length; i++ )
        {
            GUILayout.BeginVertical();
            GUILayout.Label(m_currentLevelAnswer[i].lang.ToString() + " Answer");
            m_currentLevelAnswer[i].ans = GUILayout.TextField(m_currentLevelAnswer[i].ans, 10);
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        
        
        GUILayout.BeginVertical();
        GUILayout.Label("Имя уровня");
        m_currentLevelName = GUILayout.TextField(m_currentLevelName,25);
        GUILayout.EndVertical();


    }
    private void DrawImagesField()
    {
        GUILayout.BeginHorizontal();
        for (int i = 0; i < 2; i++ )
        {
            GUILayout.BeginVertical();
            m_images[i] = (Sprite)EditorGUILayout.ObjectField("Image " + (i+1).ToString(), m_images[i], typeof(Sprite), true);
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        for (int i = 2; i < 4; i++)
        {
            GUILayout.BeginVertical();
            m_images[i] = (Sprite)EditorGUILayout.ObjectField("Image " + (i + 1).ToString(), m_images[i], typeof(Sprite), true);
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }

    private void DrawEditSettings()
    {
        GUILayout.BeginVertical();
      
        GUILayout.BeginHorizontal();

        if(GUILayout.Button("New"))
        {
            NewLevel();
        }

        if (GUILayout.Button("Delete"))
        {
            DeleteLevel(m_currentLevelIndex);
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Save"))
        {
            Save();
        }

        GUILayout.EndVertical();
    }

    private void SetLevelOptionsString()
    {
        m_options = new string[m_levelsListObj.m_levels.Count];
        for (int i = 0; i < m_levelsListObj.m_levels.Count; i++)
        {
            m_options[i] = m_levelsListObj.m_levels[i].GetName;
        }
    }

    private void NextLevel()
    {
        if(m_currentLevelIndex < m_levelsListObj.m_levels.Count - 1)
        {
            m_currentLevelIndex += 1;
            SelectLevel(m_currentLevelIndex);
            SetLevelOptionsString();
        }
        else
        {
            m_lastError = "Данный уровень последний или уровней 0";
        }
    }

    private void PreviousLevel()
    {
        if (m_currentLevelIndex > 0)
        {
            m_currentLevelIndex -= 1;
            SelectLevel(m_currentLevelIndex);
            SetLevelOptionsString();
        }
        else
        {
            m_lastError = "Данный уровень первый или уровней 0";
        }
    }

    private void Save()
    {
        Level newLevel = new Level(m_images, m_currentLevelAnswer, m_currentlevel.GetId, m_currentLevelName);
        m_levelsListObj.m_levels[m_currentLevelIndex] = newLevel;

        m_currentlevel = newLevel;
        //m_currentLevelIndex = m_levelsListObj.m_levels.Count - 1;

        SelectLevel(m_currentLevelIndex);
        SetLevelOptionsString();

        m_lastError = string.Format("Изменения в уровне \"{0}\" сохранены",m_currentlevel.GetName);
    }

    private bool HaveEmptyFields()
    {
        return false; //delete

        if( m_currentLevelName == string.Empty)
        {
            return true;
        }

        for (int i = 0; i < m_images.Length; i++ )
        {
            if(m_images[i] == null)
            {
                return true;
            }
        }

        for (int i = 0; i < m_currentLevelAnswer.Length;i++ )
        {
            if(m_currentLevelAnswer[i].ans == string.Empty)
            {
                return true;
            }
        }
        return false; 
    }
    
    private void NewLevel()
    {
        int lastId = 0;
        try
        {
            lastId = m_levelsListObj.m_levels.OrderBy(x => x.GetId).Last().GetId + 1; 
        }
        catch
        {
            lastId = 0;
        }

        Level newLevel = m_levelsListObj.m_levels.Find(l => l.GetName == m_currentLevelName);
        if (newLevel == null)
        {
            if( !HaveEmptyFields() )
            {
                newLevel = new Level(m_images, m_currentLevelAnswer, lastId, m_currentLevelName);
                m_levelsListObj.m_levels.Add(newLevel);

                m_currentlevel = newLevel;
                m_currentLevelIndex = m_levelsListObj.m_levels.Count - 1;

                SelectLevel(m_currentLevelIndex);
                SetLevelOptionsString();

                m_lastError = string.Format("Уровень \"{0}\" , был создан", newLevel.GetName);
            }
            else
            {
                m_lastError = "Невозможно создать уровень, есть незаполненные поля";
            }
        }
        else
        {
            m_lastError = string.Format("Уровень с таким именем \"{0}\" уже есть ", newLevel.GetName);
        }
        

    }

    private void SelectLevel(int index)
    {
        m_currentlevel = m_levelsListObj.m_levels[index];

        m_currentLevelAnswer = m_currentlevel.GetAnswer;
        m_currentLevelName = m_currentlevel.GetName;
        m_images = m_currentlevel.GetImages;

        m_currentLevelIndex = index;
        m_lastLevelIndex = m_currentLevelIndex;

    }
        
    private void DeleteLevel(int index)
    {
        if (index >= 0 && m_levelsListObj.m_levels.Count > 0)
        {
            m_lastError = string.Format("Уровень \"{0}\" , был удалён", m_levelsListObj.m_levels[index].GetName);
            m_levelsListObj.m_levels.Remove(m_levelsListObj.m_levels[index]);

            if (m_levelsListObj.m_levels.Count > 0)
            {
                SelectLevel(0);
                SetLevelOptionsString();
            }
            else
            {

            }
          
        }
        else
        {
            m_lastError = string.Format("Уровеня \"{0}\" , не существует или уровней 0", index);
        }

    }

}
