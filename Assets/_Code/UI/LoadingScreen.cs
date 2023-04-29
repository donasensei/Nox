using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Slider progressBar;
        [SerializeField] private Text loadingText;
        [SerializeField] private float textUpdateInterval = 0.15f;

        private const string LOADING_TEXT = "리소스 가져오는 중...";
        private Coroutine loadingTextCoroutine;

        public void SetProgress(float progress)
        {
            progressBar.value = progress;
        }

        private void OnEnable()
        {
            if (loadingText != null)
            {
                loadingTextCoroutine = StartCoroutine(UpdateLoadingText());
            }
        }

        private void OnDisable()
        {
            if (loadingTextCoroutine != null)
            {
                StopCoroutine(loadingTextCoroutine);
            }
        }

        private IEnumerator UpdateLoadingText()
        {
            while (true)
            {
                for (int i = 1; i <= LOADING_TEXT.Length; i++)
                {
                    loadingText.text = LOADING_TEXT[..i];
                    yield return new WaitForSeconds(textUpdateInterval);
                }
            }
        }
    }
}
