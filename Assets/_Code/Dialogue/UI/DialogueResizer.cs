using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class DialogueResizer : MonoBehaviour
    {
        public GameObject firstUIObject;
        public GameObject secondUIObject;

        private LayoutElement _firstLayoutElement;
        private LayoutElement _secondLayoutElement;

        void Start()
        {
            _firstLayoutElement = firstUIObject.GetComponent<LayoutElement>();
            _secondLayoutElement = secondUIObject.GetComponent<LayoutElement>();
        }

        void Update()
        {
            if (!firstUIObject.activeInHierarchy)
            {
                _secondLayoutElement.flexibleWidth = 1;
            }
            else
            {
                _secondLayoutElement.flexibleWidth = 0;
            }

            if (!secondUIObject.activeInHierarchy)
            {
                _firstLayoutElement.flexibleWidth = 1;
            }
            else
            {
                _firstLayoutElement.flexibleWidth = 0;
            }
        }
    }
}
