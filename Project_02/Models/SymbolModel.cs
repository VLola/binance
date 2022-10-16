﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Project_02.Models
{
    public class SymbolModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public List<Price> Prices = new();
        public List<Order> Orders = new();
        public void AddPrice(Price price)
        {
            Prices.Add(price);
            Prices.ForEach(price => {
                if (price.Time < Time.AddMinutes(-1)) {
                    price.NotValid = true;
                }
            });
            Prices.RemoveAll(prices => prices.NotValid == true);
            AveragePrice = Prices.Sum(price => price.Value) / Prices.Count;
            decimal min = AveragePrice < price.Value ? AveragePrice : price.Value;
            decimal max = AveragePrice > price.Value ? AveragePrice : price.Value;
            Delta = Math.Round(((max / min) - 1) * 1000, 1);
        }
        private string _name { get; set; }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        private bool _select { get; set; }
        public bool Select
        {
            get { return _select; }
            set
            {
                _select = value;
                OnPropertyChanged("Select");
            }
        }
        private decimal _price { get; set; }
        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }
        private DateTime _time { get; set; }
        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }
        private bool _run { get; set; }
        public bool Run
        {
            get { return _run; }
            set
            {
                _run = value;
                OnPropertyChanged("Run");
            }
        }
        private decimal _deltan { get; set; }
        public decimal Delta
        {
            get { return _deltan; }
            set
            {
                _deltan = value;
                OnPropertyChanged("Delta");
            }
        }
        private decimal _averagePrice { get; set; }
        public decimal AveragePrice
        {
            get { return _averagePrice; }
            set
            {
                _averagePrice = value;
                OnPropertyChanged("AveragePrice");
            }
        }
        private bool _isOpenOrder { get; set; }
        public bool IsOpenOrder
        {
            get { return _isOpenOrder; }
            set
            {
                _isOpenOrder = value;
                OnPropertyChanged("IsOpenOrder");
            }
        }
    }
}
