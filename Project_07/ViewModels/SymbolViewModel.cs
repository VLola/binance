using Binance.Net.Clients;
using Binance.Net.Objects.Models.Futures;
using Project_07.Models;

namespace Project_07.ViewModels
{
    public class SymbolViewModel
    {
        public BinanceClient? Client { get; set; }
        public BinanceSocketClient? SocketClient { get; set; }
        public SymbolModel SymbolModel { get; set; } = new(); 
        public SymbolViewModel(BinanceClient? client, BinanceSocketClient? socketClient, BinanceFuturesUsdtSymbol symbol)
        {
            SymbolModel.Name = symbol.Name;
            Client = client;
            SocketClient = socketClient;
        }
    }
}
