using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    [SerializeField]
    private List<SkinItem> allSkins;

    [SerializeField]
    private List<ComboStyle> comboStyles;

    private static DataHolder _instance;
    public static DataHolder Instance
    {
        get { return _instance; }
    }
    public List<SkinItem> AllSkins
    {
        get { return allSkins; }
    }

    private Dictionary<int, ComboStyle> _combosDictionary;

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

            _combosDictionary = new Dictionary<int, ComboStyle>();
            foreach (var cs in comboStyles) _combosDictionary.Add(cs.level, cs);
        }
    }
    public ComboStyle GetComboStyle(int level)
    {
        ComboStyle comboStyle;
        _combosDictionary.TryGetValue(level, out comboStyle);
        return comboStyle;
    }

    #region Methods

    #endregion
}
