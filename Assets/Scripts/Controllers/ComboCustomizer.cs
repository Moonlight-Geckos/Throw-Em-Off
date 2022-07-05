using System;
using System.Collections.Generic;
using UnityEngine;

public class ComboCustomizer : MonoBehaviour
{
    [Serializable]
    class ComboStyle
    {
        public int level;
        public Gradient gradient;
        [Range(0.1f, 0.5f)]
        public float trailTime;
        [Range(0,1f)]
        public float trailWidth;
        public GameObject[] particleSystems;

    }
    [SerializeField]
    private List<ComboStyle> comboStyles;

    private TrailRenderer[] _trails;
    private ComboStyle _lastComboStyle;
    private Dictionary<int, ComboStyle> _combosDictionary;
    private void Awake()
    {
        _trails = GetComponentsInChildren<TrailRenderer>();
        foreach(TrailRenderer trail in _trails)
        {
            trail.enabled = false;
        }
        _combosDictionary = new Dictionary<int, ComboStyle>();
        foreach (var cs in comboStyles) _combosDictionary.Add(cs.level, cs);

        EventsPool.ComboLevelEvent.AddListener(ComboLevel);
    }
    private void ComboLevel(int level)
    {
        ComboStyle cs;
        _combosDictionary.TryGetValue(level, out cs);

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
