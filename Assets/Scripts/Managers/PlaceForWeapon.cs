using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceForWeapon : MonoBehaviour
{
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;

    public Transform RightHand { get => rightHand; }
    public Transform LeftHand { get => leftHand; }
}
