using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using Unity.Physics.Aspects;

namespace RxceGame
{
    public readonly partial struct CarAspect : IAspect
    {
        readonly Entity _entity;
        readonly TransformAspect _transformAspect;
        readonly RigidBodyAspect _rigidBodyAspect;
        readonly RefRW<CarMoveParams> _moveParams;
        readonly RefRW<PhysicsVelocity> _velocity;
        readonly RefRW<PhysicsMass> _mass;
        readonly RefRW<PhysicsCollider> _collider;

        public void SetCarParamsOnStart(float3 spawnPos)
        {
            if (!_moveParams.ValueRO.initialized)
            {
                _rigidBodyAspect.Mass = _moveParams.ValueRW.mass;
                _moveParams.ValueRW.JumpTrigger = false;
                _rigidBodyAspect.Position = spawnPos;
                IngameUIManager.Instance.SetStartHP(_moveParams.ValueRO.hp);
                _moveParams.ValueRW.initialized = true;
            }
        }

        public void AddAcceleration(float deltaTime)
        {
            if (!_moveParams.ValueRO.JumpTrigger || _velocity.ValueRO.Linear.x >= _moveParams.ValueRO.maxSpeed)
                return;

            _rigidBodyAspect.ApplyLinearImpulseWorldSpace(new float3(_moveParams.ValueRO.acceleration * deltaTime, 0f, 0f));
        }

        public void Jump()
        {
            if (!_moveParams.ValueRO.JumpTrigger)
                return;

            _velocity.ValueRW.ApplyLinearImpulse(_mass.ValueRO, new float3(0, _moveParams.ValueRO.jumpImpulse, 0));
            _moveParams.ValueRW.JumpTrigger = false;
        }

        public void RotateBody(float deltaTime, bool forward)
        {
            var prevRot = _rigidBodyAspect.Rotation.value;
            float sign = forward ? -1f : 1f;
            _rigidBodyAspect.ApplyImpulseAtPointLocalSpace(new float3(0f, deltaTime * _moveParams.ValueRO.rotationSpeed, 0f), new float3(sign, 0f, 0f));
        }

        public void Brake(float deltaTime)
        {
            if (_moveParams.ValueRO.JumpTrigger)
                return;

            _velocity.ValueRW.Linear *= (1f - deltaTime * _moveParams.ValueRO.brakeSpeed);
        }

        public void TakeDamage(float v)
        {
            _moveParams.ValueRW.hp -= v;
            IngameUIManager.Instance.FillHP(_moveParams.ValueRO.hp);
            _moveParams.ValueRW.DamageTrigger = false;
        }

        public bool IsDead()
        {
            if (_moveParams.ValueRW.hp <= 0)
            {
                IngameUIManager.Instance.ShowResults(0, Position().x);
                return true;
            }

            return false;
        }

        public void ActivateDebuff()
        {
            _moveParams.ValueRW.acceleration *= 0.85f;
            _moveParams.ValueRW.brakeSpeed *= 0.85f;
            _moveParams.ValueRW.jumpImpulse *= 0.85f;
            _moveParams.ValueRW.maxSpeed *= 0.85f;
            _moveParams.ValueRW.rotationSpeed *= 0.85f;

            IngameUIManager.Instance.SetDebuffStatus(true);
        }

        public bool IsDamaged() => IngameUIManager.Instance.IsDamaged();
        public bool IsDebuffed() => IngameUIManager.Instance.IsDebuffed();
        public void SetJumpTrigger(bool v) => _moveParams.ValueRW.JumpTrigger = v;
        public void SetDamageTrigger(bool v) => _moveParams.ValueRW.DamageTrigger = v;
        public bool GetDamageTrigger() => _moveParams.ValueRW.DamageTrigger;
        public Entity Entity() => _entity;
        public float3 Position() => _rigidBodyAspect.Position;
        public quaternion Rotation() => _rigidBodyAspect.Rotation;
        public CarMoveParams GetMoveParams() => _moveParams.ValueRW;
    }
}
