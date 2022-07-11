using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private ParticlesPool explosionParticlesPool;

    [SerializeField]
    private Rigidbody bodyRigidbody;

    [SerializeField]
    private BoxCollider outisdeBoxCollider;

    [SerializeField]
    private float _pushBackVelocity = 85f;

    private bool _landed;
    private bool _killed;
    private IDisposable _disposable;
    private Animator _animator;
    private Collider[] _allBodyColliders;
    private Rigidbody[] _allBodyRigidbodies;
    private CharacterJoint[] _allJoints;
    private Rigidbody _movementRigidbody;
    private Timer _stopPhysicsTimer;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movementRigidbody = GetComponent<Rigidbody>();
        _allBodyColliders = GetComponentsInChildren<Collider>();
        _allBodyRigidbodies = GetComponentsInChildren<Rigidbody>();
        _allJoints = GetComponentsInChildren<CharacterJoint>();

        _stopPhysicsTimer = TimersPool.Pool.Get();
        _stopPhysicsTimer.Duration = 8f;
        _stopPhysicsTimer.AddTimerFinishedEventListener(Dispose);

        EventsPool.GameFinishedEvent.AddListener((bool t) => {
            if (!_killed)
                Dispose();
        });
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_killed || _landed)
            return;
        Land();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_killed)
            return;
        if (other.gameObject.layer == StaticValues.DestinationLayer) Explode();
        else if (Observer.PlayerIsHitting && other.gameObject.layer == StaticValues.CharacterLayer) Kill();
    }
    private void Land()
    {
        _landed = true;
        IEnumerator land()
        {
            yield return null;
            _animator.SetTrigger("Landed");
            float duration = _animator.GetCurrentAnimatorClipInfo(0).Length / 2f;
            yield return new WaitForSeconds(duration);
            transform.LookAt(new Vector3(0, transform.position.y, 0));
            _movementRigidbody.velocity = transform.forward * Random.Range(22f, 25f);
        }
        if(gameObject.activeSelf)
            StartCoroutine(land());
    }
    private void Explode()
    {
        EventsPool.CharacterDamagedEvent.Invoke(1);
        explosionParticlesPool.Pool.Get().transform.position = transform.position;
        Dispose();
    }
    private void Dispose()
    {
        if (!gameObject.activeSelf)
            return;
        if (_disposable == null)
            _disposable = GetComponent<IDisposable>();
        _disposable.Dispose();
    }
    public void Kill()
    {
        _killed = true;
        _animator.enabled = false;
        EventsPool.FinishedHitEvent.Invoke(this);
        foreach (var collider in _allBodyColliders)
        {
            collider.enabled = true;
            collider.gameObject.layer = StaticValues.RagdollLayer;
        }
        foreach (var rb in _allBodyRigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        _movementRigidbody.velocity = Vector3.up * (_pushBackVelocity + Observer.Instance.ComboCount*1.7f);
        bodyRigidbody.velocity = (-transform.forward * _pushBackVelocity) + Vector3.up *(_pushBackVelocity + Observer.Instance.ComboCount * 1.3f) / 2f;
        bodyRigidbody.angularVelocity = Vector3.right * Random.Range(-2f, 2f);
        outisdeBoxCollider.enabled = false;
        _stopPhysicsTimer.Run();
    }
    public void Initialize(Vector3 velocity)
    {
        _movementRigidbody.velocity = velocity;
        transform.rotation = Random.rotation;
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

        _landed = false;
        _killed = false;

        foreach (var collider in _allBodyColliders)
        {
            collider.enabled = false;
            collider.gameObject.layer = StaticValues.EnemyLayer;
        }
        foreach (var rb in _allBodyRigidbodies)
        {
            if(rb != _movementRigidbody)
                rb.isKinematic = true;
        }

        outisdeBoxCollider.enabled = true;
        _animator.enabled = true;
    }
}
