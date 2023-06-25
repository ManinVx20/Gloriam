using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField]
        private List<UICanvas> _UICanvasList;

        private Dictionary<string, UICanvas> _UICanvasDictionary = new Dictionary<string, UICanvas>();

        private void Awake()
        {
            for (int i = 0; i < _UICanvasList.Count; i++)
            {
                string key = _UICanvasList[i].GetType().Name;

                if (!_UICanvasDictionary.ContainsKey(key))
                {
                    _UICanvasDictionary[key] = _UICanvasList[i];
                }
            }
        }

        public T GetUICanvas<T>() where T : UICanvas
        {
            return _UICanvasDictionary[typeof(T).Name] as T;
        }

        public void OpenUICanvas<T>() where T : UICanvas
        {
            T UICanvas = GetUICanvas<T>();

            UICanvas.Setup();

            UICanvas.Open();
        }

        public void CloseUICanvas<T>() where T : UICanvas
        {
            T UICanvas = GetUICanvas<T>();

            UICanvas.Close();
        }

        public void CloseUICanvasWithDelay<T>(float delayTime) where T : UICanvas
        {
            T UICanvas = GetUICanvas<T>();

            UICanvas.CloseWithDelay(delayTime);
        }

        public void CloseAllUICanvases()
        {
            foreach (UICanvas UICanvas in _UICanvasDictionary.Values)
            {
                if (UICanvas.gameObject.activeInHierarchy)
                {
                    UICanvas.Close();
                }
            }
        }
    }
}
