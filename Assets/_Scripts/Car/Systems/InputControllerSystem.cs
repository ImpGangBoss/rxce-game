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

        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = Time.deltaTime;

            foreach (var (playerCar, car) in
            SystemAPI.Query<PlayerTag, CarAspect>())
            {
                if (car.IsDead())
                    return;

                if (car.IsDamaged() && !car.IsDebuffed())
                {
                    car.ActivateDebuff();
                    var sprite = state.EntityManager.GetComponentObject<SpriteRenderer>(car.Entity());
                    sprite.color = sprite.color * 0.5f + Color.black;
                }

                if (Input.GetKey(KeyCode.Space))
                    car.Jump();

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                    car.AddAcceleration(deltaTime);

                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    car.RotateBody(deltaTime, true);

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                    car.RotateBody(deltaTime, false);

                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                    car.Brake(deltaTime);

                if (car.GetDamageTrigger())
                    car.TakeDamage(1f);
            }
        }

    }
}
