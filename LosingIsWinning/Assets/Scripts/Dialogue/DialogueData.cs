using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueData : ScriptableObject
{
    [Header("Dialogue")]
    public List<Dialogue> m_Dialogues = new List<Dialogue>();
}
