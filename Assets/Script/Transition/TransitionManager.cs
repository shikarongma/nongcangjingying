using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MFarm.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        //�ʼ�ĳ���
        public string startSceneName = string.Empty;

        //��ý��뽥�������
        private CanvasGroup fadeCanvasGroup;
        private bool isFade;

        private void Start()
        {
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            StartCoroutine(LoadSceneSetActive(startSceneName));
        }

        //�����л��������¼�
        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }

        //ʹ��Э��
        private void OnTransitionEvent(string sceneName, Vector3 nextPosition)
        {
            if (!isFade)
            {
                StartCoroutine(Transition(sceneName, nextPosition));
            }
                
        }
        /// <summary>
        /// �л�����
        /// </summary>
        /// <param name="sceneName">Ŀ�곡��</param>
        /// <param name="nextPosition">Ŀ��λ��</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName, Vector3 nextPosition)
        {
            //�л�����ǰ��
            EventHandler.CallBeforeSceneUnloadEvent();

            yield return Fade(1);

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            //�������λ��
            EventHandler.CallMoveToPosition(nextPosition);

            yield return LoadSceneSetActive(sceneName);

            //�л�������
            EventHandler.CallAfterSceneUnloadEvent();

            yield return Fade(0);
        }

        /// <summary>
        /// ���س���������Ϊ����
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
        /// ���뽥��
        /// </summary>
        /// <param name="targetAlpha">1�Ǻڣ�0��͸��</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetAlpha)
        {
            isFade = true;
            fadeCanvasGroup.blocksRaycasts = true;
            float speed = Mathf.Abs((fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuration);

            //Mathf.Approximately���ж���������ֵ�Ƿ�����
            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                //public static float MoveTowards(float current, float target, float maxDelta);
                //Mathf.MoveTowards�����Ĺ����Ƿ���һ������������ֵΪcurrent��target������һ��ֵ�����������أ���maxDelta����
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }

            fadeCanvasGroup.blocksRaycasts = false;
            isFade = false;
        }
    }
}

