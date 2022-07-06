using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Serialized

    [SerializeField]
    private int enemiesToKill = 20;

    #endregion

    #region Private

    private static GameManager _instance;
    private static int _collectedGems;
    private static bool _started;
    private static float _timeSinceStarted = 0;

    #endregion
    static public GameManager Instance
    {
        get { return _instance; }
    }
    public float TimeSinceStarted
    {
        get { return _timeSinceStarted; }
    }
    public int EnemiesToKill
    {
        get { return enemiesToKill; }
    }
    public bool GameStarted
    {
        get { return _started; }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            _collectedGems = PlayerStorage.CoinsCollected;
            EventsPool.ClearPoolsEvent.Invoke();
            EventsPool.GameFinishedEvent.AddListener(FinishGame);
            EventsPool.GameStartedEvent.AddListener(StartGame);
        }
    }
    private void Start()
    {
        EventsPool.UpdateSkinEvent.Invoke(DataHolder.Instance.AllSkins[PlayerStorage.SkinSelected]);
    }
    void Update()
    {
        TimersPool.UpdateTimers(Time.deltaTime);
        _timeSinceStarted += Time.deltaTime;
    }
    private void StartGame()
    {

    }
    private void FinishGame(bool d)
    {
        EventsPool.ClearPoolsEvent.Invoke();
        EventsPool.ClearAllEvents();
    }
}
