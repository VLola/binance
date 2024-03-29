﻿using ScottPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project_06.Models
{
    public class MainModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public ObservableCollection<string> SymbolsName { get; set; } = new();
        public ObservableCollection<SymbolModel> Symbols { get; set; } = new();
        public List<int> StatisticsNumbers { get; set; } = new();
        public ObservableCollection<StatisticsModel> Statistics { get; set; } = new();
        public WpfPlot MyPlot { get; set; } = new();
        public WpfPlot MyPlotLine { get; set; } = new();
        private SymbolModel _selectedSymbol { get; set; }
        public SymbolModel SelectedSymbol
        {
            get { return _selectedSymbol; }
            set
            {
                _selectedSymbol = value;
                OnPropertyChanged("SelectedSymbol");
            }
        }
        private StatisticsModel _selectedStatistics { get; set; }
        public StatisticsModel SelectedStatistics
        {
            get { return _selectedStatistics; }
            set
            {
                _selectedStatistics = value;
                OnPropertyChanged("SelectedStatistics");
            }
        }
        private bool _isLoading { get; set; }
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }
        private int _plus { get; set; } = 0;
        public int Plus
        {
            get { return _plus; }
            set
            {
                _plus = value;
                OnPropertyChanged("Plus");
            }
        }
        private int _minus { get; set; } = 0;
        public int Minus
        {
            get { return _minus; }
            set
            {
                _minus = value;
                OnPropertyChanged("Minus");
            }
        }
        private double _plusPercent { get; set; } = 0;
        public double PlusPercent
        {
            get { return _plusPercent; }
            set
            {
                _plusPercent = value;
                OnPropertyChanged("PlusPercent");
            }
        }
        private double _minusPercent { get; set; } = 0;
        public double MinusPercent
        {
            get { return _minusPercent; }
            set
            {
                _minusPercent = value;
                OnPropertyChanged("MinusPercent");
            }
        }
        private int _number { get; set; } = 0;
        public int Number
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged("Number");
            }
        }
    }
}
