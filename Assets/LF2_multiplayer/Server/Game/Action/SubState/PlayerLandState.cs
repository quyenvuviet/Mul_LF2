using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LF2.Server{
    public class PlayerLandState : State
    {
        float timenow;

        public PlayerLandState(CharacterTypeEnum characterType, PlayerState player) : base(characterType, player)
        {
        }

        public override void CanChangeState(StateRequestData actionRequestData)
        {

        }

        public override void Enter()
        {
            base.Enter();
            m_ActionRequestData.StateTypeEnum = StateType.Land;
            player.serverplayer.NetState.RecvDoActionClientRPC(m_ActionRequestData);
            timenow = Time.time;
        }

        public override StateType GetId()
        {
            return StateType.Land;
        }

        public override void PhysicsUpdate()
        {

            // if (JumpInput && player.JumpState.CanJump()){
                
            //     stateMachine.ChangeState(player.DoubleJumpState);
            // }
            // else if (DefenseInput)
            // {
            //     stateMachine.ChangeState(player.RollingState);
            // }
            // else if(isAnimationFinished ) {
            //     stateMachine.ChangeState(player.IdleState);
            // }
            Debug.Log("Land");
            if (Time.time - timenow >0.2f){
                player.stateMachine.ChangeState(StateType.Idle );

            }
        }


    }
}
