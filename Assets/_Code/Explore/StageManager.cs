using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Code.Battle;
using _Code.Dialogue;
using _Code.Managers;
using _Code.UI;
using EasyTransition;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Code.Explore
{
    public class StageManager : MonoBehaviour
    {
        // Stage Elements
        [FormerlySerializedAs("StageData")] public StageData stageData;
        [SerializeField, TextArea] private string stageName;
        [SerializeField] private List<Node> nodes;
        [SerializeField] private ExploreCharacter player;

        // Player State
        private bool _isMoving = false;
        
        // Managers 
        private GameManager _gameManager;
        private TransitionManager _transitionManager;

        // UI 
        [SerializeField] private LocationIndicator locationIndicator;
        [SerializeField] private List<CanvasGroup> canvasGroups;
        [SerializeField] private ErrorDialog errorDialog;
        [SerializeField] private DialogueSystem dialogueSystem;
        
        // Dialogue
        [SerializeField] private GameObject dialogueSystemPrefab;

        // DEBUG
        public string nextSceneName;
    
        // Texts
        private const string NoneText = "이곳은 아무것도 없습니다.";
    
        // Error Texts
        private const string NodeTypeErrorText = "유효하지 않은 노드 타입입니다.";

        private void Awake()
        {
            _gameManager = GameManager.instance;
            _isMoving = false;
        }

        private void Start()
        {
            InitStageData();
            _gameManager.saveData.currentLocation = stageName;
            if (_gameManager.saveData != null && _gameManager.saveData.dayNight != DayNight.Night)
            {
                _gameManager.saveData.dayNight = DayNight.Night;
            }

            if (stageData.currentNode != null)
            {
                player.currentNode = stageData.currentNode;
                player.transform.position = stageData.currentNode.transform.position;
                HideAllNodes();
                ShowNode(stageData.currentNode);
                ShowConnectedNodes(stageData.currentNode);
                TriggerNodeEvent(player);
            }
            else
            {
                player.currentNode = nodes[0];
                player.transform.position = nodes[0].transform.position;
                HideAllNodes();
                ShowNode(nodes[0]);
                ShowConnectedNodes(nodes[0]);
                TriggerNodeEvent(player);
            }
        }

        private void Update()
        {
            locationIndicator.SetPercentage(CalculatePercentage());
            locationIndicator.SetLocationText(stageName);

            if (!dialogueSystem.gameObject.activeSelf)
            {
                EnableUIInteractions();
            }
            else
            {
                DisableInteractions();
            }

            SetCurrentNodeActive();
        }

        // Make sure current Node always ActiveSprite
        private void SetCurrentNodeActive()
        {
            Node currentNode = player.currentNode;
            stageData.currentNode = currentNode;
            currentNode.SetActiveSprites();
        }

        public async void OnNodeClicked(Node targetNode)
        {
            if (!_isMoving)
            {
                // Check if the target node is connected to the current node
                if (!player.currentNode.connectedNodes.Contains(targetNode))
                {
                    errorDialog.SetErrorText("이동할 수 없는 노드입니다.");
                    errorDialog.Show();
                    return;
                }

                _isMoving = true;

                DisableInteractions();

                await player.MoveToNode(targetNode);
                player.currentNode = targetNode;

                HideAllNodes();
                ShowNode(targetNode);
                ShowConnectedNodes(targetNode);

                EnableUIInteractions();

                TriggerNodeEvent(player);
                _isMoving = false;
            }
        }

        private void TriggerNodeEvent(ExploreCharacter pl)
        {
            // 플레이어의 현재 위치
            var currentNode = pl.currentNode; 
            
            currentNode.visited = true; // 방문한 것으로 표시
            
            // 다이얼로그 데이터가 할당된 경우 스토리부터 진행
            if(currentNode.inkData != null)
            {
                // 다이얼로그 시스템이 없는 경우 생성
                if (dialogueSystem == null)
                {
                    dialogueSystem = Instantiate(dialogueSystemPrefab).GetComponent<DialogueSystem>();
                }
                dialogueSystem.SetInkData(currentNode.inkData);
                return;
            }
            
            switch (currentNode.nodeType)
            {
                case NodeType.None:
                    break;
                case NodeType.Battle:
                    BattleDataTransfer.Instance.enemyCharacters = currentNode.enemyCharacters;
                    break;
                case NodeType.GetResource:
                    break;
                default:
                    errorDialog.SetErrorText(NodeTypeErrorText);
                    errorDialog.Show();
                    break;
            }
            if (currentNode.connectedNodes.Length > 0 || nextSceneName == "") return;
            GameManager.instance.state = GameState.MainMenu;
            _transitionManager.LoadScene(nextSceneName, "Fade", 1f);
        }

        private void EnableUIInteractions()
        {
            // Enable UI interactions
            foreach(var canvasGroup in canvasGroups)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        private void DisableInteractions()
        {
            foreach (var canvasGroup in canvasGroups)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }

        private static void ShowConnectedNodes(Node node)
        {
            foreach (var connectedNode in node.connectedNodes)
            {
                connectedNode.gameObject.SetActive(true);
            }
        }

        #region 노드 시각화

        private void HideAllNodes()
        {
            foreach (Node node in nodes)
            {
                HideNode(node);
            }
        }

        private void ShowAllNodes()
        {
            foreach (Node node in nodes)
            {
                ShowNode(node);
            }
        }

        private void HideNode(Node node)
        {
            node.gameObject.SetActive(false);
        }

        private void ShowNode(Node node)
        {
            node.gameObject.SetActive(true);
        }

        #endregion
        
        private IEnumerator InitStageDataCoroutine()
        {
            yield return new WaitForEndOfFrame();
            stageData = _gameManager.saveData.stageDataList.Find(stageData =>
            {
                if (stageData == null) throw new ArgumentNullException(nameof(stageData));
                return stageData.stageName == stageName;
            });

            if (stageData != null)
            {
                // If there is stage data, update the nodes
                stageData.stageName = stageName;
                stageData.nodes = nodes;
            }
            else
            {
                stageData = new StageData(stageName, nodes);
                _gameManager.saveData.stageDataList.Add(stageData);
            }
        }

        private void InitStageData()
        {
            StartCoroutine(InitStageDataCoroutine());
        }

        public void SaveStageData()
        {
            _gameManager.saveData.stageDataList.Find(stageData => stageData.stageName == stageName).nodes = nodes;
        }

        private int CalculatePercentage()
        {
            // Calculate the percentage of the stage
            int visitedNodeCount = nodes.Count(node => node.visited);
            int percentage = (int)((float)visitedNodeCount / nodes.Count * 100);
            return percentage;
        }
    }

    [System.Serializable]
    public class StageData
    {
        public string stageName;
        public List<Node> nodes;

        public Node currentNode;

        public StageData(string stageName, List<Node> nodes)
        {
            this.stageName = stageName;
            this.nodes = nodes;
        }
    }
}