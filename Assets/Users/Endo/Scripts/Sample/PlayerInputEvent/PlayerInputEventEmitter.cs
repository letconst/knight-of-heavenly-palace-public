using UniRx;
using UniRx.Triggers;

namespace Endo.Sample.PlayerInputEvent
{
    public sealed class PlayerInputEventEmitter : SingletonMonoBehaviour<PlayerInputEventEmitter>
    {
        // SwitchInputManagerのインスタンスキャッシュ用
        private SwitchInputManager _switchInput;

        // イベント通知送受信用
        private readonly MessageBroker _broker = new();

        // イベント通知送受信の公開用
        public IMessageBroker Broker => _broker;

        private void Start()
        {
            _switchInput = SwitchInputManager.Instance;

            this.OnDestroyAsObservable()
                .Subscribe(_ => _broker.Dispose())
                .AddTo(this);
        }

        private void Update()
        {
            InputHandler();
        }

        /// <summary>
        /// Joy-Con入力およびプレイヤーの状態などに応じて、対応する入力操作のイベントを発行する
        /// </summary>
        private void InputHandler()
        {
            // L入力
            if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.L))
            {
                /* イベントを発行可能（操作を実行可能）かを、まず確認する */

                // UI操作中なら終了
                // if (/* UI操作中か */) return;

                /* 発行可能なら、必要に応じて準備をして発行 */

                // 入力情報用意
                var actionInfo = new PlayerActionInfo
                {
                    actHand = PlayerHand.Left
                };

                // 左手が投擲モードなら
                // if (/* 左手が投擲モードか */)
                {
                    // 左手での攻撃モード切り替え入力の通知を発行
                    _broker.Publish(PlayerEvent.Input.OnSwitchedAttack.GetEvent(actionInfo));
                }

                // 左手が攻撃モードなら
                // else if (/* 左手が攻撃モードか */)
                {
                    // 左手での投擲モード切り替え入力の通知を発行
                    _broker.Publish(PlayerEvent.Input.OnSwitchedThrow.GetEvent(actionInfo));
                }
            }

            // R入力
            else if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.R))
            {
                // if (/* UI操作中か */) return;

                var actionInfo = new PlayerActionInfo
                {
                    actHand = PlayerHand.Right
                };

                // if (/* 右手が投擲モードか */)
                {
                    _broker.Publish(PlayerEvent.Input.OnSwitchedAttack.GetEvent(actionInfo));
                }

                // else if (/* 右手が攻撃モードか */)
                {
                    _broker.Publish(PlayerEvent.Input.OnSwitchedThrow.GetEvent(actionInfo));
                }
            }

            // ZL入力
            else if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.ZL))
            {
                var actionInfo = new PlayerActionInfo
                {
                    actHand = PlayerHand.Left
                };

                OnZLZRInput(actionInfo);
            }

            // ZR入力
            else if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.ZR))
            {
                var actionInfo = new PlayerActionInfo
                {
                    actHand = PlayerHand.Right
                };

                OnZLZRInput(actionInfo);
            }

            // B入力
            else if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.B))
            {
                // if (/* UI操作中か */)
                {
                    _broker.Publish(GameEvent.UI.Input.OnBack.GetEvent());
                }

                // else if (/* 回避可能か */)
                {
                    // ここの条件めんどくさくなりそう…
                    _broker.Publish(PlayerEvent.Input.OnDodge.GetEvent());
                }

                // else if (/* ジャンプ可能か */)
                {
                    _broker.Publish(PlayerEvent.Input.OnJump.GetEvent());
                }
            }

            // X入力
            else if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.X))
            {
                // if (/* 抜刀中か */) return;

                _broker.Publish(PlayerEvent.Input.OnPulling.GetEvent());
            }

            // Y入力
            else if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.Y))
            {
                // if (/* 納刀中か */) return;

                _broker.Publish(PlayerEvent.Input.OnDelivery.GetEvent());
            }
        }

        private void OnZLZRInput(PlayerActionInfo actionInfo)
        {
            // if (/* 魔法剣を装備しているか */)
            {
                // if (/* 張り付き状態か */)
                {
                    // 剣を抜く入力イベント発行
                }

                // else if (/* 納刀中か */)
                {
                    // if (/* 投擲モードか */)
                    {
                        // 投擲入力イベント発行
                        _broker.Publish(PlayerEvent.Input.OnThrowWeapon.GetEvent(actionInfo));
                    }

                    // else if (/* 攻撃モードか */)
                    {
                        // 攻撃入力イベント発行
                        _broker.Publish(PlayerEvent.Input.OnAttackWeapon.GetEvent(actionInfo));
                    }
                }

                // else if (/* 一定の魔力量があるか */)
                {
                    // if (/* 突き刺し状態か */)
                    {
                        // 魔力爆破離脱入力イベント発行
                    }

                    // else
                    {
                        // 魔力爆破入力イベント発行
                    }
                }
            }
        }
    }
}
