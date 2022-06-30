using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Serialized


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
    void Update()
    {
        TimersPool.UpdateTimers(Time.deltaTime);
        _timeSinceStarted += Time.deltaTime;
    }
}
