using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ComboStyle
{
    public int level;
    public Gradient gradient;
    public Color textColor;
    [Range(0.1f, 0.5f)]
    public float trailTime;
    [Range(0, 1f)]
    public float trailWidth;
    public GameObject[] particleSystems;

}
public class ComboCustomizer : MonoBehaviour
{
    private TrailRenderer[] _trails;
    private ComboStyle _lastComboStyle;
    private DataHolder _dataHolder;

    private void Awake()
    {
        _trails = GetComponentsInChildren<TrailRenderer>();
        foreach(TrailRenderer trail in _trails)
        {
            trail.enabled = false;
        }
        EventsPool.ComboLevelEvent.AddListener(ComboLevel);
    }
    private void ComboLevel(int level)
    {
        if(_dataHolder == null)
            _dataHolder = DataHolder.Instance;

        ComboStyle cs = _dataHolder.GetComboStyle(level);

        if(_lastComboStyle != null && (cs != null || level == 0))
            foreach(var t in _lastComboStyle.particleSystems)
                t.SetActive(false);

        foreach (var tr in _trails)
        {
            if (level == 0)
            {
                tr.enabled = false;
            }
            else if(cs != null)
            {
                tr.enabled = true;
                tr.colorGradient = cs.gradient;
                tr.time = cs.trailTime;
                tr.widthCurve.keys[0].value = cs.trailWidth;
            }
        }
        if (cs != null)
        {
            foreach (var t in cs.particleSystems)
                t.SetActive(true);
            _lastComboStyle = cs;
        }
    }
}
