using System.Collections.Generic;
using Xunit;
using zzzKatas.GildedRose;

namespace zzzKatas.Tests
{
    public class GildedRoseTests
    {
        /*
         * 
                                                      new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
                                                      new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
                                                      new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
                                                      new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                                                      new Item
                                                          {
                                                              Name = "Backstage passes to a TAFKAL80ETC concert",
                                                              SellIn = 15,
                                                              Quality = 20
                                                          },
                                                      new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
         */
        private readonly List<Item> _items = new()
        {
            new Item("+5 Dexterity Vest") { SellIn = 10, Quality = 20 },
            new Item("Aged Brie") { SellIn = 2, Quality = 0 },
            new Item("Elixir of the Mongoose") { SellIn = 5, Quality = 7 },
            new Item("Sulfuras, Hand of Ragnaros") { SellIn = 0, Quality = 80 },
            new Item("Backstage passes to a TAFKAL80ETC concert") { SellIn = 15, Quality = 20 },
            new Item("Conjured Mana Cake") { SellIn = 3, Quality = 6 }
        };

        #region Standard Items

        // This should probably be two tests
        [Fact]
        public void StandardItem()
        {
            Item item = new("item") { SellIn = 10, Quality = 20 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);

            Assert.Equal(9, item.SellIn);    // Sell-In decreases by 1
            Assert.Equal(19, item.Quality);  // Quality decreases by 1
        }

        [Fact]
        public void QualityNotNegative()
        {
            Item item = new("item") { SellIn = 10, Quality = 0 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);

            Assert.True(item.Quality >= 0); // Doesn't drop below zero
        }

        [Fact]
        public void QualityDegradationDoubledPostSellIn()
        {
            Item item = new("item") { SellIn = 0, Quality = 5 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);

            Assert.Equal(3, item.Quality); // Drops by two
        }

        #endregion

        #region Aged Brie

        [Fact]
        public void AgedBrieQualityIncreases()
        {
            Item item = new("Aged Brie") { SellIn = 5, Quality = 5 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);

            Assert.Equal(6, item.Quality); // Increases by one
        }

        [Fact]
        public void QualityNeverMoreThan50_Initially50()
        {
            Item item = new("Aged Brie") { SellIn = 5, Quality = 50 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);

            Assert.Equal(50, item.Quality); // Remains at 50
        }

        // This fails with the original UpdateQuality()
        [Fact]
        public void QualityNeverMoreThan50_InitiallyOver50()
        {
            Item item = new("Aged Brie")
            {
                SellIn = 5,
                Quality = 55
            };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);

            Assert.Equal(50, item.Quality); // "The Quality of an item is never more than 50."
        }

        #endregion

        #region Backstage passes to a TAFKAL80ETC concert

        // Test initially fails!
        [Fact]
        public void ArenaTickets_QualityIncreases()
        {
            Item item = new("Backstage passes to a TAFKAL80ETC concert") { SellIn = 15, Quality = 5 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);

            Assert.Equal(6, item.Quality); // Increases by one
        }

        // Test initially fails!
        [Fact]
        public void ArenaTickets_QualityIncreasesBy2_10DaysOrLess()
        {
            Item item = new("Backstage passes to a TAFKAL80ETC concert") { SellIn = 10, Quality = 5 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);
            // SellIn <= 10
            Assert.Equal(7, item.Quality); // Increases by two
        }

        // Test initially fails!
        [Fact]
        public void ArenaTickets_QualityIncreasesBy3_5DaysOrLess()
        {
            Item item = new("Backstage passes to a TAFKAL80ETC concert") { SellIn = 5, Quality = 5 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);
            // SellIn <= 5
            Assert.Equal(8, item.Quality); // Increases by three
        }

        [Fact]
        public void ArenaTickets_QualityIsZero_AfterConcert()
        {
            Item item = new("Backstage passes to a TAFKAL80ETC concert") { SellIn = -1, Quality = 5 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);
            // SellIn < 0
            Assert.Equal(0, item.Quality); // Drops to zero
        }

        #endregion

        #region Conjured Mana Cake

        // New item type with special rules

        [Fact]
        public void ConjuredItem_DegradesTwiceAsFast_PreSellIn()
        {
            Item item = new("Conjured Mana Cake") { SellIn = 5, Quality = 5 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);

            Assert.Equal(3, item.Quality); // Decreases by two
        }

        [Fact]
        public void ConjuredItem_DegradesTwiceAsFast_PostSellIn()
        {
            Item item = new("Conjured Mana Cake") { SellIn = -1, Quality = 5 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);

            Assert.Equal(1, item.Quality); // Decreases by four
        }

        #endregion

        #region Sulfuras, Hand of Ragnaros

        // Sulfuras, Hand of Ragnaros is a special item that does not change on UpdateQuality()

        [Fact]
        public void DragonScaleQualityNeverDecreases()
        {
            Item item = new("Sulfuras, Hand of Ragnaros") { SellIn = 5, Quality = 5 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);

            Assert.Equal(5, item.Quality); // Remains unchanged
        }

        [Fact]
        public void DragonScaleSellInNeverDecreases()
        {
            Item item = new("Sulfuras, Hand of Ragnaros") { SellIn = 5, Quality = 5 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);

            Assert.Equal(5, item.SellIn); // Remains unchanged
        }

        [Fact]
        public void DragonScale_Over50_NoChange()
        {
            Item item = new("Sulfuras, Hand of Ragnaros") { SellIn = 0, Quality = 80 };
            _items.Add(item);

            ItemUtilities.UpdateQuality(_items);

            Assert.Equal(0, item.SellIn);    // Remains unchanged
            Assert.Equal(80, item.Quality);  // Remains unchanged
        }

        #endregion
    }
}