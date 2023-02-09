// using System.Linq;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Physics;
// using Unity.Physics.Systems;
// using Unity.Transforms;
// using UnityEngine;
// using UnityEngine.UIElements;
// using static UnityEngine.GraphicsBuffer;

// namespace RxceGame
// {
//     [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
//     [UpdateAfter(typeof(PhysicsSystemGroup))]
//     [BurstCompile]
//     public partial struct TriggerSystem : ISystem
//     {
//         ComponentLookup<WheelTag> wheelsLookup;
//         ComponentLookup<GroundTag> groundLookup;
//         ComponentLookup<CarMoveParams> carsLookup;

//         [BurstCompile]
//         public void OnCreate(ref SystemState state)
//         {
//             wheelsLookup = SystemAPI.GetComponentLookup<WheelTag>();
//             groundLookup = SystemAPI.GetComponentLookup<GroundTag>();
//             carsLookup = SystemAPI.GetComponentLookup<CarMoveParams>();
//         }

//         [BurstCompile]
//         public void OnDestroy(ref SystemState state) { }

//         [BurstCompile]
//         public void OnUpdate(ref SystemState state)
//         {
//             var ecbBOS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
//             SimulationSingleton simulation = SystemAPI.GetSingleton<SimulationSingleton>();

//             wheelsLookup.Update(ref state);
//             groundLookup.Update(ref state);
//             carsLookup.Update(ref state);

//             state.Dependency = new TriggerJob()
//             {
//                 wheels = wheelsLookup,
//                 ground = groundLookup,
//                 cars = carsLookup,
//                 ecb = ecbBOS

//             }.Schedule(simulation, state.Dependency);
//         }

//     }


//     [BurstCompile]
//     struct TriggerJob : ITriggerEventsJob
//     {
//         public ComponentLookup<WheelTag> wheels;
//         public ComponentLookup<GroundTag> ground;
//         public ComponentLookup<CarMoveParams> cars;
//         public EntityCommandBuffer ecb;

//         public void Execute(TriggerEvent triggerEvent)
//         {

//             Debug.Log("Executed");

//             // bool execute = false;

//             // Entity entityA = triggerEvent.EntityA;
//             // Entity entityB = triggerEvent.EntityB;

//             // if (ground.HasComponent(entityA) && wheels.HasComponent(entityB))
//             // {
//             //     execute = true;
//             // }
//             // else if (ground.HasComponent(entityB) && wheels.HasComponent(entityA))
//             // {
//             //     execute = true;
//             // }
//         }
//     }
// }
