using UnityEngine;

[CreateAssetMenu(fileName = "New Speaker", menuName = "Dialogue/Speaker")]
public class SpeakerScriptableObject : ScriptableObject
{
    public string Name;

    [Tooltip("0 - normal | 1 - falando | 2 - pensando")]
    public Sprite[] Sprite;
}