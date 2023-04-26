using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Node : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;

    public Node[] connectedNodes;
    public NodeType nodeType;
    public bool visited;

    private StageManager stageManager;

    // Sprites
    public List<Sprite> activeSprites;
    public List<Sprite> inactiveSprites;

    public SpriteRenderer NodeSprite;
    public SpriteRenderer NodeLightSprite;

    // Dialogue Data
    public InkData inkData;

    private void Awake()
    {
        stageManager = FindObjectOfType<StageManager>();
        button.onClick.AddListener(() => stageManager.OnNodeClicked(this));

        SetInactiveSprites();
    }

    public InkData InkData { get { return inkData; } }

    public void SetActiveSprites()
    {
        NodeSprite.sprite = activeSprites[0];
        NodeLightSprite.sprite = activeSprites[1];
    }

    public void SetInactiveSprites()
    {
        NodeSprite.sprite = inactiveSprites[0];
        NodeLightSprite.sprite = inactiveSprites[1];
    }

    public void SetVisited()
    {
        visited = true;
    }

    public void SetUnvisited()
    {
        visited = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!visited)
        {
            SetActiveSprites();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!visited)
        {
            SetInactiveSprites();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!visited)
        {
            SetActiveSprites();
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!visited)
        {
            SetInactiveSprites();
        }
    }

    private void OnEnable()
    {
        button.navigation = new Navigation { mode = Navigation.Mode.None };
    }
}

public enum NodeType
{
    None,
    Battle,
    GetResource
}