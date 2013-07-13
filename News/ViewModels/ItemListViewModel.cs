using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace News.ViewModels
{
    public class ItemListViewModel : ObservableCollection<ItemViewModel>
    {
        new public void Add(ItemViewModel item)
        {
            ItemViewModel existingItem = getById(item.Id);
            if (existingItem == null)
            {
                this.insertItem(item);
            }
            else
            {
                existingItem.setUnread(item.Unread);
            }
        }

        protected ItemViewModel getById(int id)
        {
            foreach (ItemViewModel item in this)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }

        protected void insertItem(ItemViewModel newItem)
        {
            foreach (ItemViewModel item in this)
            {
                if (item.Id < newItem.Id)
                {
                    this.Insert(this.IndexOf(item), newItem);
                    return;
                }
            }
            base.Add(newItem);
        }
    }
}
