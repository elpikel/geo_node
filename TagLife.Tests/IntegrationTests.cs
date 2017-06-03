using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TagLife.Services;

namespace TagLife.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public async Task ShouldConnectToApi()
        {
            var placesService = new PlacesService();
            var places = await placesService.GetPlaces();

            Assert.That(places, Is.Not.Empty);
        }
    }
}
