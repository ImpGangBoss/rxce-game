using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RxceGame
{
    public class SceneLoader : SingletonComponent<SceneLoader>
    {
        [SerializeField] private float delay;
        private Action onLoaderCallback;
        private AsyncOperation loadingAsyncOperation;
        public bool EnablePlayerSpawner { get; set; }

        public enum Scene
        {
            Game,
            Loading,
            Garage
        }

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private IEnumerator LoadSceneAsync(Scene scene)
        {
            yield return new WaitForSeconds(delay);

            loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

            while (!loadingAsyncOperation.isDone)
                yield return new WaitForSeconds(delay);
        }

        public float GetLoadingProgress()
        {
            if (loadingAsyncOperation != null)
                return loadingAsyncOperation.progress;
            return 0f;
        }

        private void Load(Scene scene)
        {
            onLoaderCallback = () =>
            {
                StartCoroutine(LoadSceneAsync(scene));
                EnablePlayerSpawner = scene == Scene.Game;
            };

            SceneManager.LoadScene(Scene.Loading.ToString());
        }

        public void LoaderCallback()
        {
            if (onLoaderCallback != null)
            {
                onLoaderCallback();
                onLoaderCallback = null;
            }
        }

        public void LoadGarage() => Load(Scene.Garage);
        public void LoadGame() => Load(Scene.Game);

        public void Exit() => Application.Quit();
    }
}
