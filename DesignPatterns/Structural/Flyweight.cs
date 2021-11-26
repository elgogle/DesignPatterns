﻿/*

## 享元模式的定义

使用共享对象可有效地支持大量的细粒度的对象。
享元模式（Flyweight Pattern）是池技术的重要实现方式


*/

using System;
using System.Collections.Generic;
using Xunit;

namespace DesignPatterns.Structural
{
    public class Flyweight : DesignPattern
    {
        public override void Execute()
        {
            var aliasGenerator = new AliasGenerator();

            Alias firstAlias = aliasGenerator.GetAlias("John Doe");
            Assert.NotNull(firstAlias);

            Alias sameAlias = aliasGenerator.GetAlias("John Doe");
            Assert.Equal(firstAlias, sameAlias);

            Alias anotherAlias = aliasGenerator.GetAlias("Jane Doe");
            Assert.NotEqual(firstAlias, anotherAlias);
        }

        
        /// <summary>
        /// 享元工厂
        /// </summary>
        public class AliasGenerator
        {
            private readonly Dictionary<string, Alias> _aliases;

            public AliasGenerator()
            {
                this._aliases = new Dictionary<string, Alias>();
            }

            public Alias GetAlias(string name)
            {
                if (!this._aliases.TryGetValue(name, out Alias alias))
                {
                    this._aliases.Add(name, alias = new Alias(Guid.NewGuid()));
                }
                return alias;
            }
        }

        /// <summary>
        /// 享元角色
        /// </summary>
        public class Alias : IEquatable<Alias>
        {
            public Alias(Guid id)
            {
                this.Id = id;
            }

            private Guid Id { get; }

            public bool Equals(Alias other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return this.Id.Equals(other.Id);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != this.GetType())
                {
                    return false;
                }

                return this.Equals((Alias) obj);
            }

            public override int GetHashCode()
            {
                return this.Id.GetHashCode();
            }
        }
    }
}
