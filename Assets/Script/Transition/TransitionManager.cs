using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MFarm.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        //最开始的场景
        public string startSceneName = string.Empty;

        //获得渐入渐出的组件
        private CanvasGroup fadeCanvasGroup;
        private bool isFade;

        private void Start()
        {
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            StartCoroutine(LoadSceneSetActive(startSceneName));
        }

        //接受切换场景的事件
        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }

        //使用协程
        private void OnTransitionEvent(string sceneName, Vector3 nextPosition)
        {
            if (!isFade)
            {
                StartCoroutine(Transition(sceneName, nextPosition));
            }
                
        }
        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="sceneName">目标场景</param>
        /// <param name="nextPosition">目标位置</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName, Vector3 nextPosition)
        {
            //切换场景前：
            EventHandler.CallBeforeSceneUnloadEvent();

            yield return Fade(1);

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            //更改玩家位置
            EventHandler.CallMoveToPosition(nextPosition);

            yield return LoadSceneSetActive(sceneName);

            //切换场景后：
            EventHandler.CallAfterSceneUnloadEvent();

            yield return Fade(0);
        }

        /// <summary>
        /// 加载场景并设置为激活
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

            SceneManager.SetActiveScene(newScene);
           
        }

        /// <summary>
        /// 渐入渐出
        /// </summary>
        /// <param name="targetAlpha">1是黑，0是透明</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetAlpha)
        {
            isFade = true;
            fadeCanvasGroup.blocksRaycasts = true;
            float speed = Mathf.Abs((fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuration);

            //Mathf.Approximately是判断里面两个值是否相似
            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                //public static float MoveTowards(float current, float target, float maxDelta);
                //Mathf.MoveTowards方法的功能是返回一个浮点数，其值为current向target靠近的一个值。靠近多少呢？由maxDelta决定
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }

            fadeCanvasGroup.blocksRaycasts = false;
            isFade = false;
        }
    }
}

