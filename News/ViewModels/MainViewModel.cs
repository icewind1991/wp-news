using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using News.Resources;
using System.Collections.Generic;
using News.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace News.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel(AppSettings settings)
        {
            this.settings = settings;
            this.Items = new ObservableCollection<ItemViewModel>();
            this.ActiveItems = new ObservableCollection<ItemViewModel>();
            this.Folders = new ObservableCollection<FolderViewModel>();
            if (this.settings.UrlSetting.StartsWith("http"))
            {
                this.server = new Server(this.settings.UrlSetting, this.settings.UsernameSetting, this.settings.PasswordSetting);
            }
        }

        public bool IsConfigured
        {
            get
            {
                return this.server != null;
            }

        }

        private Server server;

        private AppSettings settings;

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public ObservableCollection<FolderViewModel> Folders { get; private set; }

        public ObservableCollection<ItemViewModel> ActiveItems { get; set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public ItemViewModel ActiveItem { get; set; }
        public FolderViewModel ActiveFolder { get; set; }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public bool IsLoading { get; set; }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        async public Task<bool> LoadData()
        {
            if (!this.IsConfigured)
            {
                return false;
            }
            IsLoading = true;
            this.Folders.Clear();
            NotifyPropertyChanged("IsLoading");
            try
            {
                Folder[] folders = await server.getFoldersAsync();

                FolderViewModel folderView;
                FolderViewModel all = new FolderViewModel(new Folder() { name = "All", id = -1 }, server);
                this.Folders.Add(all);
                foreach (Folder folder in folders)
                {
                    folderView = new FolderViewModel(folder, server);
                    this.Folders.Add(folderView);
                }

                IsLoading = false;
                this.IsDataLoaded = true;
                NotifyPropertyChanged("IsLoading");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("loading folders failed");
                IsLoading = false;
                NotifyPropertyChanged("IsLoading");
                this.server = null;
                return false;
            }
        }

        public void applySettings(AppSettings settings)
        {
            this.settings = settings;
            this.IsDataLoaded = false;
            if (this.settings.UrlSetting.StartsWith("http"))
            {
                this.server = new Server(this.settings.UrlSetting, this.settings.UsernameSetting, this.settings.PasswordSetting);
            }
            else
            {
                this.server = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        async public void loadFolder(int i, int offset)
        {
            try
            {
                IsLoading = true;
                NotifyPropertyChanged("IsLoading");
                ActiveFolder = this.Folders[i];
                Folder folder = this.Folders[i].folder;
                Item[] items;
                if (folder.id >= 0)
                {
                    items = await server.getFolderItemsAsync(folder, offset);

                }
                else
                {
                    items = await server.getAllItemsAsync(offset);
                }
                ItemViewModel itemView;
                foreach (Item item in items)
                {
                    itemView = new ItemViewModel(item, server);
                    this.Folders[i].Items.Add(itemView);
                }
                IsLoading = false;
                NotifyPropertyChanged("IsLoading");
            }
            catch (Exception e)
            {
                IsLoading = false;
                NotifyPropertyChanged("IsLoading");
            }
        }
    }
}