﻿/* 
 * File: PStringBuilderTest.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2014 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */


using NUnit.Framework;
using System.Prig;
using System.Text;
using System.Text.Prig;
using Urasandesu.Prig.Framework;

namespace Test.program1.System.Prig
{
    [TestFixture]
    public class PStringBuilderTest
    {
        [Test]
        public void Insert_should_be_callable_indirectly()
        {
            using (new IndirectionsContext())
            {
                // Arrange
                PStringBuilder.Insert.Body = (_, index, value, count) => { return new StringBuilder("にゃんぱすー"); };

                // Act
                var sb = new StringBuilder();
                sb.Append("abd");
                var actual = sb.Insert(2, "c", 3);

                // Assert
                Assert.AreEqual("にゃんぱすー", actual.ToString());
            }
        }
        
        [Test]
        public void Replace_should_be_callable_indirectly()
        {
            using (new IndirectionsContext())
            {
                // Arrange
                PStringBuilder.Replace.Body = (_, oldChar, newChar, startIndex, count) => { return new StringBuilder("おはもに！"); };

                // Act
                var sb = new StringBuilder("aaaa");
                var actual = sb.Replace('a', 'b', 1, 2);

                // Assert
                Assert.AreEqual("おはもに！", actual.ToString());
            }
        }
    }
}
