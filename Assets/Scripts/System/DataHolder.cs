using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    [SerializeField]
    private List<SkinItem> allSkins;

    private static DataHolder _instance;
    public static DataHolder Instance
    {
        get { return _instance; }
    }
    public List<SkinItem> AllSkins
    {
        get { return allSkins; }
    }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            allSkins.Sort((x, y) => x.skinNumber.CompareTo(y.skinNumber));
        }
    }

    #region Methods

    #endregion
}
