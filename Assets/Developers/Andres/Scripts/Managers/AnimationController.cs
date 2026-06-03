using UnityEngine;

public class AnimationController
{
    private string _CurrentState;

    public void ChangeAnimationState(string newState, Animator animator)
    {
        if (_CurrentState == newState)
            return;

        animator.Play(newState);

        _CurrentState = newState;
    }
}
