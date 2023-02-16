using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Scenes;

namespace RxceGame
{
    public class SubSceneLoader : MonoBehaviour
    {
        [SerializeField] private SubScene subSceneToLoad;
        private SystemHandle _sceneSystem;
        private Entity _subScene;

        void Start()
        {
            _sceneSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SceneSystem>();
        }

        private void LoadSubScene()
        {
            Debug.Log("Start loading SubScene...");

            var loadParams = new SceneSystem.LoadParameters { Flags = SceneLoadFlags.NewInstance };

        }
    }

}