using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private Rigidbody bodyRigidbody;

    [SerializeField]
    private BoxCollider outisdeBoxCollider;



    private bool _landed;
    private bool _killed;
    private Animator _animator;
    private Collider[] _allBodyColliders;
    private Rigidbody[] _allBodyRigidbodies;
    private Rigidbody _movementRigidbody;
    private Timer _stopPhysicsTimer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movementRigidbody = GetComponent<Rigidbody>();
        _allBodyColliders = GetComponentsInChildren<Collider>();
        _allBodyRigidbodies = GetComponentsInChildren<Rigidbody>();
        _stopPhysicsTimer = TimersPool.Pool.Get();
        _stopPhysicsTimer.Duration = 8f;
        _stopPhysicsTimer.AddTimerFinishedEventListener(StopPhysics);
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
        if (other.gameObject.layer == StaticValues.DestinationLayer)    Explode();
        else if (other.gameObject.layer == StaticValues.CharacterLayer)   Kill();
    }
    private void StopPhysics()
    {
        foreach(var rb in _allBodyRigidbodies)
        {
            rb.isKinematic = true;
        }
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
            _movementRigidbody.velocity = transform.forward * Random.Range(20f, 25f);
        }
        StartCoroutine(land());
    }
    private void Explode()
    {
        bodyRigidbody.velocity = Vector3.zero;
    }
    private void Kill()
    {
        _killed = true;
        _animator.enabled = false;
        _stopPhysicsTimer.Run();
        foreach (var collider in _allBodyColliders)
        {
            collider.enabled = true;
            collider.gameObject.layer = StaticValues.RagdollLayer;
        }
        _movementRigidbody.velocity = Vector3.up * 140f;
        bodyRigidbody.velocity = (-transform.forward * 140f) + Vector3.up * 65f;
        bodyRigidbody.angularVelocity = Vector3.right * Random.Range(-2f, 2f);
        outisdeBoxCollider.enabled = false;
    }
    public void Initialize(Vector3 velocity)
    {
        _movementRigidbody.velocity = velocity;

        _landed = false;
        _killed = false;

        foreach (var collider in _allBodyColliders)
        {
            collider.enabled = false;
            collider.gameObject.layer = StaticValues.EnemyLayer;
        }
        foreach (var rb in _allBodyRigidbodies)
        {
            rb.isKinematic = false;
        }

        outisdeBoxCollider.enabled = true;
        _animator.enabled = true;
    }
}
