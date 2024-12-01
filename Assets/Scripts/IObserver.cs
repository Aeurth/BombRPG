
public interface IObserver
{
    void UpdateData(GameData gameData);
    
}
public class GameData
{
    public string LevelName;
    public int ObsticlesCount;
    public int MaxBombCount;
    public int PlacedBombCount;
    public int ExplotionRange;
    public int BonusPlayerSpeed;

    public GameData (string levelName= "Default", int obsticlesCount = 0, int maxBombCount = 1,
        int placedBombCount = 0, int explotionRange = 1, int bonusPlayerSpeed = 0)
    {
        LevelName = levelName;
        ObsticlesCount = obsticlesCount;
        MaxBombCount = maxBombCount;
        PlacedBombCount = placedBombCount;
        ExplotionRange = explotionRange;
        BonusPlayerSpeed = bonusPlayerSpeed;
    }
}