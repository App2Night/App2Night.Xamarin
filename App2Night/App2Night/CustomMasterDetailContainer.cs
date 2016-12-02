using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using App2Night.CustomView.Template;
using App2Night.PageModel;
using App2Night.PageModel.SubPages;
using App2Night.Service.Interface;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night
{
    public sealed class CustomMasterDetailContainer : MasterDetailPage, IFreshNavigationService
    {
        public class MenuCellData
        {
            public string Title { get; set; }
            public string IconCode { get; set; }

            public bool RequiresLogin { get; set; }
        }

        public IStorageService _storageService;
        private MenuCellData _currentPageData;

        List<Xamarin.Forms.Page> _pagesInner = new List<Xamarin.Forms.Page>();
        Dictionary<MenuCellData, Xamarin.Forms.Page> _pages = new Dictionary<MenuCellData, Xamarin.Forms.Page>();
        Page.NavigationPage _menuPage;
        ObservableCollection<string> _pageNames = new ObservableCollection<string>();
        private ListView ListView => _menuPage.MenuListView;

        public Dictionary<MenuCellData, Xamarin.Forms.Page> Pages { get { return _pages; } }
        private ObservableCollection<string> PageNames { get { return _pageNames; } }

        public CustomMasterDetailContainer() : this(Constants.DefaultNavigationServiceName)
        {
            _storageService = FreshIOC.Container.Resolve<IStorageService>();
        }

        public CustomMasterDetailContainer(string navigationServiceName)
        {

            NavigationServiceName = navigationServiceName;
            RegisterNavigation();
        }

        public void Init(string menuTitle, string menuIcon = null)
        {
            CreateMenuPage(menuTitle, menuIcon);
            RegisterNavigation();
        }

        private void RegisterNavigation()
        {
            FreshIOC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);
        }

        public void AddPage<T>(string title, string iconCode, object data = null, bool requiresLogin = false) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T>(data);
            page.Title = title;
            page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
            _pagesInner.Add(page);
            var navigationContainer = CreateContainerPage(page);

            var menuData = new MenuCellData
            {
                IconCode = iconCode,
                Title = title,
                RequiresLogin = requiresLogin
            };

            _pages.Add(menuData, navigationContainer);
            _pageNames.Add(title);
            if (_pages.Count == 1)
            {
                _currentPageData = menuData;
                Detail = navigationContainer;
                ListView.SelectedItem = menuData;
            }
        }

        internal Xamarin.Forms.Page CreateContainerPageSafe(Xamarin.Forms.Page page)
        {
            if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
                return page;

            return CreateContainerPage(page);
        }

        private Xamarin.Forms.Page CreateContainerPage(Xamarin.Forms.Page page)
        {
            return new NavigationPage(page);
        }

        private void CreateMenuPage(string menuPageTitle, string menuIcon = null)
        {
            _menuPage = (Page.NavigationPage)FreshPageModelResolver.ResolvePageModel<NavigationViewModel>();

            ListView.ItemsSource = _pages.Keys;
            ListView.ItemTemplate = new DataTemplate(typeof(MenuTemplate));

            ListView.ItemSelected += SelectedItemChanged;


            //_menuPage = new Page.SubPages.NavigationPage(_listView)
            //{
            //    BindingContext = FreshIOC.Container.Resolve<NavigationViewModel>(),
            //    Title = menuPageTitle
            //};
            var navPage = _menuPage;

            if (!string.IsNullOrEmpty(menuIcon))
                navPage.Icon = menuIcon;

            Master = navPage;
        }


        private async void SelectedItemChanged(object sender, SelectedItemChangedEventArgs selectedItemChangedEventArgs)
        { 
            var selectedItem = (MenuCellData)selectedItemChangedEventArgs.SelectedItem;

            if (selectedItem != _currentPageData)
            {
                var isLoggedIn = _storageService.IsLogIn;
                if (selectedItem.RequiresLogin && !isLoggedIn)
                {
                    //Prompt with login page.
                    var loginPage = FreshPageModelResolver.ResolvePageModel<LoginViewModel>();
                    await PushPage(loginPage, null, true);
                    //TODO fast forward user to the desired page if the login succeeds

                    //Set the selected item back to the current page.
                    ListView.SelectedItem = _currentPageData;
                }
                else
                {   
                    Detail = _pages[selectedItem];
                    _currentPageData = selectedItem;
                }
            } 

            IsPresented = false;
        }

        public Task PushPage(Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return Detail.Navigation.PushModalAsync(page);

            KeyValuePair<MenuCellData, Xamarin.Forms.Page>  innerPage = _pages.FirstOrDefault(p => ((NavigationPage)p.Value).CurrentPage.GetType() == page.GetType());
            if (innerPage.Key != null)
            {
                return Task.FromResult(ListView.SelectedItem = innerPage.Key);
            }  
            return (Detail as NavigationPage).PushAsync(page, animate);    
         }

        public Task PopPage(bool modal = false, bool animate = true)
        {
            if (modal)
                return Navigation.PopModalAsync(animate);
            return (Detail as NavigationPage).PopAsync(animate); //TODO: make this better            
        }

        public Task PopToRoot(bool animate = true)
        {
            return (Detail as NavigationPage).PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; private set; }

        public void NotifyChildrenPageWasPopped()
        {
            if (Master is NavigationPage)
                ((NavigationPage)Master).NotifyAllChildrenPopped();
            foreach (var page in this.Pages.Values)
            {
                if (page is NavigationPage)
                    ((NavigationPage)page).NotifyAllChildrenPopped();
            }
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            var tabIndex = _pagesInner.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);
             
            ListView.SelectedItem = _pageNames[tabIndex];
            return Task.FromResult((Detail as NavigationPage).CurrentPage.GetModel());
        }
    } 
}