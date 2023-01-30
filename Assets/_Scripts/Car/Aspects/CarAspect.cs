using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;
using Unity.Physics.GraphicsIntegration;
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
        readonly RefRO<PhysicsMass> mass;

        public void SetCarParamsOnStart()
        {
            Debug.Log("here");
            rigidBodyAspect.Mass = moveParams.ValueRW.mass;
        }

        public void AddAcceleration(float deltaTime, float timeFromStart)
        {
            if (velocity.ValueRO.Linear.x < moveParams.ValueRO.maxSpeed)
            {
                //velocity.ValueRW.ApplyLinearImpulse(mass.ValueRO, new float3(moveParams.ValueRO.acceleration * deltaTime, 0f, 0f));
                rigidBodyAspect.ApplyLinearImpulseWorldSpace(new float3(moveParams.ValueRO.acceleration * deltaTime, 0f, 0f));
            }

            // var timeAhead = timeFromStart - moveParams.ValueRO.lastPhysicsUpdateTime;
            // var graphicalInterpolationBuffer = EntityManager.GetComponentData<PhysicsGraphicalInterpolationBuffer>(entity);
            // var smoothedTransform = GraphicalSmoothingUtility.InterpolateUsingVelocity(graphicalInterpolationBuffer.PreviousTransform,
            //                     graphicalInterpolationBuffer.PreviousVelocity, velocity.ValueRO, mass.ValueRO, timeAhead, timeAhead / 0.02f);
            // transformAspect.WorldPosition = smoothedTransform.pos;

            // moveParams.ValueRW.lastPhysicsUpdateTime = timeFromStart;
        }
    }
}
