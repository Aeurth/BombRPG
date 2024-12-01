using UnityEngine;

[CreateAssetMenu]
public class LevelConfig : ScriptableObject
{
    public string levelName;
    public int gridSizeX;
    public int gridSizeY;
    public int PWradiusCount;
    public int PWspeedCount;
    public int PWmaxBombsCount;
    public int DestructiblesCount;
    public Vector2Int[] SpawnPoints;



}
