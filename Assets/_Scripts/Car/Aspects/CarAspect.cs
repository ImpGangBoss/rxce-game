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

        public void SetCarParamsOnStart(float3 spawnPos)
        {
            if (!_moveParams.ValueRO.initialized)
            {
                _rigidBodyAspect.Mass = _moveParams.ValueRW.mass;
                _moveParams.ValueRW.JumpTrigger = false;
                _rigidBodyAspect.Position = spawnPos;
                _moveParams.ValueRW.initialized = true;
            }
        }

        public void AddAcceleration(float deltaTime)
        {
            if (_velocity.ValueRO.Linear.x < _moveParams.ValueRO.maxSpeed)
            {
                _rigidBodyAspect.ApplyLinearImpulseWorldSpace(new float3(_moveParams.ValueRO.acceleration * deltaTime, 0f, 0f));
            }
        }

        public void Jump()
        {
            if (_moveParams.ValueRO.JumpTrigger)
            {
                _velocity.ValueRW.ApplyLinearImpulse(_mass.ValueRO, new float3(0, _moveParams.ValueRO.jumpImpulse, 0));
                _moveParams.ValueRW.JumpTrigger = false;
            }
        }

        public void RotateBody(float deltaTime, bool forward)
        {
            var prevRot = _rigidBodyAspect.Rotation.value;
            float sign = forward ? -1f : 1f;
            _rigidBodyAspect.ApplyImpulseAtPointLocalSpace(new float3(0f, deltaTime * _moveParams.ValueRO.rotationSpeed, 0f), new float3(sign, 0f, 0f));
        }

        public void Brake(float deltaTime)
        {
            _velocity.ValueRW.Linear *= (1f - deltaTime * _moveParams.ValueRO.brakeSpeed);
        }

        public void SetJumpTrigger(bool v) => _moveParams.ValueRW.JumpTrigger = v;
        public Entity Entity() => _entity;

        public float3 Position() => _rigidBodyAspect.Position;
    }
}
