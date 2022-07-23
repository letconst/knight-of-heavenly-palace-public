/*--------------------------------------------------------------------------------*
  Copyright (C)Nintendo All rights reserved.

  These coded instructions, statements, and computer programs contain proprietary
  information of Nintendo and/or its licensed developers and are protected by
  national and international copyright laws. They may not be disclosed to third
  parties or copied or duplicated in any form, in whole or in part, without the
  prior written consent of Nintendo.

  The content herein is highly confidential and should be handled accordingly.
 *--------------------------------------------------------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using nn.hid;

namespace nns
{
    [AddComponentMenu("Event/Npad Input Module")]
    public class NpadInputModule : BaseInputModule
    {
        [SerializeField]
        private float _moveRepeatDelay = 0.5f;

        public float moveRepeatDelay
        {
            get { return _moveRepeatDelay; }
            set { _moveRepeatDelay = value; }
        }
        
        [SerializeField]
        private float _moveRepeatRate = 0.1f;

        public float moveRepeatRate
        {
            get { return _moveRepeatRate; }
            set { _moveRepeatRate = value; }
        }

        protected Dictionary<NpadId, NpadState> _npadStates;
        private int _npadUpdatedFrame;

        private MoveDirection _lastMoveDirection = MoveDirection.None;
        private float _lastMoveTime;
        private int _moveCount;

        public NpadId[] NpadIds { get; protected set; } = new NpadId[0];

        public void SetNpadIds(NpadId[] npadIds)
        {
            _npadStates = new Dictionary<NpadId, NpadState>(npadIds.Length);
            foreach (var npadId in npadIds)
            {
                _npadStates.Add(npadId, new NpadState());
            }

            NpadIds = npadIds;
        }

        protected override void Start()
        {
            base.Start();

            _npadUpdatedFrame = Time.frameCount - 1;
            
            SetNpadIds(new [] { NpadId.Handheld, NpadId.No1 });
        }

        public override void Process()
        {
#if UNITY_SWITCH
            UpdateNpadStates();
            bool submit = false, cancel = false, left = false, up = false, right = false, down = false;
            foreach (var npadId in NpadIds)
            {
                switch (Npad.GetStyleSet(npadId))
                {
                    case NpadStyle.Handheld:
                    case NpadStyle.FullKey:
                    case NpadStyle.JoyDual:
                        left   |= _npadStates[npadId].GetButton(NpadButton.Left  | NpadButton.StickLLeft);
                        up     |= _npadStates[npadId].GetButton(NpadButton.Up    | NpadButton.StickLUp);
                        right  |= _npadStates[npadId].GetButton(NpadButton.Right | NpadButton.StickLRight);
                        down   |= _npadStates[npadId].GetButton(NpadButton.Down  | NpadButton.StickLDown);
                        submit |= _npadStates[npadId].GetButtonDown(NpadButton.A);
                        cancel |= _npadStates[npadId].GetButtonDown(NpadButton.B);
                        break;
                    case NpadStyle.JoyLeft:
                        left   |= _npadStates[npadId].GetButton(NpadButton.StickLUp);
                        up     |= _npadStates[npadId].GetButton(NpadButton.StickLRight);
                        right  |= _npadStates[npadId].GetButton(NpadButton.StickLDown);
                        down   |= _npadStates[npadId].GetButton(NpadButton.StickLLeft);
                        submit |= _npadStates[npadId].GetButtonDown(NpadButton.Down);
                        cancel |= _npadStates[npadId].GetButtonDown(NpadButton.Left);
                        break;
                    case NpadStyle.JoyRight:
                        left   |= _npadStates[npadId].GetButton(NpadButton.StickRDown);
                        up     |= _npadStates[npadId].GetButton(NpadButton.StickRLeft);
                        right  |= _npadStates[npadId].GetButton(NpadButton.StickRUp);
                        down   |= _npadStates[npadId].GetButton(NpadButton.StickRRight);
                        submit |= _npadStates[npadId].GetButtonDown(NpadButton.X);
                        cancel |= _npadStates[npadId].GetButtonDown(NpadButton.A);
                        break;
                }
            }
            if (submit)
            {
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, GetBaseEventData(), ExecuteEvents.submitHandler);
            }
            if (cancel)
            {
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, GetBaseEventData(), ExecuteEvents.cancelHandler);
            }
            if (eventSystem.sendNavigationEvents)
            {
                float x = right ? 1f : left ? -1f : 0f;
                float y = up ? 1f : down ? -1f : 0f;
                var axisEventData = GetAxisEventData(x, y, 0.6f);
                if (axisEventData.moveDir != MoveDirection.None)
                {
                    if (axisEventData.moveDir != _lastMoveDirection)
                    {
                        _moveCount = 0;
                    }

                    bool shouldMove = true;
                    float time = Time.unscaledTime;
                    if (_moveCount != 0)
                    {
                        if (_moveCount > 1)
                        {
                            shouldMove = time > _lastMoveTime + moveRepeatRate;
                        }
                        else
                        {
                            shouldMove = time > _lastMoveTime + _moveRepeatDelay;
                        }
                    }

                    if (shouldMove)
                    {
                        ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
                        
                        _moveCount += 1;
                        _lastMoveTime = time;
                        _lastMoveDirection = axisEventData.moveDir;
                    }
                }
                else
                {
                    _moveCount = 0;
                    _lastMoveDirection = MoveDirection.None;
                }
            }
#endif
        }

        public override bool ShouldActivateModule()
        {
            if (!base.ShouldActivateModule())
            {
                return false;
            }

            var shouldActivate = false;
#if UNITY_SWITCH
            UpdateNpadStates();
            foreach (var npadId in NpadIds)
            {
                switch (Npad.GetStyleSet(npadId))
                {
                    case NpadStyle.Handheld:
                    case NpadStyle.FullKey:
                    case NpadStyle.JoyDual:
                        shouldActivate |= _npadStates[npadId].GetButton(
                            NpadButton.Left  | NpadButton.StickLLeft |
                            NpadButton.Up    | NpadButton.StickLUp |
                            NpadButton.Right | NpadButton.StickLRight |
                            NpadButton.Down  | NpadButton.StickLDown);
                        shouldActivate |= _npadStates[npadId].GetButton(NpadButton.A | NpadButton.B);
                        break;
                    case NpadStyle.JoyLeft:
                        shouldActivate |= _npadStates[npadId].GetButton(
                            NpadButton.StickLLeft | NpadButton.StickLUp | NpadButton.StickLRight | NpadButton.StickLDown);
                        shouldActivate |= _npadStates[npadId].GetButton(NpadButton.Down | NpadButton.Left);
                        break;
                    case NpadStyle.JoyRight:
                        shouldActivate |= _npadStates[npadId].GetButton(
                            NpadButton.StickRLeft | NpadButton.StickRUp | NpadButton.StickRRight | NpadButton.StickRDown);
                        shouldActivate |= _npadStates[npadId].GetButton(NpadButton.X | NpadButton.A);
                        break;
                }
            }
#endif
            return shouldActivate;
        }

        public override void ActivateModule()
        {
            base.ActivateModule();

            var toSelect = eventSystem.currentSelectedGameObject;
            if (toSelect == null)
            {
                toSelect = eventSystem.firstSelectedGameObject;
            }

            eventSystem.SetSelectedGameObject(toSelect, GetBaseEventData());
        }

        public override bool IsModuleSupported()
        {
#if UNITY_SWITCH
            return true;
#else
            return false;
#endif
        }

#if UNITY_SWITCH
        /// <summary> This function updates npad state only the first time in a frame, even if it is called more than once in the frame. </summary>
        private void UpdateNpadStates()
        {
            if (_npadUpdatedFrame == Time.frameCount)
            {
                return;
            }
            _npadUpdatedFrame = Time.frameCount;

            foreach (var npadId in NpadIds)
            {
                NpadState tmpState = _npadStates[npadId];
                NpadStyle npadStyle = Npad.GetStyleSet(npadId);
                Npad.GetState(ref tmpState, npadId, npadStyle);
                _npadStates[npadId] = tmpState;
            }
        }
#endif
    }
}
