using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

namespace RxceGame
{
    public partial struct InputControllerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {

        }

        public void OnDestroy(ref SystemState state)
        {

        }

        public void OnUpdate(ref SystemState state)
        {
            //float deltaTime = Time.deltaTime;

            foreach (var (playerCar, carParams) in
            SystemAPI.Query<PlayerCar, RefRW<CarMoveParams>>())
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    carParams.ValueRW.JumpTrigger = true;
                }
            }
        }

    }
}
