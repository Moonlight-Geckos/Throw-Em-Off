using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{

    private static Observer _instance;

    private static Transform _playerTransform;
    private static bool _playerIsHitting;
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
        set { _playerIsHitting = value; }
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
        }
    }
}
