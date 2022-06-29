using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int numOfAnimations = 4;

    private Animator _animator;
    private int _animationIndex = 0;
    private List<int> _randomAnimations;
    private bool _animating;
    private bool _idle;


    private Queue<AttackDirection> _nextAttacksQueue;
    private AttackDirection _atkDir;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _atkDir = AttackDirection.None;
        _randomAnimations = new List<int>();
        _nextAttacksQueue = new Queue<AttackDirection>();
        _animating = false;
        _idle = false;

        for (int i = 1; i <= numOfAnimations; i++)
        {
            _randomAnimations.Add(i);
        }
        _randomAnimations.Sort((a, b) => 1 - 2 * Random.Range(0, _randomAnimations.Count));

        EventsPool.UserSwipedEvent.AddListener(RegisterAttack);
    }
    private void Update()
    {
        if(_nextAttacksQueue.Count > 0)
        {
            if (_animating)
                return;
            _idle = false;
            AttackWithAnimation(_nextAttacksQueue.Dequeue());
        }
        else if(!_animating && !_idle)
        {
            _idle = true;
            _animator.CrossFade("Idle", 0.2f);
        }
    }
    private void AttackWithAnimation(AttackDirection dir)
    {
        _animationIndex++;
        _atkDir = dir;
        _animating = true;
        if (_animationIndex >= _randomAnimations.Count)
        {
            _randomAnimations.Sort((a, b) => 1 - 2 * Random.Range(0, _randomAnimations.Count));
            _animationIndex = 0;
        }
        _animator.CrossFade(_randomAnimations[_animationIndex].ToString(), 0.2f);
        IEnumerator s()
        {
            yield return null;
            float duration = 0.1f;
            bool right = dir == AttackDirection.Right;
            if ((dir == AttackDirection.Right && transform.localEulerAngles.y > 100)
                || (dir == AttackDirection.Left && transform.localEulerAngles.y > 0))
            {
                while (duration > 0)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90 * (right ? 1 : -1), 0), Time.deltaTime / duration);
                    duration -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            duration = 0.1f;
            yield return new WaitForSeconds(duration);
            _animating = false;
        }
        StartCoroutine(s());
    }
    private void RegisterAttack(AttackDirection dir)
    {
        if(_idle || _animating)
            _nextAttacksQueue.Enqueue(dir);
    }
}
