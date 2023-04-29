using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Code.Explore
{
    public class ExploreCharacter : MonoBehaviour
    {
        public Node currentNode;
        [SerializeField] private float moveDuration = 1f;

        public async Task MoveToNode(Node targetNode)
        {
            transform.DOKill();
            TaskCompletionSource<bool> tcs = new();

            // Move the player smoothly to the target node
            transform.DOMove(targetNode.transform.position, moveDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    currentNode = targetNode;
                    tcs.SetResult(true);
                });

            await tcs.Task;
        }
    }
}
