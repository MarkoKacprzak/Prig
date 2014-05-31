﻿/* 
 * File: VillageTest.cs
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
using program1.MyLibrary;
using System;
using System.Collections.Generic;
using System.Collections.Generic.Prig;
using System.Linq;
using System.Prig;
using System.Threading;
using Urasandesu.Prig.Framework;

namespace Test.program1.MyLibraryTest
{
    [TestFixture]
    public class VillageTest
    {
        [Test]
        public void Constructor_shall_not_call_within_short_timeframe_to_generate_unique_information()
        {
            using (new IndirectionsContext())
            {
                // Arrange
                var seeds = new HashSet<int>();
                PRandom.Constructor.Body = (@this, seed) =>
                {
                    IndirectionsContext.ExecuteOriginal(() =>
                    {
                        var ctor = typeof(Random).GetConstructor(new[] { typeof(int) });
                        ctor.Invoke(@this, new object[] { seed });
                    });
                    seeds.Add(seed);
                };
                new Random();  // preparing JIT
                seeds.Clear();


                // Act
                var vil1 = new Village();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                var vil2 = new Village();

                
                // Assert
                Assert.AreEqual(2, seeds.Count);
            }
        }



        [Test]
        public void GetShortestRoute_should_consider_routes_in_order_from_small_distance()
        {
            using (new IndirectionsContext())
            {
                // Arrange
                var slot = 0;
                var numAndDistances = new[] { 4, 2, 4, 3, 1, 6, 7 };
                PRandom.Next_int.Body = (@this, maxValue) => numAndDistances[slot++];

                var vil = new Village();

                var considerations = new List<RicePaddy>();
                PList<RicePaddy>.Add.Body = (@this, item) => 
                {
                    IndirectionsContext.ExecuteOriginal(() =>
                    {
                        considerations.Add(item);
                        @this.Add(item);
                    });
                };


                // Act
                var result = vil.GetShortestRoute(vil.RicePaddies.ElementAt(2), vil.RicePaddies.ElementAt(0));

                
                // Assert
                Assert.AreEqual(3, result.TotalDistance);
                Assert.AreEqual(4, considerations.Count);
                Assert.AreEqual(2, considerations[0].Identifier);
                Assert.AreEqual(1, considerations[1].Identifier);
                Assert.AreEqual(0, considerations[2].Identifier);
                Assert.AreEqual(3, considerations[3].Identifier);
            }
        }
    }
}
