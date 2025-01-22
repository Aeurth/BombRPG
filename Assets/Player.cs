using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerState state;
    Animator animator;

    // Initialize the Heroine with a starting state
    public void Initialize(PlayerState initialState)
    {
        //state = initialState;
    }
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Unity's Update method
    private void Update()
    {
        // Delegate update behavior to the current state
        if (state != null)
        {
            state.Update(this);
        }
    }

    // Handle PlayerInputs
    public void HandlePlayerInputs(PlayerInputs PlayerInputs)
    {
        if (state != null)
        {
            state.HandleInput(this, PlayerInputs);
        }
    }

    // Change the current state
    public void ChangeState(PlayerState newState)
    {
        if (newState != null)
        {
            state = newState;
            state.Enter(this);
        }

    }

    // Example method to visualize state change
    public void LogCurrentState(string stateName)
    {
        Debug.Log($"Player changed to state: {stateName}");
    }
    public void SetAnimationParam(PlayerInputs input, bool value)
    {
        if(animator  != null)
            animator.SetBool(input.ToString(), value);
    }
    public void PlayAnimation(string stateName)
    {
        if (animator != null)
        {
            animator.Play(stateName);
        }
    }
}

