using System;
using System.Collections.Generic;
using App2Night.CustomView.Page;
using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.ValueConverter;
using FreshMvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.Page
{ 
    public class DashboardPage : CustomContentPage 
    {
        #region Views
        readonly EnhancedContainer _interestingPartieContainer = new EnhancedContainer
        {
            Name = AppResources.IntParty,
            ContentMissingText = AppResources.ContIntParty
        };

        readonly EnhancedContainer _myPartiesContainer = new EnhancedContainer
        {
            Name = AppResources.MyParty,
            ContentMissingText = AppResources.ContMyParty
        };

        readonly EnhancedContainer _historyContainer = new EnhancedContainer
        {
            Name = AppResources.History,
            ContentMissingText = AppResources.ContHistory
        };

        readonly Map _headerMap = new Map()
        {
            HeightRequest = 200
        };

        readonly Image _profilePicture = new Image
        {
            HeightRequest = 100,
            WidthRequest = 100,
            Margin = new Thickness(10),
            Source = ImageSource.FromResource("App2Night.Data.IconCode.icon.png")
        };

        readonly Label _usernameLabel = new Label();
        readonly Label _emaiLabel = new Label();

        readonly HorizontalGallerieView _historyGallerieView = new HorizontalGallerieView
        {
            Columns = 2,
            Rows = 1,
            Template = typeof(QuadraticPartyTemplate)
        };

        readonly HorizontalGallerieView _interestingPartieGallerie = new HorizontalGallerieView
        {
            Columns = 1,
            Template = typeof(QuadraticPartyTemplate)
        };

        readonly HorizontalGallerieView _myPartieGallerie = new HorizontalGallerieView
        {
            Columns = 1,
            Template = typeof(QuadraticPartyTemplate)
        };
        #endregion

        public DashboardPage()
        {
            #region InterestingPartieView
            _interestingPartieContainer.Content = _interestingPartieGallerie;
            _interestingPartieGallerie.ElementTapped += PartieSelected;
            _interestingPartieGallerie.SetBinding(GallerieView.ItemSourceProperty, "InterestingPartiesForUser"); 
          _interestingPartieContainer.SetBinding(EnhancedContainer.CommandProperty, "MoveToPartyPicker");
         _interestingPartieContainer.SetBinding(  EnhancedContainer.NoContentWarnignVisibleProperty,
               "InterestingPartieAvailable", converter: new InvertBooleanConverter());
            #endregion

            #region MyParties
            _myPartiesContainer.Content = _myPartieGallerie;
            _myPartieGallerie.ElementTapped += PartieSelected;
            _myPartiesContainer.SetBinding(EnhancedContainer.CommandProperty, "MoveToMyPartiesCommand");
            _myPartieGallerie.SetBinding( GallerieView.ItemSourceProperty, "Selectedparties");
            _myPartiesContainer.SetBinding( EnhancedContainer.NoContentWarnignVisibleProperty,
                "SelectedpartiesAvailable", converter: new InvertBooleanConverter());
            #endregion

            #region History
            _historyContainer.Content = _historyGallerieView;
            _historyContainer.SetBinding(EnhancedContainer.CommandProperty, "MoveToHistoryCommand");
            _historyGallerieView.ElementTapped += PartieSelected;
            _historyGallerieView.SetBinding( GallerieView.ItemSourceProperty, "PartyHistory");
            _historyContainer.SetBinding(EnhancedContainer.NoContentWarnignVisibleProperty,
                "PartyHistoryAvailable", converter: new InvertBooleanConverter());
            #endregion
            //Main layout
            var mainLayout = new Grid()
            {
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    new MapWrapper(_headerMap),
                    {_myPartiesContainer, 0, 2},
                    {_interestingPartieContainer, 0, 3},
                    {_historyContainer, 0, 4}
                }
            };

            var mainScroll = new ScrollView
            {
                Content = mainLayout,
                Orientation = ScrollOrientation.Vertical
            };
            Content = mainScroll;
        }
        /// <summary>
        /// Opens seleted party with <see cref="PartyPreviewView"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="o"></param>
        private void PartieSelected(object sender, object o)
        {
            //PreviewItemSelected<Party, PartyPreviewView>((Party) o, new object[] {this., Height});
        }

        private int lastColumns = 1;

        /// <summary>
        /// Allocates size for <see cref="EnhancedContainer"/>. 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            //Handle big screens
            int columns = (int) Math.Ceiling((Width - 200)/300); //Available columns
            if (lastColumns != columns)
            {
                lastColumns = columns;
                _historyGallerieView.Columns = columns;
                _interestingPartieGallerie.Columns = columns;
                _myPartieGallerie.Columns = columns;
            }

            _profilePicture.HeightRequest = Width/2 - 20;
            _profilePicture.WidthRequest = Width/2 - 20;
        }
        /// <summary>
        /// Removes <see cref="EventHandler"/> of <see cref="GallerieView"/>'s
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _interestingPartieGallerie.ElementTapped -= PartieSelected;
            _myPartieGallerie.ElementTapped -= PartieSelected;
            _historyGallerieView.ElementTapped -= PartieSelected;
        }
    }
}
