using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveParams", menuName = "ScriptableObjects/CarMoveParams", order = 1)]
[System.Serializable]
public class MoveParamsData : ScriptableObject
{
    [SerializeField] float mass;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpImpulse;
    [SerializeField] float rotationSpeed;
    [SerializeField] float brakeSpeed;

    public float Mass { get => mass; }
    public float Acceleration { get => acceleration; }
    public float MaxSpeed { get => maxSpeed; }
    public float JumpImpulse { get => jumpImpulse; }
    public float RotationSpeed { get => rotationSpeed; }
    public float BrakeSpeed { get => brakeSpeed; }
}
