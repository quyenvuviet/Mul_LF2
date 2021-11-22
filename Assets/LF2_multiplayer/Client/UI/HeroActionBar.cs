using MLAPI;
using MLAPI.Spawning;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
// using SkillTriggerStyle = LF2.Client.ClientInputSender.SkillTriggerStyle;
using UnityEngine.InputSystem.OnScreen;

namespace LF2.Visual
{
    /// <summary>
    /// Provides logic for a Hero Action Bar with attack, skill buttons and a button to open emotes panel
    /// This bar tracks button clicks on hero action buttons for later use by ClientInputSender
    /// </summary>

    public class HeroActionBar : MonoBehaviour
    {
        [SerializeField]
        AttackButton m_AttackButton;

        [SerializeField]
        JumpButton m_JumpButton;

        [SerializeField]
        DefenseButton m_DefenseButton;

        [SerializeField]
        JoystickScreen m_JoystickScreen;

        [SerializeField]
        DownSlotButton m_DownSlotButton;

        [SerializeField]
        UpSlotButton m_UpSlotButton;

        /// <summary>
        /// Our input-sender. Initialized in RegisterInputSender()
        /// </summary>
        Client.ClientInputSender m_InputSender;

        /// <summary>
        /// Cached reference to local player's net state.
        /// We find the Sprites to use by checking the Skill1, Skill2, and Skill3 members of our chosen CharacterClass
        /// </summary>
        NetworkCharacterState m_NetState;

        /// <summary>
        /// If we have another player selected, this is a reference to their stats; if anything else is selected, this is null
        /// </summary>
        NetworkCharacterState m_SelectedPlayerNetState;

        /// <summary>
        /// If m_SelectedPlayerNetState is non-null, this indicates whether we think they're alive. (Updated every frame)
        /// </summary>
        bool m_WasSelectedPlayerAliveDuringLastUpdate;

        private float m_InputHoldTime = 0.2f;

        private float InputPressedStartTime ;

        private bool InputPressed ;

        public float AttackPressedStartTime { get; private set; }
        public bool AttackPressed { get; private set; }

        public float JumpPressedStartTime { get; private set; }
        public bool JumpPressed { get; private set; }

        public float DefensePressedStartTime { get; private set; }
        public bool DefensePressed { get; private set; }

        /// <summary>
        /// Identifiers for the buttons on the action bar.
        /// </summary>





        /// <summary>
        /// Called during startup by the ClientInputSender. In response, we cache the provided
        /// inputSender and self-initialize.
        /// </summary>
        /// <param name="inputSender"></param>
        public void RegisterInputSender(Client.ClientInputSender inputSender)
        {
            if (m_InputSender != null)
            {
                Debug.LogWarning($"Multiple ClientInputSenders in scene? Discarding sender belonging to {m_InputSender.gameObject.name} and adding it for {inputSender.gameObject.name} ");
            }

            m_InputSender = inputSender;
            m_NetState = m_InputSender.GetComponent<NetworkCharacterState>();
            // m_NetState.TargetId.OnValueChanged += OnSelectionChanged;
            // UpdateAllActionButtons();
        }


        void OnEnable()
        {


            m_JoystickScreen.SendControlValue += JoystickDrag;
            m_AttackButton.AttackAction += OnAtack;
            m_DefenseButton.DefenseAction += OnDefense;
            m_JumpButton.JumpAction += OnJump;


        }

        void OnDisable()
        {
            // foreach (ActionButtonInfo buttonInfo in m_ButtonInfo.Values)
            // {
            //     buttonInfo.UnregisterEventHandlers();
            // }
        }

        void OnDestroy()
        {
            m_JoystickScreen.SendControlValue -= JoystickDrag;
            m_AttackButton.AttackAction -= OnAtack;
            m_DefenseButton.DefenseAction -= OnDefense;
            m_JumpButton.JumpAction -= OnJump;
        }

        void Update()
        {
            // If we have another player selected, see if their aliveness state has changed,
            // and if so, update the interactiveness of the basic-action button

            if (!m_SelectedPlayerNetState) { return; }

            bool isAliveNow = m_SelectedPlayerNetState.NetworkLifeState.LifeState.Value == LifeState.Alive;
            if (isAliveNow != m_WasSelectedPlayerAliveDuringLastUpdate)
            {
                // this will update the icons so that the basic-action button's interactiveness is correct
                // UpdateAllActionButtons();
            }

            m_WasSelectedPlayerAliveDuringLastUpdate = isAliveNow;


            
        }


        
        void OnAtack()
        {
            if (AttackPressed){
                if ( Time.time >= AttackPressedStartTime + m_InputHoldTime){
                    AttackPressed = false;
                }
            } 

            if (!AttackPressed){
                AttackPressed = true;
                AttackPressedStartTime = Time.time;
                // send input to begin the action associated with this button
                m_InputSender.RequestAction(StateType.Attack);
            }
        }

        void OnJump()
        {
            if (JumpPressed){
                if ( Time.time >= JumpPressedStartTime + m_InputHoldTime){
                    JumpPressed = false;
                }
            } 

            if (!JumpPressed){
                JumpPressed = true;
                JumpPressedStartTime = Time.time;
                // send input to begin the action associated with this button
                m_InputSender.RequestAction(StateType.Jump);
            }
        }

        void OnDefense()
        {
            if (DefensePressed){
                if ( Time.time >= DefensePressedStartTime + m_InputHoldTime){
                    DefensePressed = false;
                }
            } 

            if (!DefensePressed){
                DefensePressed = true;
                DefensePressedStartTime = Time.time;
                // send input to begin the action associated with this button
                m_InputSender.RequestAction(StateType.Defense);
            }
        }



        void JoystickDrag(Vector2 position)
        {

            // send position to begin the move 
            m_InputSender.OnMoveInputUI(position);
        }





    }
}
