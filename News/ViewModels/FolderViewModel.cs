using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.Models;
using System.Diagnostics;

namespace News.ViewModels
{
    public class FolderViewModel : INotifyPropertyChanged
    {
        public Folder folder;
        private ItemListViewModel _items;
        private Server server;

        public FolderViewModel()
        {
            folder = new Folder();
            this.Items = new ItemListViewModel();
        }

        public FolderViewModel(Folder newFolder, Server server)
        {
            folder = newFolder;
            this.server = server;
            this.Items = new ItemListViewModel();
        }

        public string Name
        {
            get
            {
                return folder.name;
            }
        }

        public ItemListViewModel Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                NotifyPropertyChanged("Items");
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

        public void LoadPage(int offset)
        {
        }
    }
}
