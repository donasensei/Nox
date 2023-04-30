using System.Collections.Generic;
using _Code.Character;
using _Code.Dialogue;
using _Code.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Code.Explore
{
    public class Node : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Button button;
        
        // 노드 정보
        public Node[] connectedNodes;
        public NodeType nodeType;
        public bool visited;

        private StageManager _stageManager;

        // Sprites
        public List<Sprite> activeSprites;
        public List<Sprite> inactiveSprites;

        public SpriteRenderer nodeSprite;
        public SpriteRenderer nodeLightSprite;

        // Dialogue Data
        public InkData inkData;
        
        // Enemy Data 
        [Tooltip("전투일 경우만 할당.")]
        public List<CharacterData> enemyCharacters;

        private void Awake()
        {
            _stageManager = FindObjectOfType<StageManager>();
            button.onClick.AddListener(() => _stageManager.OnNodeClicked(this));

            SetInactiveSprites();
        }

        #region 스프라이트 변경 관련

        public void SetActiveSprites()
        {
            nodeSprite.sprite = activeSprites[0];
            nodeLightSprite.sprite = activeSprites[1];
        }

        public void SetInactiveSprites()
        {
            nodeSprite.sprite = inactiveSprites[0];
            nodeLightSprite.sprite = inactiveSprites[1];
        }

        #endregion

        #region 노드 방문 여부

        public void SetVisited()
        {
            visited = true;
        }

        public void SetUnvisited()
        {
            visited = false;
        }

        #endregion

        #region 버튼 이벤트 관련

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

        #endregion
    }

    public enum NodeType
    {
        None,
        Battle,
        GetResource
    }
}