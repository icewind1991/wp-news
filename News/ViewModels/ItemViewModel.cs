using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using News.Models;

namespace News.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private Item _item;

        private Server server;

        public ItemViewModel()
        {
            _item = new Item();
        }

        public ItemViewModel(Item item, Server server)
        {
            _item = item;
            this.server = server;
        }

        public string Title
        {
            get
            {
                return _item.title;
            }
        }

        public string Author
        {
            get
            {
                return _item.author;
            }
        }

        public string Body
        {
            get
            {
                return _item.body;
            }
        }

        public string Url
        {
            get
            {
                return _item.url;
            }
        }

        public bool Unread
        {
            get
            {
                return _item.unread;
            }
            set
            {
                if (value != _item.unread)
                {
                    _item.unread = value;
                    if (value)
                    {
                        server.markAsUnRead(_item.id);
                    }
                    else
                    {
                        server.markAsRead(_item.id);
                    }
                }
            }
        }

        public void setUnread(bool unread)
        {
            _item.unread = unread;
            NotifyPropertyChanged("Unread");
        }

        public int Id
        {
            get
            {
                return _item.id;
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
    }
}