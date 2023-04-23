using UnityEngine.EventSystems;

public interface ISelectableElement
{
    void OnNavigatePerformed(AxisEventData axisEventData);
    void OnSubmitPerformed(BaseEventData eventData);
    void OnCancelPerformed(BaseEventData eventData);
}