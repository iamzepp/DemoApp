using System.Data;

namespace WebApiServer.Common.DbConnection
{
    public class DbProxy : IMainDbConnection
    {
        private IDbConnection _conncetion { get; set; }
        
        public DbProxy(IDbConnection connection)
        {
            _conncetion = connection;
        }
        
        public string ConnectionString { get => _conncetion.ConnectionString; set => _conncetion.ConnectionString = value; }

        public int ConnectionTimeout => _conncetion.ConnectionTimeout;

        public string Database => _conncetion.Database;

        public ConnectionState State => _conncetion.State;

        public IDbTransaction BeginTransaction()
        {
            return _conncetion.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _conncetion.BeginTransaction();
        }

        public void ChangeDatabase(string databaseName)
        {
            _conncetion.ChangeDatabase(databaseName);
        }

        public IDbCommand CreateCommand()
        {
            return _conncetion.CreateCommand();
        }

        public void Open()
        {
            _conncetion.Open();
        }
        
        public void Close()
        {
            _conncetion.Close();
        }
        
        public void Dispose()
        {
            _conncetion.Dispose();
        }
    }
}