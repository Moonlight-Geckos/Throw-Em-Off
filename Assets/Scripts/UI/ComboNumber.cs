using System.Collections;
using TMPro;
using UnityEngine;

public class ComboNumber : MonoBehaviour
{

    private TextMeshProUGUI _comboNumText;

    private Observer _observer;
    private DataHolder _dataHolder;
    private ComboStyle _comboStyle;

    Vector3 _newScale = Vector3.one * 1.5f;
    Vector3 _originalScale = Vector3.one;

    float _duration;

    private void Awake()
    {
        _comboNumText = GetComponent<TextMeshProUGUI>();
        _originalScale = transform.localScale;
        IEnumerator shake()
        {
            yield return null;
            transform.localScale = _originalScale; 
            _duration = 0.25f;
            while (_duration > 0)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, _newScale, _duration / Time.deltaTime);
                _duration -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            _duration = 0.25f;
            while (_duration > 0)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, _originalScale, _duration / Time.deltaTime);
                _duration -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        void addcombo(Enemy deadenem)
        {
            if (_observer == null) 
            {
                _observer = Observer.Instance;
                _dataHolder = DataHolder.Instance;
            }
            if (deadenem == null)
                return;
            _comboStyle = _dataHolder.GetComboStyle(_observer.ComboCount/10);
            if(_comboStyle != null)
            {
                _comboNumText.color = _comboStyle.textColor;
            }
            else
            {
                _comboNumText.color = Color.white;
            }
            _comboNumText.enabled = true;
            _comboNumText.text = "x" + _observer.ComboCount.ToString();
            StopAllCoroutines();
            StartCoroutine(shake());
        }
        void removecombo(int y)
        {
            if (_observer == null)
            { 
                _observer = Observer.Instance;
                _dataHolder = DataHolder.Instance;
            }
            _comboNumText.enabled = false;
        }
        removecombo(-1);
        EventsPool.FinishedHitEvent.AddListener(addcombo);
        EventsPool.CharacterDamagedEvent.AddListener(removecombo);
    }
}
