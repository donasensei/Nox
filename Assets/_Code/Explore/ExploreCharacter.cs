using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class ExploreCharacter : MonoBehaviour
{
    public Node currentNode;
    public float moveDuration = 1f;

    public async Task MoveToNode(Node targetNode)
    {
        // Stop any existing movement tweens
        transform.DOKill();

        // Create a task completion source
        TaskCompletionSource<bool> tcs = new();

        // Move the player smoothly to the target node
        transform.DOMove(targetNode.transform.position, moveDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                currentNode = targetNode;
                tcs.SetResult(true);
            });

        // Wait for the movement to complete
        await tcs.Task;
    }
}
