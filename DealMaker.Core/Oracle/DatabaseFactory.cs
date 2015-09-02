using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;
using KK.DealMaker.Core.Helper;

namespace KK.DealMaker.Core.Oracle
{
    public sealed class DatabaseFactory 
    {
        public static DatabaseFactorySectionHandler sectionHandler = (DatabaseFactorySectionHandler)ConfigurationManager.GetSection("DatabaseFactoryConfiguration");

        private DatabaseFactory() { }

        public static Database CreateDatabase()
        {
            LoggingHelper.Debug("===CresteDatabase===");
            // Verify a DatabaseFactoryConfiguration line exists in the web.config.
            if (sectionHandler.Name.Length == 0)
            {
                LoggingHelper.Debug("Database name not defined in DatabaseFactoryConfiguration section of web.config.");
                throw new Exception("Database name not defined in DatabaseFactoryConfiguration section of web.config.");
            }

            try
            {
                // Find the class
                Type database = Type.GetType(sectionHandler.Name);

                // Get it's constructor
                ConstructorInfo constructor = database.GetConstructor(new Type[] { });

                // Invoke it's constructor, which returns an instance.
                Database createdObject = (Database)constructor.Invoke(null);

                // Initialize the connection string property for the database.
                createdObject.connectionString = sectionHandler.ConnectionString;

                // Pass back the instance as a Database
                return createdObject;
            }
            catch (Exception excep)
            {
                LoggingHelper.Debug("Error instantiating database " + sectionHandler.Name + ". " + excep.Message);
                throw new Exception("Error instantiating database " + sectionHandler.Name + ". " + excep.Message);
            }
        }
    }

    public sealed class DatabaseFactorySectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string)base["Name"]; }
        }

        [ConfigurationProperty("ConnectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)base["ConnectionStringName"]; }
        }

        public string ConnectionString
        {
            get
            {
                try
                {
                    return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
                }
                catch (Exception excep)
                {
                    LoggingHelper.Debug("Connection string " + ConnectionStringName + " was not found in web.config. " + excep.Message);
                    throw new Exception("Connection string " + ConnectionStringName + " was not found in web.config. " + excep.Message);
                }
            }
        }
    }
}
