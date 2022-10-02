

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using System.IO;

namespace ClassLibrary.Classes
{
    public static class GlobalVarsAndActions
    {
        static string storageType = "json";

        static string mp = Directory.GetCurrentDirectory();
        
        static string _clientsRepoPath = $"{mp}\\jsondb\\clients.json";
        static string _mainAccsRepoPath = $"{mp}\\jsondb\\mainAccs.json";
        static string _depoAccsRepoPath = $"{mp}\\jsondb\\depoAccs.json";
        static string _transactionsRepoPath = $"{mp}\\jsondb\\transactions.json";
        static string _sqldblogsPath = $"{mp}\\log\\sql\\";
        static string _jsondblogsPath = $"{mp}\\log\\json\\";
        //static string _sqldblogsPath = @"d:\repos\_BankAppSkillBoxHW\log\sql\";
        //static string _jsondblogsPath = @"d:\repos\_BankAppSkillBoxHW\log\json\";

        public static string StorageType 
        { 
            get { return storageType; } 
            set {
                switch (value)
                {
                    case "json":
                        storageType = "json";
                        break;
                    case "mssql":
                        storageType = "mssql";
                        break;
                    default:
                        storageType = "json";
                        break;
                }
                storageType = value; 
            } 
        }
        public static string ClientsRepoPath
        {
            get 
            {
                if (!Directory.Exists(Path.GetDirectoryName(_clientsRepoPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_clientsRepoPath));
                }
                if (!File.Exists(_clientsRepoPath))
                {
                    File.CreateText(_clientsRepoPath).Close();
                };
                return _clientsRepoPath;
            }
        }
        public static string MainAccsRepoPath
        {
            get
            {
                if (!Directory.Exists(Path.GetDirectoryName(_mainAccsRepoPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_mainAccsRepoPath));
                }
                if (!File.Exists(_mainAccsRepoPath))
                {
                    File.CreateText(_mainAccsRepoPath).Close();
                };
                return _mainAccsRepoPath;
            }
        }
        public static string DepoAccsRepoPath
        {
            get
            {
                if (!Directory.Exists(Path.GetDirectoryName(_depoAccsRepoPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_depoAccsRepoPath));
                }
                if (!File.Exists(_depoAccsRepoPath))
                {
                    File.CreateText(_depoAccsRepoPath).Close();
                };
                return _depoAccsRepoPath;
            }
        }
        public static string TransactionsRepoPath
        {
            get
            {
                if (!Directory.Exists(Path.GetDirectoryName(_transactionsRepoPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_transactionsRepoPath));
                }
                if (!File.Exists(_transactionsRepoPath))
                {
                    File.CreateText(_transactionsRepoPath).Close();
                };
                return _transactionsRepoPath;
            }
        }
        public static void SetLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File($"{ getLogPath() }\\AppLog.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
        static string getLogPath()
        {
            if(storageType == "mssql") return _sqldblogsPath;
            if(storageType == "json") return _jsondblogsPath;
            return _sqldblogsPath;
        }
        public static void LogInfo(string msg)
        {
            Log.Information(msg);
        }
        public static void LogAlarm(string msg)
        {
            Log.Error(msg);
        }
    }
}
