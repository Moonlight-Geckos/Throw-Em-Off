using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Serialized

    [SerializeField]
    private int enemiesToKill = 20;

    #endregion

    #region Private

    private static GameManager _instance;

    private static int _collectedGems;
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
        }
    }
    private void Start()
    {
        Debug.Log(DataHolder.Instance.AllSkins);
        EventsPool.UpdateSkinEvent.Invoke(DataHolder.Instance.AllSkins[PlayerStorage.SkinSelected]);
    }
    void Update()
    {
        TimersPool.UpdateTimers(Time.deltaTime);
        _timeSinceStarted += Time.deltaTime;
    }
}
