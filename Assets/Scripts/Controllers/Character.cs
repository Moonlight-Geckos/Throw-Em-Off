using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour, IDamagable
{
    [SerializeField]
    private int maxHealth = 10;

    [SerializeField]
    private int numOfAnimations = 4;

    [SerializeField]
    private float attackDuration = 0.35f;

    private Animator _animator;
    private int _animationIndex = 0;
    private bool _animating;
    private bool _idle;

    private bool _right;
    private bool _canSwipe;
    private float _duration;
    private float _health;

    private Timer _attackTimer;
    private List<string> _randomAnimations;
    private Queue<AttackDirection> _nextAttacksQueue;
    private void Awake()
    {
        _attackTimer = TimersPool.Pool.Get();
        _attackTimer.Duration = attackDuration;
        _attackTimer.AddTimerFinishedEventListener(FinishedAnimating);
        _animator = GetComponentInChildren<Animator>();
        _randomAnimations = new List<string>();
        _nextAttacksQueue = new Queue<AttackDirection>();
        _health = maxHealth;
        _animating = false;
        _canSwipe = true;
        _idle = false;

        for (int i = 1; i <= numOfAnimations; i++)
        {
            _randomAnimations.Add(i.ToString());
        }
        _randomAnimations.Sort((a, b) => 1 - 2 * Random.Range(0, _randomAnimations.Count));

        EventsPool.CharacterDamagedEvent.AddListener(GetDamage);
        EventsPool.UserSwipedEvent.AddListener(RegisterAttack);
    }
    private void Update()
    {
        if (_animating && !_canSwipe && _attackTimer.Running && _attackTimer.Duration - _attackTimer.ElapsedSeconds <= 0.15f)
            _canSwipe = true;
        if(_nextAttacksQueue.Count > 0)
        {
            if (_animating)
                return;
            _idle = false;
            AttackWithAnimation(_nextAttacksQueue.Dequeue());
            _attackTimer.Run();
        }
        else if(!_animating && !_idle)
        {
            _idle = true;
            _animator.CrossFade("Idle", 0.12f);
        }
    }
    private void AttackWithAnimation(AttackDirection dir)
    {
        _animationIndex++;
        _animating = true;
        _canSwipe = false;
        EventsPool.CharacterAttackedEvent.Invoke();
        if (_animationIndex >= _randomAnimations.Count)
        {
            _randomAnimations.Sort((a, b) => 1 - 2 * Random.Range(0, _randomAnimations.Count));
            _animationIndex = 0;
        }
        _animator.CrossFade(_randomAnimations[_animationIndex], 0.12f);
        IEnumerator s()
        {
            yield return null;
            _duration = 0.1f;
            _right = dir == AttackDirection.Right;
            if ((dir == AttackDirection.Left && transform.localEulerAngles.y > 100)
                || (dir == AttackDirection.Right && transform.localEulerAngles.y > 0))
            {
                while (_duration > 0)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90 * (_right ? -1 : 1), 0), Time.deltaTime / _duration);
                    _duration -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        StartCoroutine(s());
    }
    private void RegisterAttack(AttackDirection dir)
    {
        if(_canSwipe && _nextAttacksQueue.Count < 3)
            _nextAttacksQueue.Enqueue(dir);
    }
    private void FinishedAnimating()
    {
        _animating = false;
    }
    private void Die()
    {

    }
    public void GetDamage(int damage)
    {
        if (_health <= 0)
            return;
        _health -= damage;
        if(_health <= 0)
        {
            Die();
        }
    }
}