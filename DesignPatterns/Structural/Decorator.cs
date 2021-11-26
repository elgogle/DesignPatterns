/*

## 装饰模式的定义

动态地给一个对象添加一些额外的职责。就增加功能来说，装饰模式相比生成子类更为灵活。


*/
using Xunit;

namespace DesignPatterns.Structural
{
    public class Decorator : DesignPattern
    {
        public override void Execute()
        {
            IMessage defaultMessage = new Message("Hello World!");

            Assert.Equal("Hello World!", defaultMessage.GetMessage());

            IMessage messageWithDecoration = new MessageWithDecoration(defaultMessage);
            Assert.Equal("|!=* Hello World! *=!|", messageWithDecoration.GetMessage());
        }

        #region Definition

        public interface IMessage
        {
            string GetMessage();
        }

        #endregion

        #region Concrete Implementation

        public class Message : IMessage
        {
            private readonly string _message;

            public Message(string message)
            {
                this._message = message;
            }

            public string GetMessage() => this._message;
        }

        /* Decorator */
        /// <summary>
        /// 装饰器
        /// </summary>
        public class MessageWithDecoration : IMessage
        {
            private readonly IMessage _message;

            public MessageWithDecoration(IMessage message)
            {
                this._message = message;
            }

            /// <summary>
            /// 通过装饰后
            /// </summary>
            /// <returns></returns>
            public string GetMessage() => $"|!=* {this._message.GetMessage()} *=!|";
        }

        #endregion
    }
}
