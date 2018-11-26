using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
            handler = new EntityRepositoryHandler(connectionString: GetConnectionString(), ensureDeleted: false);
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

        internal void AreEqual<TBefore, TAfter>(TBefore after, TAfter updated)
            where TBefore : class, IEntity
            where TAfter : class, IEntity
        {
            PropertyInfo[] afterInfos = after.GetType().GetProperties();
            PropertyInfo[] updatedInfos = updated.GetType().GetProperties();

            updatedInfos = updatedInfos.Where(x => x.Name != "LazyLoader").ToArray();

            afterInfos = afterInfos.OrderBy(x => x.Name).ToArray();
            updatedInfos = updatedInfos.OrderBy(x => x.Name).ToArray();

            if (afterInfos.Length != updatedInfos.Length)
                throw new Exception("Amount of properties in given T's does not match," +
                                    " properly two completly differnet types.");

            for (int i = 0; i < afterInfos.Length; i++)
            {
                if (afterInfos[i].GetValue(after).GetType().FullName.Contains("System.Collections") ||
                    updatedInfos[i].GetValue(updated).GetType().FullName.Contains("System.Collections"))
                    continue;

                // test that the name on the same spot matches
                Assert.AreEqual(afterInfos[i].Name, updatedInfos[i].Name);

                // test that the values on the same spot matches
                Assert.AreEqual(afterInfos[i].GetValue(after), updatedInfos[i].GetValue(updated));
            }
        }
    }
}
