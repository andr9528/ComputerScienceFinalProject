using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Core;
using Repository.EntityFramework;

namespace Ad.UnitTests
{
    
    public class BaseForTests
    {
        private readonly string connectionPath = "..\\..\\ConnectionStringFile.txt";
        private string connectionString = "";
        internal IRepository handler;

        [TestInitialize]
        public void Init()
        {
            handler = new EntityRepositoryHandler(connectionString: GetConnectionString());
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        private string GetConnectionString()
        {
            if (string.IsNullOrEmpty(connectionString))
                connectionString = File.ReadLines(connectionPath).First();
            return connectionString;
        }
    }
}
