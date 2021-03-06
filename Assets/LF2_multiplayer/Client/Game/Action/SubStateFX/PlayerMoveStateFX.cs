using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LF2.Visual{
    public class PlayerMoveStateFX : StateFX
    {
        public PlayerMoveStateFX(CharacterTypeEnum characterType, PlayerStateFX m_PlayerFX) : base(characterType, m_PlayerFX)
        {
        }

        public override void AnticipateState(StateRequestData data)
        {
            if (data.StateTypeEnum == StateType.Attack || data.StateTypeEnum == StateType.Jump ){
                Anticipated = true;
                m_PlayerFX.stateMachineViz.GetState(data.StateTypeEnum).PlayAnim(data.StateTypeEnum);
            }


        }

        public override void SetMovementTarget(Vector2 position)
        {
            if (position == Vector2.zero ){
                m_PlayerFX.stateMachineViz.ChangeState(StateType.Idle);
            }
        }

        

        public override void Enter()
        {
            if( !Anticipated)
            {
                PlayAnim(m_PlayerFX.stateMachineViz.CurrentStateViz);
            }
            base.Enter();
        }
        public override void PlayAnim(StateType currentState)
        {
            m_PlayerFX.m_ClientVisual.OurAnimator.Play("Walk_anim");
        }

        public override StateType GetId()
        {
            return StateType.Move;
        }



        public override bool LogicUpdate()
        {
            Debug.Log("MoveState Visual");
            return true;
        }



        // public override void PhysicsUpdate()
        // {
        //     base.PhysicsUpdate();
        // }


    }
}

