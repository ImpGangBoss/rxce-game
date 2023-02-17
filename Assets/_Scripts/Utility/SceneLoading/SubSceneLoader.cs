using System.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Scenes;

namespace RxceGame
{
    public class SubSceneLoader : MonoBehaviour
    {
        [SerializeField] private SubScene subSceneToLoad;
        private WorldUnmanaged _world;
        private Entity _subScene;

        void Awake()
        {
            _world = World.DefaultGameObjectInjectionWorld.Unmanaged;
            _subScene = SceneSystem.GetSceneEntity(_world, subSceneToLoad.SceneGUID);

            //UnloadSubScene();
            //LoadSubScene();
        }

        private void LoadSubScene()
        {
            Debug.Log("Start loading SubScene...");

            var loadParams = new SceneSystem.LoadParameters { Flags = SceneLoadFlags.NewInstance };
            _subScene = SceneSystem.LoadSceneAsync(_world, subSceneToLoad.SceneGUID, loadParams);

            StartCoroutine(CheckLoadingScene());
        }

        private void UnloadSubScene()
        {
            Debug.Log("Start unloading SubScene...");

            SceneSystem.UnloadScene(_world, _subScene, SceneSystem.UnloadParameters.DestroySectionProxyEntities);

            StartCoroutine(CheckUnloadingScene());
        }

        IEnumerator CheckLoadingScene()
        {
            while (!SceneSystem.IsSceneLoaded(_world, _subScene))
            {
                Debug.Log("Loading...");
                yield return null;
            }

            Debug.Log("Scene loaded!");
        }

        IEnumerator CheckUnloadingScene()
        {
            while (SceneSystem.IsSceneLoaded(_world, _subScene))
            {
                Debug.Log("Unloading...");
                yield return null;
            }

            Debug.Log("Scene unloaded!");
            LoadSubScene();
        }
    }

}