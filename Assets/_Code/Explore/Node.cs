using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public Node[] connectedNodes;
    public NodeType nodeType;
    public bool visited;
}

public enum NodeType
{
    None,
    Battle,
    GetResource
}