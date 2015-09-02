using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using KK.DealMaker.Core.Constraint;

namespace KK.DealMaker.Core.Helper
{
    public class LDAPHelper
    {
        public class LdapUser
        {
            public string WindowsLogin { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string DomainName {get; set;}
            public string Email {get; set;}
            public string Branch { get; set; }
            public string UserGroup { get; set; }
        }

        private static string LdapServer;
        private static string LdapPath;
        private static string LdapComponent;

        static LDAPHelper()
        {
            LdapServer = System.Configuration.ConfigurationManager.AppSettings[AppSettingName.LDAP_SERVER];
            LdapComponent = System.Configuration.ConfigurationManager.AppSettings[AppSettingName.LDAP_COMPONENT];
            LdapPath = @"LDAP://" + LdapServer + (String.IsNullOrEmpty(LdapComponent) ? "" : string.Format("/{0}", LdapComponent));
        }

        public static bool Exists(string objectPath)
        {
            bool found = false;
            if (DirectoryEntry.Exists(@"LDAP://" + objectPath))
            {
                found = true;
            }
            return found;
        }

        public static bool ValidateUser(string windowsUserName, string password)
        {            
            try
            {
                DirectoryEntry entry = new DirectoryEntry(@"LDAP://" + LdapServer, windowsUserName, password);
                DirectorySearcher searcher = new DirectorySearcher(entry);
                searcher.Filter = "(&(objectCategory=person)(samAccountName=" + windowsUserName + "))";
                SearchResult result = searcher.FindOne();

                if (result != null)
                {
                    LoggingHelper.Debug(windowsUserName + " can access!!");
                    return true;
                }
                LoggingHelper.Debug(windowsUserName + " is not authorized!!");
                return false;
            }
            catch(Exception ex)
            {
                LoggingHelper.Debug(windowsUserName + " is not authorized!! (" +  ex.Message + ")");
                return false;
            }
  
        }

        public static bool UserExists(string windowsUserName)
        {
            try
            {
                using (var ctx = new PrincipalContext(ContextType.Domain, LdapServer, LdapComponent))
                {
                    var user = UserPrincipal.FindByIdentity(ctx, windowsUserName);

                    if (user != null)
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug(windowsUserName + " is not authorized!! (" + ex.Message + ")");
                return false;
            }
        }

        //Translate the Friendly Domain Name to Fully Qualified Domain
        public static string FriendlyDomainToLdapDomain(string friendlyDomainName)
        {
            string ldapPath = null;
            try
            {
                DirectoryContext objContext = new DirectoryContext(
                                    DirectoryContextType.Domain, friendlyDomainName);
                Domain objDomain = Domain.GetDomain(objContext);
                ldapPath = objDomain.Name;
            }
            catch (DirectoryServicesCOMException e)
            {
                ldapPath = e.Message.ToString();
            }
            return ldapPath;
        }

        public static LdapUser GetLdapUser(string windowUserLogin)
        {
            LdapUser user = null;

            DirectoryEntry entry = new DirectoryEntry(LdapPath);
            
            DirectorySearcher searcher = new DirectorySearcher(entry);
            //searcher.PropertiesToLoad.Add("givenName");
            //searcher.PropertiesToLoad.Add("sn");
            searcher.Filter = "(&(objectCategory=person)(samAccountName=" + windowUserLogin + "))";
            SearchResult result = searcher.FindOne();

            if (result != null)
            {
                user = new LdapUser();
                user.FirstName = result.Properties["givenName"][0].ToString();
                user.LastName = result.Properties["sn"][0].ToString();
                user.WindowsLogin = windowUserLogin;

            }

            return user;
        }

        public static LdapUser[] GetLdapUser()
        {
            List<LdapUser> users = new List<LdapUser>();

            DirectoryEntry entry = new DirectoryEntry(LdapPath);

            LoggingHelper.Debug(entry.Path);

            DirectorySearcher searcher = new DirectorySearcher(entry);
            searcher.Filter = "(&(objectClass=user)(objectCategory=person))";

            SearchResult result;
            SearchResultCollection resultCol = searcher.FindAll();
            if (resultCol != null)
            {
                for (int counter = 0; counter < resultCol.Count; counter++)
                {
                    result = resultCol[counter];
                    if (result.Properties.Contains("samaccountname"))
                    {
                        users.Add(new LdapUser() 
                            { WindowsLogin = (String)result.Properties["samaccountname"][0],
                              FirstName = (result.Properties["givenName"].Count > 0?(String)result.Properties["givenName"][0]:null),
                              LastName = (result.Properties["sn"].Count > 0?(String)result.Properties["sn"][0]:null),
                              Email = (result.Properties["mail"].Count > 0?(String)result.Properties["mail"][0]:null)
                            });
                    }
                }
            }
            return users.ToArray<LdapUser>();

        }
        
        public static LdapUser[] SearchLdapByFirstName(string firstName)
        {
            List<LdapUser> users = new List<LdapUser>();

            DirectoryEntry entry = new DirectoryEntry(LdapPath);

            LoggingHelper.Debug(entry.Path);

            DirectorySearcher searcher = new DirectorySearcher(entry);
            searcher.Filter = "(&(objectClass=user)(objectCategory=person)(givenName=" + firstName + "*))";

            SearchResult result;
            SearchResultCollection resultCol = searcher.FindAll();
            if (resultCol != null)
            {
                for (int counter = 0; counter < resultCol.Count; counter++)
                {
                    result = resultCol[counter];
                    if (result.Properties.Contains("samaccountname"))
                    {
                        users.Add(new LdapUser()
                        {
                            WindowsLogin = (String)result.Properties["samaccountname"][0],
                            FirstName = (result.Properties["givenName"].Count > 0 ? (String)result.Properties["givenName"][0] : null),
                            LastName = (result.Properties["sn"].Count > 0 ? (String)result.Properties["sn"][0] : null),
                            Email = (result.Properties["mail"].Count > 0 ? (String)result.Properties["mail"][0] : null)
                        });
                    }
                }
            }
            return users.ToArray<LdapUser>();

        }
    }
}
