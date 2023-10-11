using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    // 추상 메서드: 각 상태에서 구현할 동작들
    public abstract void EnterState(Player player);
    public abstract void UpdateState(Player player);
    public abstract void ExitState(Player player);
}

public class IdleState : PlayerState
{
    public override void EnterState(Player player)
    {
        player.anim.SetBool("Walk", false);
        player.anim.SetBool("Idle", true);
    }

    public override void UpdateState(Player player)
    {
        player.Idle();
    }

    public override void ExitState(Player player)
    {
        player.anim.SetBool("Idle", false);
    }
}

// Moving 상태를 나타내는 클래스
public class MovingState : PlayerState
{
    public override void EnterState(Player player)
    {
        player.anim.SetBool("Walk", true);
    }

    public override void UpdateState(Player player)
    {
        player.Move();
    }

    public override void ExitState(Player player)
    {
        player.anim.SetBool("Walk", false);
    }
}

// Jumping 상태를 나타내는 클래스
public class RollingState : PlayerState
{
    public override void EnterState(Player player)
    {
        player.anim.SetTrigger("Rolling");
    }

    public override void UpdateState(Player player)
    {
        
    }

    public override void ExitState(Player player)
    {
        
    }
}