using UnityEngine.EventSystems;

namespace _Code.Interface
{
    public interface ISelectableElement
    {
        void OnNavigatePerformed(AxisEventData axisEventData);
        void OnSubmitPerformed(BaseEventData eventData);
        void OnCancelPerformed(BaseEventData eventData);
    }
}