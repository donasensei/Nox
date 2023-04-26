using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageData StageData;

    [SerializeField, TextArea] private string stageName;
    public List<Node> nodes;

    [SerializeField] private ExploreCharacter player;

    // State
    private bool isMoving = false;

    // UI 
    [SerializeField] private LocationIndicator locationIndicator;
    [SerializeField] private List<CanvasGroup> canvasGroups;
    [SerializeField] private ErrorDialog errorDialog;
    [SerializeField] private DialogueSystem dialogueSystem;

    // DEBUG
    public string nextSceneName;

    // Texts
    private const string NoneText = "이곳은 아무것도 없습니다.";
    
    // Error Texts
    private const string NodeTypeErrorText = "유효하지 않은 노드 타입입니다.";

    private void Start()
    {
        InitStageData();
        GameManager.Instance.SaveData.currentLocation = stageName;
        GameManager.Instance.state = GameManager.GameState.Explore;

        if (StageData.currentNode != null)
        {
            player.currentNode = StageData.currentNode;
            player.transform.position = StageData.currentNode.transform.position;
            HideAllNodes();
            ShowNode(StageData.currentNode);
            ShowConnectedNodes(StageData.currentNode);
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
            DisableUIinteractions();
        }

        SetCurrentNodeActive();
    }

    // Make sure current Node always ActiveSprite
    private void SetCurrentNodeActive()
    {
        Node currentNode = player.currentNode;
        StageData.currentNode = currentNode;
        currentNode.SetActiveSprites();
    }

    public async void OnNodeClicked(Node targetNode)
    {
        if (!isMoving)
        {
            // Check if the target node is connected to the current node
            if (!player.currentNode.connectedNodes.Contains(targetNode))
            {
                errorDialog.SetErrorText("이동할 수 없는 노드입니다.");
                errorDialog.Show();
                return;
            }

            isMoving = true;

            DisableUIinteractions();

            await player.MoveToNode(targetNode);
            player.currentNode = targetNode;

            HideAllNodes();
            ShowNode(targetNode);
            ShowConnectedNodes(targetNode);

            EnableUIInteractions();

            TriggerNodeEvent(player);
            isMoving = false;
        }
    }

    private void TriggerNodeEvent(ExploreCharacter player)
    {
        Node currentNode = player.currentNode;

        currentNode.visited = true;
        if(currentNode.InkData != null)
        {
            dialogueSystem.StartStory(currentNode.InkData);
        }

        switch (currentNode.nodeType)
        {
            case NodeType.None:
/*              errorDialog.SetErrorText(NoneText);
                errorDialog.Show();*/
                break;
            case NodeType.Battle:
                break;
            case NodeType.GetResource:
                break;
            default:
                errorDialog.SetErrorText(NodeTypeErrorText);
                errorDialog.Show();
                break;
        }

        if (currentNode.connectedNodes.Length <= 0 && nextSceneName != "")
        {
            GameManager.Instance.state = GameManager.GameState.MainMenu;
            CustomSceneManager.Instance.LoadScene(nextSceneName);
        }
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

    private void DisableUIinteractions()
    {
        foreach (var canvasGroup in canvasGroups)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void ShowConnectedNodes(Node node)
    {
        foreach (Node connectedNode in node.connectedNodes)
        {
            connectedNode.gameObject.SetActive(true);
        }
    }

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

    private IEnumerator InitStageDataCoroutine()
    {
        yield return new WaitForEndOfFrame();

        GameManager gameManager = GameManager.Instance;
        StageData = gameManager.SaveData.stageDataList.Find(stageData => stageData.stageName == stageName);

        if (StageData != null)
        {
            // If there is stage data, update the nodes
            StageData.stageName = stageName;
            StageData.nodes = nodes;
        }
        else
        {
            StageData = new StageData(stageName, nodes);
            gameManager.SaveData.stageDataList.Add(StageData);
        }
    }

    private void InitStageData()
    {
        StartCoroutine(InitStageDataCoroutine());
    }

    public void SaveStageData()
    {
        GameManager.Instance.SaveData.stageDataList.Find(stageData => stageData.stageName == stageName).nodes = nodes;
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
