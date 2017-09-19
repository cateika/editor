using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Level {

    public string GetName { get { return m_name; } }
    public Answer[] GetAnswer { get { return m_answer; } }
    public int GetId { get { return m_id; } }
    public Sprite[] GetImages { get { return m_images; } }

    [SerializeField]
    private int m_id;

    [SerializeField] 
    private Answer[] m_answer;

    [SerializeField] 
    private Sprite[] m_images;

    [SerializeField] 
    private string m_name;
    
    

    public Level (Sprite[] images, Answer[] answer, int id, string name)
    {
        if (images.Length != 4)
            throw new Exception("Wrong images array size");
        this.m_answer = answer;
        this.m_images = images;
        this.m_id = id;
        this.m_name = name;
    }

    public void ChangeId(int id)
    {
        this.m_id = id;
    }

}
