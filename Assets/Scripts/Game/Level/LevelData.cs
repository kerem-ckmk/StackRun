using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelData", menuName = "Raising Game/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public int ID;
    public Level LevelPrefab;
}
