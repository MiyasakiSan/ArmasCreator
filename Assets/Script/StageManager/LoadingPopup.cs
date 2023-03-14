using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ArmasCreator.Utilities;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

namespace ArmasCreator.UI
{
    public class LoadingPopup : MonoBehaviour
    {
        [SerializeField]
        private GameObject loadingCanvas;

        [SerializeField]
        private GameObject content;

        [SerializeField]
        private Slider loadingSlider;

        [SerializeField]
        private Image FadeBlackImage;

        private float targetValue;
        private float currentValue;

        [SerializeField]
        private List<Sprite> bgList = new List<Sprite>();

        [SerializeField]
        private LoadingPopupBackGround LPBG;

        [SerializeField]
        private Image bgImage;

        public delegate void OnLoadingSceneFinishedCallback();
        public event OnLoadingSceneFinishedCallback OnLoadingSceneFinished;

        private bool isFadingBlack = false;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            SharedContext.Instance.Add(this);
        }

        public void LoadSceneAsync(string sceneName)
        {
            loadingCanvas.SetActive(true);
            content.SetActive(true);
            StartCoroutine(LoadScene(sceneName));
        }

        public void ChangeBG()
        {
            var bgIndex = UnityEngine.Random.Range(0, bgList.Count - 1);
            bgImage.sprite = bgList[bgIndex];
        }

        IEnumerator LoadScene(string sceneName)
        {
            //yield return new WaitUntil(() => loadingCanvas.activeSelf);

            //yield return new WaitUntil(() => content.activeSelf);
            LPBG.Reset();
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            loadingSlider.value = 0;
            targetValue = 0;
            currentValue = 0;

            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            if (cameraData.cameraStack.Count > 0)
            {
                bool cameraUIExist = false;
                foreach (Camera stackCamera in cameraData.cameraStack)
                {
                    if (stackCamera.gameObject.name == "UICamera")
                    {
                        loadingCanvas.GetComponent<Canvas>().worldCamera = stackCamera;
                        cameraUIExist = true;
                    }
                }
                if (cameraUIExist == false)
                {
                    loadingCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
                }
            }
            else
            {
                loadingCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            }

            var bgIndex = UnityEngine.Random.Range(0, bgList.Count - 1);
            bgImage.sprite = bgList[bgIndex];

            LPBG.Fade();

            while (!asyncOperation.isDone)
            {
                targetValue = asyncOperation.progress / 0.9f;
                currentValue = Mathf.MoveTowards(currentValue, targetValue, 0.5f * Time.deltaTime);
                loadingSlider.value = currentValue;

                if (Mathf.Approximately(currentValue, 1))
                {
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }

            OnLoadingSceneFinished?.Invoke();
            cameraData = Camera.main.GetUniversalAdditionalCameraData();
            if (cameraData.cameraStack.Count > 0)
            {
                bool cameraUIExist = false;
                foreach (Camera stackCamera in cameraData.cameraStack)
                {
                    if (stackCamera.gameObject.name == "UICamera")
                    {
                        loadingCanvas.GetComponent<Canvas>().worldCamera = stackCamera;
                        cameraUIExist = true;
                    }
                }
                if(cameraUIExist == false)
                {
                    loadingCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
                }
            }
            else
            {
                loadingCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            }

            LPBG.Reset();
            FadeBlack();
        }

        public void FadeBlack(bool isShowContent = true)
        {
            //TODO : Fade black in and out
            if (!isShowContent)
            {
                content.SetActive(false);
            }

            if (!loadingCanvas.activeSelf)
            {
                loadingCanvas.SetActive(true);
            }

            content.SetActive(true);
            isFadingBlack = true;
            StartCoroutine(FadeBlackCoroutine());
            Debug.Log("Fade Black");
        }

        IEnumerator FadeBlackCoroutine()
        {
            while (FadeBlackImage.color.a < 1)
            {
                var tempColor = FadeBlackImage.color;
                tempColor.a += 0.05f;
                FadeBlackImage.color = tempColor;
                yield return new WaitForSeconds(0.025f);
            }

            content.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            while (FadeBlackImage.color.a > 0)
            {
                var tempColor = FadeBlackImage.color;
                tempColor.a -= 0.05f;
                FadeBlackImage.color = tempColor;
                yield return new WaitForSeconds(0.025f);
            }

            isFadingBlack = false;
            loadingCanvas.SetActive(false);
        }
    }
}

