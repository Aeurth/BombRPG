using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerState
{
    public abstract void HandleInput(Player player, PlayerInputs playerInputs);
    public abstract void Update(Player player);
    public abstract void Enter(Player player);
    public abstract PlayerInputs GetState();
   
}

public class IdleState : PlayerState
{
    public override void Enter(Player player)
    {
        //set the player animation to match current state
        player.SetAnimationParam(PlayerInputs.IsIdle, true);
    }

    public override PlayerInputs GetState()
    {
        return PlayerInputs.IsIdle;
    }

    public override void HandleInput(Player player, PlayerInputs playerInputs)
    {
        if (playerInputs == PlayerInputs.IsWalking)
        {
            //turn off current player state animation
            player.SetAnimationParam(PlayerInputs.IsIdle, false);
            player.ChangeState(new WalkingState());

        }else if (playerInputs == PlayerInputs.IsRunning)
        {
            //turn off current player state animation
            player.SetAnimationParam(PlayerInputs.IsIdle, false);
            player.ChangeState(new RunningState());
        }
        else if (playerInputs == PlayerInputs.IsPlacing)
        {
            player.ChangeState(new PlacingState());
        }
        else if (playerInputs == PlayerInputs.IsDetonating)
        {
            player.ChangeState(new DetonatingState());
        }
    }

    public override void Update(Player player)
    {
        // Idle state update logic
    }

}
public class WalkingState : PlayerState
{
    public override void Enter(Player player)
    {
        player.SetAnimationParam(PlayerInputs.IsWalking, true);
    }

    public override PlayerInputs GetState()
    {
        return PlayerInputs.IsWalking;
    }

    public override void HandleInput(Player player, PlayerInputs playerInputs)
    {

        if (playerInputs == PlayerInputs.IsIdle)
        {
            //turn off current player state animation
            player.SetAnimationParam(PlayerInputs.IsWalking, false);
            player.ChangeState(new IdleState());

        }
        else if (playerInputs == PlayerInputs.IsRunning)
        {
            //turn off current player state animation
            player.SetAnimationParam(PlayerInputs.IsWalking, false);
            player.ChangeState(new RunningState());
        }
        else if (playerInputs == PlayerInputs.IsPlacing)
        {
            player.ChangeState(new PlacingState());
        }
        else if (playerInputs == PlayerInputs.IsDetonating)
        {
            player.ChangeState(new DetonatingState());
        }
    }

    public override void Update(Player player)
    {
    }
}
public class RunningState : PlayerState
{
    float _running = 0f; 
    public override void Enter(Player player)
    {
        player.SetAnimationParam(PlayerInputs.IsRunning, true);
    }
    public override PlayerInputs GetState()
    {
        return PlayerInputs.IsRunning;
    }
    public override void HandleInput(Player player, PlayerInputs playerInputs)
    {
        if (playerInputs == PlayerInputs.IsIdle)
        {
            //turn off current player state animation
            player.SetAnimationParam(PlayerInputs.IsRunning, false);
            player.ChangeState(new IdleState());

        }else if(playerInputs == PlayerInputs.IsWalking)
        {
            //turn off current player state animation
            player.SetAnimationParam(PlayerInputs.IsRunning, false);
            player.ChangeState(new WalkingState());
        }
        else if (playerInputs == PlayerInputs.IsPlacing)
        {
            player.ChangeState(new PlacingState());
        }
        else if (playerInputs == PlayerInputs.IsDetonating)
        {
            player.ChangeState(new DetonatingState());
        }
    }

    public override void Update(Player player)
    {
        //_running += Time.deltaTime;
       // if(_running > player.stamina) { 
        //    player.ChangeState(new WalkingState());
       // }
    }
}
public class PlacingState : PlayerState
{
    float animationDuration = 0.5f;
    float duration = 0;
    public override void Enter(Player player)
    {
        //set the player animation to match current state
            player.PlayAnimation("Dummy_root_Dummy_root_Pickup");
        
    }
    public override PlayerInputs GetState()
    {
        return PlayerInputs.IsPlacing;
    }
    public override void HandleInput(Player player, PlayerInputs playerInputs)
    {
       
    }

    public override void Update(Player player)
    {
        duration++;
        if(duration > animationDuration)
        {
            player.ChangeState(new IdleState());
        }
    }
}
public class DetonatingState: PlayerState
{
    float duration = 2.840f;
    float currentDuration = 0;
    public override void Enter(Player player)
    {
        //set the player animation to match current state
        player.PlayAnimation("Dummy_root_Dummy_root_Joy");
    }
    public override PlayerInputs GetState()
    {
        return PlayerInputs.IsDetonating;
    }
    public override void HandleInput(Player player, PlayerInputs playerInputs)
    {
        if (playerInputs == PlayerInputs.IsWalking)
        {
            //turn off current player state animation
            player.SetAnimationParam(PlayerInputs.IsDetonating, false);
            player.ChangeState(new WalkingState());

        }
        else if (playerInputs == PlayerInputs.IsIdle)
        {
            //turn off current player state animation
            player.SetAnimationParam(PlayerInputs.IsDetonating, false);
            player.ChangeState(new IdleState());
        }
    }

    public override void Update(Player player)
    {
        currentDuration++;
        if (currentDuration > duration) {
           player.ChangeState(new IdleState());
        }
        
    }
}