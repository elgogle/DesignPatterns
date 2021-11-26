/*

## 桥梁模式的定义

将抽象和实现解耦，使得两者可以独立地变化。

桥梁模式的重点是在“解耦”上，如何让它们两者解耦是我们要了解的重点


## 桥梁模式的优点

- 抽象和实现分离

*/

using System;
using Xunit;

namespace DesignPatterns.Structural
{
    public class Bridge : DesignPattern
    {
        public override void Execute()
        {
            const string authorName = "John Doe";
            const string bookName = "Best book in the world";

            // 创建书籍描述实例，样式为普通样式，也就是原样
            Book authorAndName = new BookWithAuthorAndName(authorName, bookName, FontStyle.Normal);
            
            Assert.Equal("John Doe", authorAndName.AuthorName);
            Assert.Equal("Best book in the world", authorAndName.BookName);
            Assert.Equal("John Doe's Best book in the world", authorAndName.Title);

            // 创建书籍描述实例，样式为大写样式
            Book authorAndNameUppercase = new BookWithAuthorAndName(authorName, bookName, FontStyle.Uppercase);

            Assert.Equal("JOHN DOE", authorAndNameUppercase.AuthorName);
            Assert.Equal("BEST BOOK IN THE WORLD", authorAndNameUppercase.BookName);
            Assert.Equal("JOHN DOE's BEST BOOK IN THE WORLD", authorAndNameUppercase.Title);


            Book nameAndAuthor = new BookWithNameAndAuthor(authorName, bookName, FontStyle.Normal);

            Assert.Equal("John Doe", nameAndAuthor.AuthorName);
            Assert.Equal("Best book in the world", nameAndAuthor.BookName);
            Assert.Equal("Best book in the world by John Doe", nameAndAuthor.Title);

            Book nameAndAuthorUppercase = new BookWithNameAndAuthor(authorName, bookName, FontStyle.Uppercase);

            Assert.Equal("JOHN DOE", nameAndAuthorUppercase.AuthorName);
            Assert.Equal("BEST BOOK IN THE WORLD", nameAndAuthorUppercase.BookName);
            Assert.Equal("BEST BOOK IN THE WORLD by JOHN DOE", nameAndAuthorUppercase.Title);
        }

        #region Definition

        public enum FontStyle
        {
            Uppercase,
            Normal
        }

        /* Bridge */
        /// <summary>
        /// 抽象类（书籍），有一个抽象属性（书籍描述）
        /// </summary>
        public abstract class Book
        {
            private readonly string _authorName;
            private readonly string _bookName;
            private readonly BookStyle _bookStyle;

            /// <summary>
            /// 定义一本书籍
            /// </summary>
            /// <param name="authorName">作者</param>
            /// <param name="bookName">书名</param>
            /// <param name="titleStyle">书籍描述样式类</param>
            protected Book(string authorName, string bookName, FontStyle titleStyle)
            {
                this._authorName = authorName;
                this._bookName = bookName;
                this._bookStyle = GetBookStyle(titleStyle);
            }

            /// <summary>
            /// 书籍作者描述属性，通过描述样式类控制
            /// </summary>
            /// <returns></returns>
            public string AuthorName => this._bookStyle.StyleAuthorName(this._authorName);
            /// <summary>
            /// 书籍名称描述属性，通过描述样式类控制
            /// </summary>
            /// <returns></returns>
            public string BookName => this._bookStyle.StyleBookName(this._bookName);

            /// <summary>
            /// 书籍描述，抽象属性
            /// </summary>
            /// <value></value>
            public abstract string Title { get; }

            /// <summary>
            /// 根据描述样式类型，返回具体的描述样式实例
            /// </summary>
            /// <param name="fontStyle"></param>
            /// <returns></returns>
            private static BookStyle GetBookStyle(FontStyle fontStyle)
            {
                switch (fontStyle)
                {
                    case FontStyle.Uppercase:
                        return new BookStyleUppercase();
                    case FontStyle.Normal:
                        return new BookStyleNormal();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(fontStyle), fontStyle, null);
                }
            }
        }

        /* Bridge Implementation */
        /// <summary>
        /// 书籍样式类，抽象类
        /// </summary>
        public abstract class BookStyle
        {
            /// <summary>
            /// 返回书籍作者样式描述，抽象方法
            /// </summary>
            /// <param name="authorName">作者</param>
            /// <returns></returns>
            public abstract string StyleAuthorName(string authorName);
            /// <summary>
            /// 返回书籍名称样式描述，抽象方法
            /// </summary>
            /// <param name="bookName">书籍名称</param>
            /// <returns></returns>
            public abstract string StyleBookName(string bookName);
        }

        #endregion

        #region Concrete Implementation

        /// <summary>
        /// 继承书籍类（抽象类），需要实现：书籍描述，抽象属性
        /// </summary>
        public class BookWithAuthorAndName : Book
        {
            public BookWithAuthorAndName(string authorName, string bookName, FontStyle titleStyle)
                : base(authorName, bookName, titleStyle)
            {
            }

            /// <summary>
            /// 实现抽象属性
            /// </summary>
            /// <value></value>
            public override string Title => $"{this.AuthorName}'s {this.BookName}";
        }

        /// <summary>
        /// 继承书籍类（抽象类），需要实现：书籍描述抽象属性
        /// </summary>
        public class BookWithNameAndAuthor : Book
        {
            public BookWithNameAndAuthor(string authorName, string bookName, FontStyle titleStyle)
                : base(authorName, bookName, titleStyle)
            {
            }

            /// <summary>
            /// 实现抽象属性
            /// </summary>
            /// <value></value>
            public override string Title => $"{this.BookName} by {this.AuthorName}";
        }

        /// <summary>
        /// 继承书籍样式类（抽象类），需要实现：书籍作者样式描述、书籍名称样式描述 两个抽象方法
        /// </summary>
        public class BookStyleUppercase : BookStyle
        {
            /// <summary>
            /// 实现抽象方法，返回大写样式
            /// </summary>
            /// <param name="authorName"></param>
            /// <returns></returns>
            public override string StyleAuthorName(string authorName) => authorName?.ToUpper();

            /// <summary>
            /// 实现抽象方法，返回大写样式
            /// </summary>
            /// <param name="bookName"></param>
            /// <returns></returns>
            public override string StyleBookName(string bookName) => bookName?.ToUpper();
        }

        /// <summary>
        /// 继承书籍样式类（抽象类），需要实现：书籍作者样式描述、书籍名称样式描述 两个抽象方法
        /// </summary>
        public class BookStyleNormal : BookStyle
        {
            /// <summary>
            /// 实现抽象方法，返回原样
            /// </summary>
            /// <param name="authorName"></param>
            /// <returns></returns>
            public override string StyleAuthorName(string authorName) => authorName;

            /// <summary>
            /// 实现抽象方法，返回原样
            /// </summary>
            /// <param name="bookName"></param>
            /// <returns></returns>
            public override string StyleBookName(string bookName) => bookName;
        }
        #endregion
    }
}
