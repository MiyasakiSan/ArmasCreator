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
        private Slider loadingSlider;

        private float targetValue;
        private float currentValue;

        [SerializeField]
        private List<Sprite> bgList = new List<Sprite>();

        [SerializeField]
        private Image bgImage;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            SharedContext.Instance.Add(this);
        }

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadScene(sceneName));
        }

        IEnumerator LoadScene(string sceneName)
        {
            yield return null;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            loadingCanvas.SetActive(true);
            loadingSlider.value = 0;
            targetValue = 0;
            currentValue = 0;

            loadingCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            if (cameraData.cameraStack.Count > 0)
            {
                cameraData.cameraStack[0].gameObject.SetActive(false);
            }

            var bgIndex = UnityEngine.Random.Range(0, bgList.Count - 1);
            bgImage.sprite = bgList[bgIndex];

            while (asyncOperation.isDone)
            {
                //targetValue = asyncOperation.progress / 0.9f;
                //currentValue = Mathf.MoveTowards(currentValue, targetValue, 0.25f * Time.deltaTime);
                loadingSlider.value = currentValue;

                if (Mathf.Approximately(currentValue, 1))
                {
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }

            loadingCanvas.SetActive(false);
        }
    }
}

