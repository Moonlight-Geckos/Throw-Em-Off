using UnityEngine;

public class CoinsAnimator : MonoBehaviour
{
    [SerializeField]
    private Mesh coinMesh;
    [SerializeField]
    private Material coinMaterial;

    private Matrix4x4[] _matrices;
    private Vector3[] _positions;
    private Vector3 _coinsDestination = new Vector3(-22, 88, 0);
    private Observer _observer;
    private GameManager _gameManager;
    private float time;

    void Awake()
    {
        EventsPool.GameFinishedEvent.AddListener((bool d) => enabled = true);
        enabled = false;
        time = 0;
    }

    void Update()
    {
        if (_observer == null) 
        {
            _observer = Observer.Instance;
            _gameManager = GameManager.Instance;
        }
        if (_observer.EnemiesLeft == _gameManager.EnemiesToKill)
            return;

        if (_matrices == null)
        {
            int coins = GameManager.Instance.EnemiesToKill - Observer.Instance.EnemiesLeft;
            if (coins > 0)
                InitializeCoins(coins);
        }
        else
            AnimateCoins();
    }

    private void InitializeCoins(int coins)
    {
        _matrices = new Matrix4x4[coins];
        _positions = new Vector3[coins];
        int[] sign = { 1, -1 };
        for (int i = 0; i < coins; i++)
        {
            _positions[i] = new Vector3(Random.Range(20, 25) * sign[Random.Range(0, 2)], Random.Range(8, 15), 0);
            _matrices[i] = Matrix4x4.TRS(_positions[i], Quaternion.identity, Vector3.one);
        }
    }
    private void AnimateCoins()
    {
        time += Time.deltaTime;
        for (int i = 0; i < _positions.Length; i++)
        {
            if (time < 1 && i % 2 == 0)
                return;
            _positions[i] += (_coinsDestination - _positions[i]).normalized * Time.deltaTime * (60 - i * 1.4f);
            _matrices[i] = Matrix4x4.TRS(_positions[i], Quaternion.identity, Vector3.one);
        }
        Graphics.DrawMeshInstanced(coinMesh, 0, coinMaterial, _matrices);
    }
}
