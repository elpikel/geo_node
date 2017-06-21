using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TagLife.Models.Api;
using TagLife.Services;

namespace TagLife.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public async Task ShouldGetPlaces()
        {
            var placesService = new ApiService();
            var places = await placesService.GetPlaces();

            Assert.That(places, Is.Not.Empty);
        }

        [Test]
        public async Task ShouldGetNotes()
        {
            var placesService = new ApiService();
            var notes = await placesService.GetNotes();

            Assert.That(notes, Is.Not.Empty);
        }

        [Test]
        public async Task ShouldSendNote()
        {
            var placesService = new ApiService();
            await placesService.SendNote(new InputNote()
            {
                Description = "ssfsdfsddf",
                Latitude = 1.0f,
                Longitude = 2.0f,
                Username = "sdfsdsdsdf"
            });
        }
    }
}
