using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class DialogueResizer : MonoBehaviour
    {
        public GameObject firstUIObject;
        public GameObject secondUIObject;

        private LayoutElement firstLayoutElement;
        private LayoutElement secondLayoutElement;

        void Start()
        {
            firstLayoutElement = firstUIObject.GetComponent<LayoutElement>();
            secondLayoutElement = secondUIObject.GetComponent<LayoutElement>();
        }

        void Update()
        {
            if (!firstUIObject.activeInHierarchy)
            {
                secondLayoutElement.flexibleWidth = 1;
            }
            else
            {
                secondLayoutElement.flexibleWidth = 0;
            }

            if (!secondUIObject.activeInHierarchy)
            {
                firstLayoutElement.flexibleWidth = 1;
            }
            else
            {
                firstLayoutElement.flexibleWidth = 0;
            }
        }
    }
}
