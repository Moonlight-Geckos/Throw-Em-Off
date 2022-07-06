using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField]
    private CoinsPool coinsPool;

    [SerializeField]
    private Vector3 coinsDestination;

    private void Awake()
    {
        EventsPool.FinishedHitEvent.AddListener(SpawnCoin);
        coinsPool.ClearPool();
    }

    private void SpawnCoin(Enemy enemy)
    {
        if (enemy == null)
            return;
        var coin = coinsPool.Pool.Get();
        coin.transform.position = enemy.transform.position + Vector3.up * 4f;
        coin.Initialize(coinsDestination);

    }

}
