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


    private void Awake()
    {
        currentHP = maxHP;

        if (TryGetComponent(out Interactable i))
        {
            i.SetHP(currentHP);
        }
    }
}
