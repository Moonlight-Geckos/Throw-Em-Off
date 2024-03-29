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

    private bool _right;
    private bool _stopped;
    private float _leftChance;
    private Timer _spawnTimer;
    private Observer _observer;
    private void Awake()
    {
        _leftChance = 50f; 
        _stopped = false;
        _spawnTimer = TimersPool.Pool.Get();
        _spawnTimer.Duration =
            UnityEngine.Random.Range(spawnCooldown.minCooldown, spawnCooldown.maxCooldown);
        _spawnTimer.AddTimerFinishedEventListener(Spawn);


        EventsPool.GameStartedEvent.AddListener(() => {
            _spawnTimer.Run();
            enemiesPool.ClearPool();
        });
        EventsPool.GameFinishedEvent.AddListener(Finish);
    }
    private void Finish(bool t)
    {
        _spawnTimer?.Stop();
        _stopped = true;
    }
    private void Spawn()
    {
        if(_stopped) return;
        if(_observer == null)
        {
            _observer = Observer.Instance;
        }
        AssignRandomPosition();

        var t = enemiesPool.Pool.Get();
        t.transform.position =  transform.position + (Vector3.right * (_right ? 1 : -1) * spawnPositionOffset * UnityEngine.Random.Range(0.90f,1.07f));
        t.Initialize(new Vector3(0, -5, 0));

        _spawnTimer.Duration =
            UnityEngine.Random.Range(spawnCooldown.minCooldown, spawnCooldown.maxCooldown);
        _spawnTimer.Run();
    }
    private void AssignRandomPosition()
    {
        _right = UnityEngine.Random.Range(0f, 100f) > _leftChance;
        _leftChance += 10 * (_right ? 1 : -1);
    }
    private void OnDestroy()
    {
        TimersPool.Pool.Release(_spawnTimer);
    }
}
