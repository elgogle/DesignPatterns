/*


## 适配器模式的定义

将一个类的接口变换成客户端所期待的另一种接口，从而使原本因接口不匹配而无法在一起工作的两个类能够在一起工作。

## 适配器模式的优点

- 适配器模式可以让两个没有任何关系的类在一起运行，只要适配器这个角色能够搞定他们就成。

*/

using System;
using Xunit;

namespace DesignPatterns.Structural
{
    /// <summary>
    /// 适配器转换后输出的类型
    /// </summary>
    using MAmp = Int32;
    /// <summary>
    /// 适配器输入的类型
    /// </summary>
    using Amp = Decimal;

    /// <summary>
    /// 手机的充电器比作 Adapter (适配器)
    /// <para> 适配器需要连接交流电源转换后通过 USB 接口输出</para>
    /// <para> 手机通过 USB 接口连接到适配器充电</para>
    /// </summary>
    public class Adapter : DesignPattern
    {
        public override void Execute()
        {
            // 限制最大电流
            MAmp maxUsbPower = 200;

            // 新建手机，并设置了最大电流
            var phone = new Phone(maxMAmps: maxUsbPower);
            // 断言此时手机没有在充电
            Assert.False(phone.IsCharging);
            // 新建一个交流电插口
            var outlet = new AcOutlet(amps: 15);
            // 新建一个 Usb 适配器，连接到上面的交流电插口，且该适配器设置了最大输出电流
            var adapter = new UsbAdapter(socket: outlet, maxMAmps: maxUsbPower);
            // 手机连接到上面的 Usb 适配器
            phone.ConnectTo(adapter);
            // 断言此时手机在充电
            Assert.True(phone.IsCharging);
            // 新建一个坏的 Usb 适配器，并连接到上面的交流电接口，该适配器没有限制输出最大电流
            var badAdapter = new BadUsbAdapter(socket: outlet);
            // 手机连接到上面这个 Usb 适配器，断言连接异常
            Assert.Throws<OverflowException>(() => phone.ConnectTo(badAdapter));
        }

        #region Definition

        public class Phone
        {
            private readonly MAmp _maxMAmps;
            private MAmp _mAmps;

            /// <summary>
            /// 手机设置最大电流
            /// </summary>
            /// <param name="maxMAmps"></param>
            public Phone(MAmp maxMAmps)
            {
                this._maxMAmps = maxMAmps;
            }

            /// <summary>
            /// 是否在充电
            /// </summary>
            public bool IsCharging => this._mAmps > 0;

            /// <summary>
            /// 连接 USB 
            /// </summary>
            /// <param name="socket"></param>
            public void ConnectTo(IUsbSocket socket)
            {
                MAmp mAmps = socket.GetPower();
                if (mAmps > this._maxMAmps)
                {
                    throw new OverflowException();
                }
                this._mAmps = mAmps;
            }
        }

        /// <summary>
        /// USB 接口
        /// </summary>
        public interface IUsbSocket
        {
            /// <summary>
            /// 获取 USB 电流
            /// </summary>
            /// <returns></returns>
            MAmp GetPower();
        }

        /// <summary>
        /// 交流电接口
        /// </summary>
        public interface IAcSocket
        {
            /// <summary>
            /// 获取交流电电流
            /// </summary>
            /// <returns></returns>
            Amp GetPower();
        }

        #endregion

        #region Concrete Implementation

        /// <summary>
        /// 交流电接口实例
        /// </summary>
        public class AcOutlet : IAcSocket
        {
            private readonly Amp _amps;

            /// <summary>
            /// 交流电定义
            /// </summary>
            /// <param name="amps">输出电流</param>
            public AcOutlet(Amp amps)
            {
                this._amps = amps;
            }

            public Amp GetPower()
            {
                return this._amps;
            }
        }

        /// <summary>
        /// USB 适配器
        /// </summary>
        public class UsbAdapter : IUsbSocket
        {
            private readonly IAcSocket _socket;
            private readonly MAmp _maxMAmps;

            /// <summary>
            /// USB 定义
            /// </summary>
            /// <param name="socket">交流电接口</param>
            /// <param name="maxMAmps">限制输出的最大电流</param>
            public UsbAdapter(IAcSocket socket, MAmp maxMAmps)
            {
                this._socket = socket;
                this._maxMAmps = maxMAmps;
            }

            public MAmp GetPower()
            {
                Amp amps = this._socket.GetPower();
                var mAmps = (MAmp)(amps * 100);
                return Math.Min(this._maxMAmps, mAmps);
            }
        }

        /// <summary>
        /// 坏的 USB 适配器
        /// </summary>
        public class BadUsbAdapter : IUsbSocket
        {
            private readonly IAcSocket _socket;

            /// <summary>
            /// 定义，没有限制输出的最大电流
            /// </summary>
            /// <param name="socket">交流电接口</param>
            public BadUsbAdapter(IAcSocket socket)
            {
                this._socket = socket;
            }

            public MAmp GetPower()
            {
                Amp amps = this._socket.GetPower();
                return (MAmp)(amps * 100);
            }
        }
        #endregion
    }
}
