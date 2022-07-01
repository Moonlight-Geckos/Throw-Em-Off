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
    private Observer _observer;
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
        if(_observer == null)
        {
            _observer = Observer.Instance;
        }
        var t = enemiesPool.Pool.Get();
        _right = (int)Time.timeSinceLevelLoad*11 % 2 == 0;
        t.transform.position =  transform.position + (Vector3.right * (_right ? 1 : -1) * spawnPositionOffset * UnityEngine.Random.Range(0.87f,1.15f));
        t.Initialize(new Vector3(0, -5, 0));
        _spawnTimer.Duration =
            UnityEngine.Random.Range(spawnCooldown.minCooldown, spawnCooldown.maxCooldown);
        _spawnTimer.Run();
    }
}
