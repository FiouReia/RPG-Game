﻿using System.ComponentModel;

namespace Engine
{
    public class InventoryItem : INotifyPropertyChanged
    {
        private Item _details;
        private int _quantity;

        public int ItemID
        {
            get { return Details.ID; }
        }

        public int Price
        {
            get { return Details.Price; }
        }

        public Item Details
        {
            get { return _details; }
            set
            {
                _details = value;
                OnPropertyChanged("Details");
            }
        }
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
                OnPropertyChanged("Description");
            }
        }
        public string Description
        {
            get { return Quantity == 1 ?  Details.Name:Details.NamePlural; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public InventoryItem(Item details, int quantity)
        {
            Details = details;
            Quantity = quantity;
        }
    }
}
