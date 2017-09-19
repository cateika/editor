using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Levels", menuName = "Level/List", order = 1)]
public class LevelsList : ScriptableObject 
{
    public List<Level> m_levels = new List<Level>();
}
