using System;

namespace Project_07.Models
{
    public class SymbolModel : ChangedModel
    {
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
        private DateTime _openTime { get; set; }
        public DateTime OpenTime
        {
            get { return _openTime; }
            set
            {
                _openTime = value;
                OnPropertyChanged("OpenTime");
            }
        }
        private decimal _priceShort { get; set; }
        public decimal PriceShort
        {
            get { return _priceShort; }
            set
            {
                _priceShort = value;
                OnPropertyChanged("PriceShort");
            }
        }
        private decimal _takeProfitShort { get; set; }
        public decimal TakeProfitShort
        {
            get { return _takeProfitShort; }
            set
            {
                _takeProfitShort = value;
                OnPropertyChanged("TakeProfitShort");
            }
        }
        private decimal _stopLossShort { get; set; }
        public decimal StopLossShort
        {
            get { return _stopLossShort; }
            set
            {
                _stopLossShort = value;
                OnPropertyChanged("StopLossShort");
            }
        }
        private decimal _priceLong { get; set; }
        public decimal PriceLong
        {
            get { return _priceLong; }
            set
            {
                _priceLong = value;
                OnPropertyChanged("PriceLong");
            }
        }
        private decimal _takeProfitLong { get; set; }
        public decimal TakeProfitLong
        {
            get { return _takeProfitLong; }
            set
            {
                _takeProfitLong = value;
                OnPropertyChanged("TakeProfitLong");
            }
        }
        private decimal _stopLossLong { get; set; }
        public decimal StopLossLong
        {
            get { return _stopLossLong; }
            set
            {
                _stopLossLong = value;
                OnPropertyChanged("StopLossLong");
            }
        }

        private bool _isOpenShort { get; set; }
        public bool IsOpenShort
        {
            get { return _isOpenShort; }
            set
            {
                _isOpenShort = value;
                OnPropertyChanged("IsOpenShort");
            }
        }
        private bool _isOpenLong { get; set; }
        public bool IsOpenLong
        {
            get { return _isOpenLong; }
            set
            {
                _isOpenLong = value;
                OnPropertyChanged("IsOpenLong");
            }
        }
        private double _total { get; set; }
        public double Total
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged("Total");
            }
        }
    }
}
