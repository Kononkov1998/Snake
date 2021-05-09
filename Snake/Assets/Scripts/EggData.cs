using UnityEngine;

[CreateAssetMenu(fileName = "Egg", menuName = "Snake/Egg")]
public class EggData : ScriptableObject
{
    public int partsToAdd;
    public Sprite sprite;
}
