using System;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RxceGame
{
    public class RandomAuthoring : MonoBehaviour
    {
        public int GetSeed() => DateTime.Now.Millisecond;
    }

    public class RandomBaker : Baker<RandomAuthoring>
    {
        public override void Bake(RandomAuthoring authoring)
        {
            AddComponent(new RandomComponent
            {
                random = new Random((uint)authoring.GetSeed())
            });
        }
    }
}