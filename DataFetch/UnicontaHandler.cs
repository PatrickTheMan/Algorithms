using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Uniconta.API.Service;
using Uniconta.API.System;
using Uniconta.ClientTools.DataModel;
using Uniconta.Common;
using Uniconta.Common.User;
using Uniconta.DataModel;

namespace Algorithms.DataFetch
{
    internal class UnicontaHandler
    {
        static readonly NameValueCollection config = ConfigurationManager.AppSettings;

        Session session;
        Company company;
        readonly Guid key;
        readonly QueryAPI api;

        //note: all of the null reference assignments should be taken care of within the constructor.
        //I disabled warnings cause annoying
        /* Explained:
         * The error shows up due to the data being read from the config file.
         * That on the other hand means, that the compiler cannot know whether the config file
         * actually has a value for "Username" or one calle "Password".
         * It expects it, because we provide the key, but it could be empty.
         */
#pragma warning disable CS8601, CS8604, CS8618 // Possible null reference assignment.
        readonly string username = config["Username"];
        readonly string password = config["Password"];
        UnicontaItem[] items;

        /// <summary>
        /// Instantiates a handler for the Uniconta conection
        /// </summary>
        /// <param name="IsProduction"> Defines whether return should be Production or Test </param>
        /// <returns> Uniconta Connection <br/>
        /// true => Prodction <br/>
        /// false => Test</returns>
        public UnicontaHandler(bool IsProduction)
        {
            string sessionType;
            if (IsProduction) { sessionType = "Production"; }
            else if (!IsProduction) { sessionType = "Test"; }
            else
            {
                Debug.WriteLine("Session Type Key is holding illegal value.");
                throw new Exception();
            }

            key = new(config["Key"]);

            if (password == null || username == null)
            {
                Debug.WriteLine(
                    $"Error loggin in. Attempted login with following data:"
                    + $"\nUserame: {username}"
                    + $"\nPassword: {password}"
                    + $"\nIf either of these values is null, please check App.congfig inside of the Data folder");
            }

            try
            {
                session = GetLoggedInSession();
                company = GetCompany(session, int.Parse(config[sessionType]));
            }
            catch (NullReferenceException ex)
            {
                Debug.Write(ex.Message);
            }
#pragma warning restore CS8601, CS8604, CS8618 // Possible null reference assignment.

            api = new(session, company);

        }

        public async Task<UnicontaItem[]> GetData()
        {
            return items ??= await api.Query<UnicontaItem>();
        }

        readonly ObservableCollection<InvTransClient> ResultList = new();

        public async Task<ObservableCollection<InvTransClient>> GetInvTransClientList()
        {
            InvTransClient[] here = await api.Query<InvTransClient>();

            if (ResultList.Count != here.Length)
            {
                foreach (InvTransClient item in here)
                {
                    ResultList.Add(item);
                }
            }

            return ResultList;

        }


        private bool LogInToSession()
        {
            Task<ErrorCodes> task = session.LoginAsync(username, password, LoginType.API, key);
            while (!task.IsCompleted)
            {
                Thread.Sleep(1000);
            }
            if (task.Result != ErrorCodes.Succes)
            {
                Debug.WriteLine(task.Result.ToString());
                return false;

            }
            else
                return true;
        }

        private Session? GetLoggedInSession()
        {
            UnicontaConnection connection = new(APITarget.Live);
            session = new(connection);

            if (LogInToSession())
            {
                return session;
            }
            else
                return null;

        }

        private Company GetCompany(Session session, int CompanyID)
        {
            Task<Company> task = session.GetCompany(CompanyID);

            while (!task.IsCompleted) { }
            company = task.Result;
            return company;
        }

    }
}