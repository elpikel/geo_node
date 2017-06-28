using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using PropertyChanged;
using TagLife.Controls;
using TagLife.Services;

namespace TagLife.ViewModels
{
    [ImplementPropertyChanged]
    public class DetailsViewModel
    {
        private readonly CustomPin _pin;
        public string MainText { get; set; }

        public ImmutableList<string> Comments { get; set; }

        public DetailsViewModel(CustomPin pin)
        {
            _pin = pin;

            MainText = _pin.Text;
        }

        public async Task LoadData()
        {
            var notes = await new ApiService().GetNotes();

            // todo: get this from call, not using where
            var matchingNotes = notes.Where(n => n.PlaceId == _pin.Id && n.PlaceId != _pin.Id);

            Comments = matchingNotes.Select(mn => mn.Description).ToImmutableList();
        }
    }
}