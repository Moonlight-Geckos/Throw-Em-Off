using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Serializable]
    class Cooldown
    {
        [SerializeField]
        public float minCooldown;
        [SerializeField]
        public float maxCooldown;
    }
    [SerializeField]
    private float spawnPositionOffset;

    [SerializeField]
    private Cooldown spawnCooldown;

    [SerializeField]
    private EnemyPool enemiesPool;

    private Timer _spawnTimer;
    private bool _right;
    private void Awake()
    {
        _spawnTimer = TimersPool.Pool.Get();
        _spawnTimer.Duration =
            UnityEngine.Random.Range(spawnCooldown.minCooldown, spawnCooldown.maxCooldown);
        _spawnTimer.AddTimerFinishedEventListener(Spawn);
        _spawnTimer.Run();
    }
    private void Spawn()
    {
        var t = enemiesPool.Pool.Get();
        _right = (int)Time.timeSinceLevelLoad*11 % 2 == 0;
        t.transform.position = transform.position + (Vector3.right * (_right ? 1 : -1) * spawnPositionOffset);
        t.Initialize(new Vector3(0, -5, 0));
        _spawnTimer.Duration =
            UnityEngine.Random.Range(spawnCooldown.minCooldown, spawnCooldown.maxCooldown);
        _spawnTimer.Run();
    }
}
