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
            await placesService.SendNote(new InputNoteWithLocation
            {
                Description = "ssfsdfsddf",
                Latitude = 1.0f,
                Longitude = 2.0f,
                Username = "sdfsdsdsdf"
            });
        }

        [Test]
        public async Task ShouldAddNotesToPointAndGetThem()
        {

            var placesService = new ApiService();

            var places = await placesService.GetPlaces();
            var before = places.Count;

            var notes = await placesService.GetNotes();
            var notesBefore = notes.Count;

            await placesService.SendNote(new InputNoteWithPosition()
            {
                Description = "11234",
                Place = 2,
                Username = "sdfsdsdsdf"
            });

            await placesService.SendNote(new InputNoteWithPosition
            {
                Description = "11334",
                Place = 2,
                Username = "sdfsdsdsdf"
            });

            var placesAfter = await placesService.GetPlaces();
            var after = placesAfter.Count;

            var notesAfter = await placesService.GetNotes();
            var notesAfterA = notesAfter.Count;

            Assert.That(placesAfter.Any(p => p.Description == "11234"));
            Assert.That(placesAfter.Any(p => p.Description == "11334"));

            Assert.That(notesAfterA, Is.EqualTo(notesBefore +2));
            Assert.That(after, Is.EqualTo(before));
        }
    }
}
