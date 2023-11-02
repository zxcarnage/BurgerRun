using UnityEngine;

public class ServiceLocator
{
    private static LevelEndCanvas _loseCanvasInstance;

    public static LevelSpawner LevelSpawner { get; set; }
    public static InterstitialAdsManager AdsManager { get; set; }
    public static PaginationScroll Shop { get; set; }
    public static StartPanel InGameUI { get; set; }

    public static Player Player { get; set; }

    private static SpawnPoint _spawnPointInstance;

    public static SpawnPoint SpawnPoint => _spawnPointInstance;

    public static void SetEndCanvas(LevelEndCanvas canvas)
    {
        _loseCanvasInstance = canvas;
    }

    public static void SetSpawnPoint(SpawnPoint spawnPoint)
    {
        _spawnPointInstance = spawnPoint;
    }

    public static LevelEndCanvas GetEndCanvas()
    {
        return _loseCanvasInstance;
    }
}
