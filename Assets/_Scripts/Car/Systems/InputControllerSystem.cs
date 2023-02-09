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
            state.RequireForUpdate<PlayerTag>();
        }

        public void OnDestroy(ref SystemState state) { }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var (playerCar, car) in
            SystemAPI.Query<PlayerTag, CarAspect>())
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    car.Jump();
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    car.RotateBody(Time.deltaTime, true);
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    car.RotateBody(Time.deltaTime, false);
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    car.Brake(Time.deltaTime);
                }
            }
        }

    }
}
