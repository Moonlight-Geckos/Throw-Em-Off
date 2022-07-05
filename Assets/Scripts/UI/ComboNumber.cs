using System.Collections;
using TMPro;
using UnityEngine;

public class ComboNumber : MonoBehaviour
{
    private TextMeshProUGUI _comboNumText;
    private Observer _observer;

    Vector3 _newScale = Vector3.one * 1.25f;
    Vector3 _originalScale = Vector3.one * 1.25f;

    private void Awake()
    {
        _comboNumText = GetComponent<TextMeshProUGUI>();
        _originalScale = transform.localScale;
        IEnumerator shake()
        {
            yield return null;
            float duration = 0.1f;
            transform.localScale = _originalScale;
            while (duration > 0)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, _newScale, duration / Time.deltaTime);
                duration -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            duration = 0.1f;
            while (duration > 0)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, _originalScale, duration / Time.deltaTime);
                duration -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        void addcombo(bool deadenem)
        {
            if (!deadenem)
                return;
            if(_observer == null) _observer = Observer.Instance;
            _comboNumText.enabled = true;
            _comboNumText.text = "x" + _observer.ComboCount.ToString();
            StopAllCoroutines();
            StartCoroutine(shake());
        }
        void removecombo(int y)
        {
            if (_observer == null) _observer = Observer.Instance;
            _comboNumText.enabled = false;
        }
        removecombo(-1);
        EventsPool.FinishedHitEvent.AddListener(addcombo);
        EventsPool.CharacterDamagedEvent.AddListener(removecombo);
    }
}
