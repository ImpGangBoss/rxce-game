using Unity.Entities;
using UnityEngine;

namespace RxceGame
{
    public partial class InitCarParamsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var car in SystemAPI.Query<CarAspect>())
            {
                car.SetCarParamsOnStart();
            }
        }
    }
}