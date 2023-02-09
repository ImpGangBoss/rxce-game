using Unity.Entities;
using UnityEngine;
using Unity.Burst;

namespace RxceGame
{
    [BurstCompile]
    public partial struct InputControllerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (playerCar, car) in
            SystemAPI.Query<PlayerTag, CarAspect>())
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    Debug.Log("Jump");
                    car.Jump();
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    Debug.Log("Right");
                    car.RotateBody(Time.deltaTime, true);
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    Debug.Log("Left");
                    car.RotateBody(Time.deltaTime, false);
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    Debug.Log("Break");
                    car.Brake(Time.deltaTime);
                }
            }
        }

    }
}
