using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace RxceGame
{
    public readonly partial struct PlayerSpawnerAspect : IAspect
    {
        public readonly Entity Entity;
        private readonly TransformAspect _transformAspect;
        private readonly RefRO<PlayerSpawnData> _playerSpawnData;


        public float4x4 GetSpawnPos() => new float4x4(quaternion.identity, _playerSpawnData.ValueRO.spawnPos);
        public Entity PlayerCarPrefab() => _playerSpawnData.ValueRO.playerCar;
    }


}
