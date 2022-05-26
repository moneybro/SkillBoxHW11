using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    public static class GlobalVarsAndActions
    {
        static string _clientsRepoPath = @"d:\repos\SkillBoxHW13\Repositories\clients.json";
        static string _mainAccsRepoPath = @"d:\repos\SkillBoxHW13\Repositories\mainAccs.json";
        static string _depoAccsRepoPath = @"d:\repos\SkillBoxHW13\Repositories\depoAccs.json";
        
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
    }
}
