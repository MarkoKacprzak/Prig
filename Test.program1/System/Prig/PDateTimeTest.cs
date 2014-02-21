﻿/* 
 * File: PDateTimeTest.cs
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
using System;
using System.Collections.Generic;
using System.Prig;
using System.Threading;
using Urasandesu.Prig.Framework;

namespace Test.program1.System.Prig
{
    [TestFixture]
    public class PDateTimeTest
    {
        [Test]
        public void get_Now_ShouldBeCallableIndirectly()
        {
            using (new IndirectionsContext())
            {
                // Arrange
                PDateTime.NowGet.Body = () => new DateTime(2014, 02, 14, 11, 30, 55, 00);

                // Act
                var actual = DateTime.Now;

                // Assert
                Assert.AreEqual(new DateTime(2014, 02, 14, 11, 30, 55, 00), actual);
            }
        }

        [Test]
        public void get_Now_ShouldBeCallableOriginally_AnyTime()
        {
            using (new IndirectionsContext())
            {
                // Arrange
                var count = 0;
                PDateTime.NowGet.Body = () =>
                {
                    if (5 <= ++count)
                        return new DateTime(2013, 12, 23, 11, 22, 33, 44);
                    else
                        return IndirectionsContext.ExecuteOriginal(() => DateTime.Now);
                };

                // Act
                var actuals = new List<DateTime>();
                actuals.Add(DateTime.Now);
                actuals.Add(DateTime.Now);
                actuals.Add(DateTime.Now);
                actuals.Add(DateTime.Now);
                actuals.Add(DateTime.Now);
                actuals.Add(DateTime.Now);

                // Assert
                var indirectValue = new DateTime(2013, 12, 23, 11, 22, 33, 44);
                Assert.AreNotEqual(indirectValue, actuals[0]);
                Assert.AreNotEqual(indirectValue, actuals[1]);
                Assert.AreNotEqual(indirectValue, actuals[2]);
                Assert.AreNotEqual(indirectValue, actuals[3]);
                Assert.AreEqual(indirectValue, actuals[4]);
                Assert.AreEqual(indirectValue, actuals[5]);
            }
        }

        [Test]
        public void FromBinary_ShouldBeCallableIndirectly()
        {
            using (new IndirectionsContext())
            {
                // Arrange
                PDateTime.FromBinary.Body = _ => new DateTime(2014, 12, 23, 01, 02, 03, 00);

                // Act
                var dateData = DateTime.Now.ToBinary();
                var actual = DateTime.FromBinary(dateData);
                
                // Assert
                Assert.AreEqual(new DateTime(2014, 12, 23, 01, 02, 03, 00), actual);
            }
        }
    }
}
