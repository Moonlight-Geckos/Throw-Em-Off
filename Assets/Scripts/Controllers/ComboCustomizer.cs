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
        public GameObject particleSystem;

    }
    [SerializeField]
    private List<ComboStyle> comboStyles;

    private TrailRenderer[] _trails;
    private GameObject _lastParticleSystem;
    private void Awake()
    {
        _trails = GetComponentsInChildren<TrailRenderer>();
        foreach(TrailRenderer trail in _trails)
        {
            trail.enabled = false;
        }
        comboStyles?.Sort((x, y) => x.level.CompareTo(y.level));

        EventsPool.ComboLevelEvent.AddListener(ComboLevel);
    }
    private void ComboLevel(int level)
    {
        if (level > comboStyles.Count)
            level = comboStyles.Count;
        foreach(var tr in _trails)
        {
            if (level == 0)
            {
                tr.enabled = false;
            }
            else
            {
                tr.enabled = true;
                tr.colorGradient = comboStyles[level - 1].gradient;
                tr.time = comboStyles[level - 1].trailTime;
                tr.widthCurve.keys[0].value = comboStyles[level - 1].trailWidth;
            }
        }
        _lastParticleSystem?.SetActive(false);
        if (level != 0)
        {
            comboStyles[level - 1].particleSystem.SetActive(true);
            _lastParticleSystem = comboStyles[level - 1].particleSystem;
        }
    }
}
