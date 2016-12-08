using App2Night.Model.Model;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class RateView : PreviewView
    {
        #region Views
        InputContainer<Label> _generalRatingLabel = new InputContainer<Label>
        {
            IconCode = "\uf007",
            Input = { Text = "General Rating" }
        };
        InputContainer<Label> _priceRatingLabel = new InputContainer<Label>
        {
            IconCode = "\uf155",
            Input = { Text = "Price Rating" }
        };
        InputContainer<Label> _locationRatingLabel = new InputContainer<Label>
        {
            IconCode = "\uf041",
            Input = { Text = "Location Rating" }
        };
        InputContainer<Label> _moodRatingLabel = new InputContainer<Label>
        {
            IconCode = "\uf0a1",
            Input = { Text = "Mood Rating"}
        };
        
        #endregion
        public RateView(Party party) : base(party.Name, party)
        {
            
        }
    }
}