using UnityEngine;

namespace RxceGame
{
    public class LoaderCallback : MonoBehaviour
    {
        private bool isFirstUpdate = true;

        void Update()
        {
            if (isFirstUpdate)
            {
                isFirstUpdate = false;
                SceneLoader.Instance.LoaderCallback();
            }
        }
    }
}
