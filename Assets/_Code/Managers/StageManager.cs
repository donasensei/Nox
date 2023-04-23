using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private string stageName;
    public Node[] nodes;
    public ExploreCharacter playerPrefab;

    private ExploreCharacter playerInstance;
    private bool isMoving = false;

    // Methods

    private void Start()
    {
        InitializeStageData();
    }

    private void InitializeStageData()
    {
        StageData stageData = GameManager.Instance.SaveData.stageDataList.Find(sd => sd.stageName == stageName);

        if (stageData != null)
        {
            // Load StageData
        }
        else
        {
            // Create StageData and Save it to SaveData
            stageData = new StageData(stageName, nodes);
            GameManager.Instance.SaveData.stageDataList.Add(stageData);
        }
    }
    private async void Update()
    {
        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            HandleUserInput();
        }
    }

    private async void HandleUserInput()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Node targetNode = hit.collider.GetComponent<Node>();
            if (targetNode != null && System.Array.IndexOf(playerInstance.currentNode.connectedNodes, targetNode) >= 0)
            {
                isMoving = true;
                await playerInstance.MoveToNode(targetNode);
                isMoving = false;
            }
        }
    }
}

[System.Serializable]
public class StageData
{
    public string stageName;
    public Node[] nodes;

    public StageData(string stageName, Node[] nodes)
    {
        this.stageName = stageName;
        this.nodes = nodes;
    }
}
