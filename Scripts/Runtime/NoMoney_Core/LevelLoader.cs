using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoMoney_Core
{
    /// <summary>
    /// Singleton object to handle level loading.
    /// </summary>
    public class LevelLoader : Singleton<LevelLoader>
    {
        // Percent loaded.
        public float progress { get; private set; }

        private List<AsyncOperation> loadOperations = new List<AsyncOperation>();

        #region Loading
        /// <summary>
        /// For loading individual scenes and leves.
        /// </summary>
        /// <param name="level">The Level to load.</param>
        public void LoadLevel(SceneLevel level)
        {          
            StartCoroutine(LoadScene(LoadOperation(level), level));
        }

        /// <summary>
        /// For initializing a scene with its sub-scenes.
        /// </summary>
        /// <param name="levels">The levels to load.</param>
        public void LoadLevel(List<SceneLevel> levels)
        {
            long minLoadTime = 0;
            foreach(SceneLevel level in levels)
            {
                // Take the largest min load time
                if(level.minLoadTime > minLoadTime)
                {
                    minLoadTime = level.minLoadTime;
                }

                AsyncOperation operation = LoadOperation(level);
                //operation.completed += _ => loadOperations.Remove(operation);
                loadOperations.Add(operation);
            }
            StartCoroutine(LoadScene(minLoadTime));
        }

        /// <summary>
        /// Create a load operation.
        /// </summary>
        /// <param name="level">The level to create the operation on.</param>
        /// <returns></returns>
        private AsyncOperation LoadOperation(SceneLevel level)
        {
            LoadSceneMode mode = level.isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            AsyncOperation operation = level.name == null ?
                SceneManager.LoadSceneAsync(level.index, mode) :
                SceneManager.LoadSceneAsync(level.name, mode);
            operation.allowSceneActivation = false;
            return operation;
        }
        #endregion

        #region Unloading
        /// <summary>
        /// Unload the scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="options"></param>
        public void UnLoadScene(Scene scene, UnloadSceneOptions options = UnloadSceneOptions.None)
        {
            SceneManager.UnloadSceneAsync(scene, options);
        }

        /// <summary>
        /// Unload the scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="options"></param>
        public void UnloadScene(int index, UnloadSceneOptions options = UnloadSceneOptions.None)
        {
            SceneManager.UnloadSceneAsync(index, options);
        }

        /// <summary>
        /// Unload the scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="options"></param>
        public void UnloadScene(string name, UnloadSceneOptions options = UnloadSceneOptions.None)
        {
            SceneManager.UnloadSceneAsync(name, options);
        }
        #endregion

        #region Enumerators
        private IEnumerator LoadScene(AsyncOperation operation, SceneLevel level)
        {
            long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            while (!operation.isDone)
            {
                progress = Mathf.Clamp01(operation.progress / .9f);
                long delta = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - startTime;
                operation.allowSceneActivation = delta > level.minLoadTime;
                yield return null;
            }
        }

        private IEnumerator LoadScene(long minLoadTime)
        {
            progress = 0;
            long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            while (progress < 1)
            {
                foreach (AsyncOperation operation in loadOperations)
                {
                    progress += Mathf.Clamp01(operation.progress / .9f);
                    long delta = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - startTime;
                    operation.allowSceneActivation = delta > minLoadTime;
                    yield return null;
                }
            }
            loadOperations.Clear();
        }
        #endregion

        /// <summary>
        /// Quits the game.
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// Structure needed to load levels
    /// </summary>
    public struct SceneLevel
    {
        public long minLoadTime { get; private set; }
        public int index { get; private set; }
        public string name { get; private set; }
        public bool isAdditive { get; private set; }

        #region Index
        public SceneLevel(int index)
        {
            this.index = index;
            minLoadTime = 0;
            name = null;
            isAdditive = false;
        }

        public SceneLevel(int index, bool isAdditive)
        {
            this.index = index;
            this.isAdditive = isAdditive;
            name = null;
            minLoadTime = 0;
        }

        public SceneLevel(int index, long minLoadTime)
        {
            this.index = index;
            this.minLoadTime = minLoadTime;
            name = null;
            isAdditive = false;
        }
        #endregion

        #region Level Name
        public SceneLevel(string name)
        {
            this.name = name;
            minLoadTime = 0;
            index = -1;
            isAdditive = false;
        }

        public SceneLevel(string name, bool isAdditive)
        {
            this.name = name;
            this.isAdditive = isAdditive;
            index = -1;
            minLoadTime = 0;
        }

        public SceneLevel(string name, long minLoadTime)
        {
            this.name = name;
            this.minLoadTime = minLoadTime;
            index = -1;
            isAdditive = false;
        }
        #endregion
    }
}