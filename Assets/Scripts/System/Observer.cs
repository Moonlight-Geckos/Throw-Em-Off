using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{

    private static Observer _instance;
    private static Transform _playerTransform;
    private static bool _playerIsHitting;
    private static int _comboCount;
    public static Observer Instance
    {
        get { return _instance; } 
    }
    public Transform PlayerTransform
    {
        get { return _playerTransform; }
    }
    public static bool PlayerIsHitting
    {
        get { return _playerIsHitting; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this);
        else
        {
            _instance = this;
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _playerIsHitting = false;
            _comboCount = 0;
            void hitting()
            {
                _playerIsHitting = true;
            }
            void nothitting(bool enemydied)
            {
                _playerIsHitting = false;
                if (enemydied)
                    _comboCount++;
                if(_comboCount % 10 == 0)
                {
                    EventsPool.ComboLevelEvent.Invoke(_comboCount / 10);
                }
            }
            void playerhit(int y)
            {
                _comboCount = 0;
                EventsPool.ComboLevelEvent.Invoke(0);
            }
            EventsPool.CharacterAttackedEvent.AddListener(hitting);
            EventsPool.FinishedHitEvent.AddListener(nothitting);
            EventsPool.CharacterDamagedEvent.AddListener(playerhit);
        }
    }
}
