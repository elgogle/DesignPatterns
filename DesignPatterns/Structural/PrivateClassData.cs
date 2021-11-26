/*

## 访问者模式的定义

封装一些作用于某种数据结构中的各元素的操作，它可以在不改变数据结构的前提下定义作用于这些元素的新的操作。
通过限制访问器/修改器的可见性来限制它们的暴露。

*/

using System;
using Xunit;

namespace DesignPatterns.Structural
{
    public class PrivateClassData : DesignPattern
    {
        public override void Execute()
        {
            const double radius = 4;

            var circle = new Circle(radius);
            Assert.Equal(radius * 2, circle.Diameter);
            Assert.Equal(radius * 2 * Math.PI, circle.Circumference);
        }

        public class Circle
        {
            public Circle(double radius)
            {
                this.Data = new CircleData(radius);
            }

            private CircleData Data { get; }

            public double Circumference => this.Diameter * Math.PI;

            public double Diameter => this.Data.Radius * 2;
        }

        /// <summary>
        /// Private class data
        /// </summary>
        public class CircleData
        {
            public CircleData(double radius)
            {
                this.Radius = radius;
            }

            public double Radius { get; }
        }
    }
}
