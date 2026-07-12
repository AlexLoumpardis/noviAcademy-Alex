using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Application;
using WorldRank.Domain;

namespace WorldRank.Infrastructure
{
    public class DBWalletRepository : IWalletRepository
    {

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private WorldRankDbContext _context;

        public DBWalletRepository(WorldRankDbContext context)
        {
            _context = context;
        }
        public void Add(Wallet wallet)
        {
            var exists = _context.Wallets.Any(w => w.PlayerId == wallet.PlayerId && w.Currency == wallet.Currency);
            if (exists)
                throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);

            _context.Wallets.Add(wallet);
            _logger.Info("Wallet created for player {PlayerId} in {Currency} with balance {Balance}", wallet.PlayerId, wallet.Currency, wallet.Balance);
            _context.SaveChanges();
        }

        public void Block(int playerId, Currency currency)
        {
            GetWallet(playerId, currency).Block();
            _logger.Info("Player {PlayerId} {Currency} wallet blocked", playerId, currency);
            _context.SaveChanges();
        }

        public void Deposit(int playerId, Currency currency, decimal amount)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Deposit(amount);        
            _context.SaveChanges();
        }

        public List<Wallet> GetAllWalletsByPlayerId(int playerId)
        {
            return _context.Wallets.Where(w => w.PlayerId == playerId).ToList();
            
        }

        private Wallet GetWallet(int playerId, Currency currency)
        {
            var wallet = _context.Wallets.SingleOrDefault(w => w.PlayerId == playerId && w.Currency == currency);
            if (wallet is null)
                throw new WalletNotFoundException(playerId, currency);
            return wallet;

        }

        public void Unblock(int playerId, Currency currency)
        {
            GetWallet(playerId, currency).Unblock();
            _logger.Info("Player {PlayerId} {Currency} wallet unblocked", playerId, currency);
            _context.SaveChanges();
        }

        public void UpdateBalance(int playerId, Currency currency, decimal newBalance)
        {
            GetWallet(playerId, currency).SetBalance(newBalance);
            _logger.Info("Player {PlayerId} {Currency} wallet balance set to {Balance}", playerId, currency, newBalance);
            _context.SaveChanges();
        }

        public void Withdraw(int playerId, Currency currency, decimal amount)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Withdraw(amount);
            _logger.Info("Withdrew {Amount} from player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
            _context.SaveChanges();
        }
    }
}
