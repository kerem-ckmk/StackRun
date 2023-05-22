using UnityEngine;

[CreateAssetMenu(fileName = "New LevelData", menuName = "Raising Game/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public int ID;
    public Level LevelPrefab;
    public float StackCount;
}
