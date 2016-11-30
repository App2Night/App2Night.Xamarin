﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using App2Night.CustomView.Template;
using App2Night.PageModel.SubPages;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night
{
    public sealed class CustomMasterDetailContainer : MasterDetailPage, IFreshNavigationService
    {
        List<Xamarin.Forms.Page> _pagesInner = new List<Xamarin.Forms.Page>();
        Dictionary<MenuCellData, Xamarin.Forms.Page> _pages = new Dictionary<MenuCellData, Xamarin.Forms.Page>();
        ContentPage _menuPage;
        ObservableCollection<string> _pageNames = new ObservableCollection<string>();
        ListView _listView = new ListView()
        {
            RowHeight = 80
        }; 

        public Dictionary<MenuCellData, Xamarin.Forms.Page> Pages { get { return _pages; } }
        private ObservableCollection<string> PageNames { get { return _pageNames; } }

        public CustomMasterDetailContainer() : this(Constants.DefaultNavigationServiceName)
        {
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

        public void AddPage<T>(string title, string iconCode, object data = null) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T>(data);
            page.Title = title;
            page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
            _pagesInner.Add(page);
            var navigationContainer = CreateContainerPage(page);
            _pages.Add(new MenuCellData
            {
                IconCode = iconCode,
                Title = title
            },navigationContainer);
            _pageNames.Add(title);
            if (_pages.Count == 1)
                Detail = navigationContainer;
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
            _listView.ItemsSource = _pages.Keys;
            _listView.ItemTemplate = new DataTemplate(typeof(MenuTemplate));
            _listView.ItemSelected += (sender, args) => {
                if (_pages.ContainsKey((MenuCellData)args.SelectedItem))
                {
                    Detail = _pages[(MenuCellData)args.SelectedItem];
                }

                IsPresented = false;
            };
            _menuPage = new Page.SubPages.NavigationPage(_listView)
            {
                BindingContext = new NavigationViewModel(),
                Title = menuPageTitle
            }; 

            var navPage = new NavigationPage(_menuPage) { Title = "Menu" };

            if (!string.IsNullOrEmpty(menuIcon))
                navPage.Icon = menuIcon;

            Master = navPage;
        }

        public Task PushPage(Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return Detail.Navigation.PushModalAsync(page);

            KeyValuePair<MenuCellData, Xamarin.Forms.Page>  innerPage = _pages.FirstOrDefault(p => ((NavigationPage)p.Value).CurrentPage.GetType() == page.GetType());
            if (innerPage.Key != null)
            {
                return Task.FromResult(_listView.SelectedItem = innerPage.Key);
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

            //Detail = _pages.Values.ElementAt(tabIndex); ;
            _listView.SelectedItem = _pageNames[tabIndex];
            return Task.FromResult((Detail as NavigationPage).CurrentPage.GetModel());
        }
    }

    public class MenuCellData
    {
        public  string Title { get; set; }
        public string IconCode { get; set; }
    }
}