﻿/* 
 * File: PArrayTest.cs
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
using System.Prig;
using Urasandesu.NAnonym.Collections.Generic;
using Urasandesu.Prig.Framework;

namespace Test.program1.System.Prig
{
    [TestFixture]
    public class PArrayTest
    {
        [Test]
        public void CreateInstance_ShouldBeCallableIndirectly()
        {
            using (new IndirectionsContext())
            {
                // Arrange
                PArray.CreateInstance.Body = (elementType, lengths, lowerBounds) => new int[5, 5];

                // Act
                var actual = (int[,])Array.CreateInstance(typeof(int), new int[] { 3, 3 }, new int[] { 0, 0 });

                // Assert
                Assert.AreEqual(5, actual.GetLength(0));
                Assert.AreEqual(5, actual.GetLength(1));
            }
        }

        [Test]
        public void Exists_T_ShouldBeCallableIndirectly()
        {
            using (new IndirectionsContext())
            {
                // Arrange
                PArray.Exists<int>.Body = (array, match) => true;

                // Act
                var actual = Array.Exists(new int[] { 1, 2, 3 }, x => x == 42);

                // Assert
                Assert.IsTrue(actual);
            }
        }

        [Test]
        public void BinarySearch_ShouldBeCallableIndirectly()
        {
            using (new IndirectionsContext())
            {
                // Arrange
                PArray.BinarySearch.Body = (array, index, length, value, comparer) => 42;

                // Act
                var actual = Array.BinarySearch(new int[] { 1, 2, 3 }, 0, 3, (object)2, new LambdaComparer<int>((_1, _2) => _1 - _2));

                // Assert
                Assert.AreEqual(42, actual);
            }
        }
    }
}
