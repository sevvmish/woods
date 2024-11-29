using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public AnimationStates AnimationState { get; private set; }

    private Animator _animator;
    private PlayerControl pc;
    private float speedLimit = 1f;
    private float minSpeed = 0.1f;
    private float _timer;
    private readonly float coolDown = 0.1f;
        

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();        
        pc = transform.parent.GetComponent<PlayerControl>();
        pc.SetAnimatorData(this);
        AnimationState = AnimationStates.None;
        idle();
    }

    public void Run()
    {
        checkMovement();
    }

    public void Idle()
    {
        checkMovement();
    }

    public async UniTask<bool> Hit()
    {
        await hit().AsUniTask();
        return true;
    }

    public async UniTask<bool> Collect(Asset asset)
    {
        await collect(asset).AsUniTask();
        return true;
    }

    public void JumpStart()
    {
        _animator.Play("JumpStart");
        AnimationState = AnimationStates.Fly;
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }

        _timer = coolDown;

        checkMovement();
    }

    private void checkMovement()
    {
        if (pc.IsGrounded && AnimationState != AnimationStates.Hit && AnimationState != AnimationStates.Collect)
        {
            float speed = pc.PlayerVelocity;
                        
            if (speed < speedLimit && speed > minSpeed)
            {
                walk();
            }
            else if (speed > speedLimit)
            {
                run();
            }
            else if (speed <= minSpeed)
            {
                idle();
            }
        }
        else if (!pc.IsGrounded)
        {            
            fly();
        }
        
    }

    private void fly()
    {
        if (AnimationState == AnimationStates.Fly) return;
            AnimationState = AnimationStates.Fly;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpLoop")
                && !_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpStart")) _animator.Play("JumpLoop");
    }

    private void idle()
    {
        if (AnimationState == AnimationStates.Idle) return;
            AnimationState = AnimationStates.Idle;

        if (pc.IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {            
            _animator.Play("Idle");
        }
        else if (!pc.IsGrounded)
        {
            fly();
        }
    }

    private void run()
    {
        if (AnimationState == AnimationStates.Run) return;
            AnimationState = AnimationStates.Run;

        if (pc.IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            _animator.Play("Run");
        }
        else if (!pc.IsGrounded)
        {
            fly();
        }
    }

    private void walk()
    {
        if (AnimationState == AnimationStates.Walk) return;
            AnimationState = AnimationStates.Walk;

        if (pc.IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            _animator.Play("Walk");
        }
        else if (!pc.IsGrounded)
        {
            fly();
        }
    }

    private async UniTask<bool> hit()
    {
        if (AnimationState == AnimationStates.Hit) return false;

        AnimationState = AnimationStates.Hit;

        string anim = "";

        if (1 == 1)
        {
            int rnd = UnityEngine.Random.Range(0, 2);

            switch(rnd)
            {
                case 0:
                    anim = "Unarmed Hit R";
                    
                    break;

                case 1:
                    anim = "Unarmed Hit L";
                    break;
            }
            
        }

        _animator.Play(anim);
        int awaited = 0;
        bool isHitted = false;

        await UniTask.Delay(100);
        pc.Effects.PlayEffectAtLocation(pc.Effects.PunchSwingPool, pc.transform.position + Vector3.up * 1.2f, 0.5f);

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) 
        {
            awaited += 20;
            await UniTask.Delay(20);
            

            if (awaited > 100 && !isHitted)
            {
                isHitted = true;
                pc.Hits.MakeHit(pc.transform, Vector3.up * 1.2f);
            }
        }

        AnimationState = AnimationStates.Idle;
        checkMovement();
        return true;
    }

    private async UniTask<bool> collect(Asset asset)
    {
        if (AnimationState == AnimationStates.Collect) return false;

        AnimationState = AnimationStates.Collect;

        string anim = "Loot fast";
                   

        switch(Asset.GetBodyLevelByAsset(asset.AssetType))
        {
            default:
                anim = "Loot fast";
                break;

            case BodyLevel.Low:
                anim = "Loot fast";
                break;

            case BodyLevel.Medium:
                anim = "Loot fast";
                break;

            case BodyLevel.High:
                anim = "Loot fast";
                break;
        }

        _animator.Play(anim);
        int awaited = 0;
        bool isHitted = false;

        await UniTask.Delay(100);
        
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            awaited += 20;
            await UniTask.Delay(20);


            if (awaited > 100 && !isHitted)
            {
                isHitted = true;
                if (asset.TryGetComponent(out Interactable i))
                {
                    i.GetHit(1000);
                }
            }
        }

        AnimationState = AnimationStates.Idle;
        checkMovement();
        return true;
    }


}
