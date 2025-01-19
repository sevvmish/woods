using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCstats : MonoBehaviour
{
    [SerializeField] private float maxHP = 20;
    public float MaxHP => maxHP;

    private float currentHP;
    public float CurrentHP => currentHP;

    [SerializeField] private float maxSpeed = 5;
    public float MaxSpeed => maxSpeed;

    [SerializeField] private float walkSpeed = 1;
    public float WalkSpeed => walkSpeed;

    [SerializeField] private float agroRadius = 2;
    public float AgroRadius => agroRadius;

    [SerializeField] private float hitRadius = 3;
    public float HitRadius => hitRadius;

    [SerializeField] private float attackSpeed = 3;
    public float AttackSpeed => attackSpeed;

    [SerializeField] private float bodyHitDistance = 1;
    public float BodyHitDistance => bodyHitDistance;

    [SerializeField] private float idleWalkingRadius = 6;
    public float IdleWalkingRadius => idleWalkingRadius;


    private void Awake()
    {
        Restart();        
    }

    public void Restart()
    {
        currentHP = maxHP;

        if (TryGetComponent(out Interactable i))
        {
            i.SetHP(currentHP);
        }
    }
}
