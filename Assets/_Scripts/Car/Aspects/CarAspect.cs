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
        readonly Entity entity;
        readonly TransformAspect transformAspect;
        readonly RigidBodyAspect rigidBodyAspect;
        readonly RefRW<CarMoveParams> moveParams;
        readonly RefRW<PhysicsVelocity> velocity;
        readonly RefRW<PhysicsMass> mass;

        public void SetCarParamsOnStart(float3 spawnPos)
        {
            if (!moveParams.ValueRO.initialized)
            {
                rigidBodyAspect.Mass = moveParams.ValueRW.mass;
                moveParams.ValueRW.JumpTrigger = false;
                rigidBodyAspect.Position = spawnPos;
                moveParams.ValueRW.initialized = true;
            }
        }

        public void AddAcceleration(float deltaTime)
        {
            if (velocity.ValueRO.Linear.x < moveParams.ValueRO.maxSpeed)
            {
                rigidBodyAspect.ApplyLinearImpulseWorldSpace(new float3(moveParams.ValueRO.acceleration * deltaTime, 0f, 0f));
            }
        }

        public void Jump()
        {
            if (moveParams.ValueRO.JumpTrigger)
            {
                velocity.ValueRW.ApplyLinearImpulse(mass.ValueRO, new float3(0, moveParams.ValueRO.jumpImpulse, 0));
                moveParams.ValueRW.JumpTrigger = false;
            }
        }

        public void RotateBody(float deltaTime, bool forward)
        {
            var prevRot = rigidBodyAspect.Rotation.value;
            float sign = forward ? -1f : 1f;
            rigidBodyAspect.ApplyImpulseAtPointLocalSpace(new float3(0f, deltaTime * moveParams.ValueRO.rotationSpeed, 0f), new float3(sign, 0f, 0f));
        }

        public void Brake(float deltaTime)
        {
            velocity.ValueRW.Linear *= (1f - deltaTime * moveParams.ValueRO.brakeSpeed);
        }

        public void SetJumpTrigger(bool v) => moveParams.ValueRW.JumpTrigger = v;
        public Entity Entity() => entity;

        public float3 Position() => rigidBodyAspect.Position;
    }
}
