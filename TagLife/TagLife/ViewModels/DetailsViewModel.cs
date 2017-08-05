using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Geolocator;
using PropertyChanged;
using TagLife.Controls;
using TagLife.Models.Api;
using TagLife.Services;
using Xamarin.Forms;

namespace TagLife.ViewModels
{
    [ImplementPropertyChanged]
    public class DetailsViewModel
    {
        private readonly CustomPin _pin;
        public string MainText { get; set; }

        public ImmutableList<string> Comments { get; set; }

        public string Comment { get; set; }

        public DetailsViewModel(CustomPin pin)
        {
            _pin = pin;

            MainText = _pin.Text;
        }

        public async Task LoadData()
        {
            await UpdateNotes();
        }

        private async Task UpdateNotes()
        {
            var notes = await new ApiService().GetNotes();

            // todo: get this from call, not using where
            var matchingNotes = notes.Where(n => n.PlaceId == _pin.Id);

            Comments = matchingNotes.Select(mn => mn.Description).ToImmutableList();
        }

        public ICommand AddComment
        {
            get
            {
                return new Command(async () =>
                {
                    await Task.Delay(0);

                    if (Comment.IsNullOrWhitespace())
                    {
                        return;
                    }

                    await new ApiService().SendNote(new InputNoteWithPosition()
                    {
                        Description = Comment,
                        Place = Convert.ToInt32(_pin.Id),
                        Username = Guid.NewGuid().ToString()
                    });

                    Comment = "";

                    await UpdateNotes();
                });
            }
        }
    }
}