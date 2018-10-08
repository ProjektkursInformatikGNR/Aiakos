using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aiakos
{
    public class Server
    {
        private string _name;
        private string _host;
        private string _port;
        private string _userId;
        private string _password;
        private string _database;

        public Server(string name, string host, string port, string userId, string password, string database)
        {
            _name = name;
            _host = host;
            _port = port;
            _userId = userId;
            _password = password;
            _database = database;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        public string Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string Database
        {
            get { return _database; }
            set { _database = value; }
        }
    }
}
