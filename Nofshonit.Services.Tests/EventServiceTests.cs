using Nofshonit.Common.Interfaces;
using Nofshonit.Common;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.Club;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Nofshonit.Infrastructure;
using Nofshonit.Services.Base;
using Nofshonit.Services.Tests.Data;

namespace Nofshonit.Services.Tests
{
    public class EventServiceTests
    {
        private Event.EventService CreateDefaultEventService()
        {
            return new Event.EventService();
        }

        [Fact]
        public void EventsCatalogTest()
        {
            // Arrange
            var eventService = CreateDefaultEventService();

            // Act
            var result = eventService.EventCatalog(11778, 43141);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(227)]
        public void GetEventsByCategoryIdTest(long categoryId)
        {
            // Arrange
            var eventService = CreateDefaultEventService();

            // Act
            var result = eventService.GetEventsByCategoryId(categoryId);

            // Assert
            Assert.NotNull(result);            
        }

        [Theory]
        [InlineData(42733)]
        public void GetEventsByCategoryIdQtyTest(long categoryId)
        {
            // Arrange
            var eventService = CreateDefaultEventService();
            var data = EventData.GetEventsByCategoryIdQtyTest_Data();
            var count = 0;
            data.TryGetValue(categoryId, out count);

            // Act
            var result = eventService.GetEventsByCategoryId(categoryId);

            // Assert
            Assert.Equal(count, result.Count);
        }

    }
}
