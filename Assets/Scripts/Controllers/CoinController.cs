using System.Collections;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private IDisposable _disposable;

    public void Initialize(Vector3 destination)
    {
        IEnumerator animate()
        {
            yield return null;
            float duration = 2f;
            yield return new WaitForSeconds(duration/4f);
            while(duration > 0)
            {
                transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime / duration);
                duration -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            EventsPool.CoinCollectedEvent.Invoke();
            if(_disposable == null)
                _disposable = GetComponent<IDisposable>();
            _disposable.Dispose();
        }
        StartCoroutine(animate());
    }
}