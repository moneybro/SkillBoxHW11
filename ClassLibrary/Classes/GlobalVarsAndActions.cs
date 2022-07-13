using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace ClassLibrary.Classes
{
    public static class GlobalVarsAndActions
    {
        static string mp = @"d:\repos\_BankAppSkillBoxHW\Repositories\";
        static string _clientsRepoPath = $"{mp}clients.json";
        static string _mainAccsRepoPath = $"{mp}mainAccs.json";
        static string _depoAccsRepoPath = $"{mp}depoAccs.json";
        static string _transactionsRepoPath = $"{mp}transactions.json";
        static string _logsPath = @"d:\repos\_BankAppSkillBoxHW\log\";

        public static string ClientsRepoPath
        {
            get { return _clientsRepoPath;}
        }
        public static string MainAccsRepoPath
        {
            get {
                if (File.Exists(_mainAccsRepoPath))
                {
                    return _mainAccsRepoPath;
                }
                else
                {
                    File.Create(_mainAccsRepoPath).Close();
                    return _mainAccsRepoPath;
                }
            }
        }
        public static string DepoAccsRepoPath
        {
            get
            {
                if (File.Exists(_depoAccsRepoPath))
                {
                    return _depoAccsRepoPath;
                }
                else
                {
                    File.Create(_depoAccsRepoPath).Close();
                    return _depoAccsRepoPath;
                }
            }
        }
        public static string TransactionsRepoPath
        {
            get
            {
                if (File.Exists(_transactionsRepoPath))
                {
                    return _transactionsRepoPath;
                }
                else
                {
                    File.Create(_transactionsRepoPath).Close();
                    return _transactionsRepoPath;
                }
            }
        }
        
        public static void SetLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File($"{_logsPath}\\AppLog.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
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
