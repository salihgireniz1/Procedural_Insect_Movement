using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using System;

public class ProceduralLegTarget : MonoBehaviour
{
    [SerializeField] Transform followPoint;
    [SerializeField] Vector3 alignmentOffset;
    [SerializeField] float alignmentThreshold = 0.01f;
    [SerializeField] float jumpPower = 1f; // Jump height
    [SerializeField] float jumpSpeed = 1f; // Jump duration
    [SerializeField] ProceduralLegTarget oppositeLeg; // Reference to the opposite leg

    private Transform _transform;
    private CancellationTokenSource _jumpTokenSource;
    private bool _isJumping;

    private void OnValidate()
    {
        _transform = transform;
    }

    private void LateUpdate()
    {
        if (CanAlignToFollowPoint)
        {
            if (!_isJumping)
            {
                AsyncAlign().Forget();
            }
        }
    }

    private async UniTaskVoid AsyncAlign()
    {
        // Cancel any ongoing jump if a new jump is initiated
        _jumpTokenSource?.Cancel();
        _jumpTokenSource = new CancellationTokenSource();
        var token = _jumpTokenSource.Token;

        _isJumping = true;

        // Calculate the target position
        Vector3 targetPosition = followPoint.position + alignmentOffset;

        try
        {
            float animSpeed = jumpSpeed / (_transform.position - targetPosition).sqrMagnitude;
            // Perform the jump
            var jump = _transform.DOJump(targetPosition, jumpPower, 1, animSpeed)
                            .ToUniTask(cancellationToken: token);
            var rotate = _transform.DORotate(followPoint.rotation.eulerAngles, animSpeed).ToUniTask(cancellationToken: token);

            await UniTask.WhenAll(jump, rotate);
            // Ensure final position and rotation
            _transform.position = targetPosition;
            _transform.rotation = followPoint.rotation;
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation if needed
            Debug.Log("Operation cancelled. Forced to align transform.");
            _transform.position = targetPosition;
            _transform.rotation = followPoint.rotation;
        }
        finally
        {
            _isJumping = false;
        }
    }

    bool CanAlignToFollowPoint => (followPoint.position - _transform.position).sqrMagnitude > alignmentThreshold && IsOppositeLegLanded;

    bool IsOppositeLegLanded => oppositeLeg != null && oppositeLeg.IsLegLanded();

    bool IsLegLanded() => !_isJumping;
}
