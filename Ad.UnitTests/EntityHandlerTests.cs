using System;
using Domain.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectGenerator;
using ObjectGenerator.SpecialityGenerators;
using Ad = Domain.Concrete.Ad;

namespace Ad.UnitTests
{
    [TestClass]
    public class EntityHandlerTests : BaseForTests
    {
        #region CRUD
        #region Create
        [TestMethod]
        public void CreateAd_SuccesfullyAdd()
        {
            bool result = handler.Add(new AdGenerator().PropertiesSetter(new Domain.Concrete.Ad()), true);

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void CreatePlaylist_SuccesfullyAdd()
        {
            bool result = handler.Add(new BaseGenerator<Playlist>().PropertiesSetter(new Playlist()), true);

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void CreateClient_SuccesfullyAdd()
        {
            bool result = handler.Add(new ClientGenerator().PropertiesSetter(new Client()), true);

            Assert.IsTrue(result);
        }
        #endregion
        #region Read
        [TestMethod]
        public void GetAd_SuccesfullyGet()
        {
            Domain.Concrete.Ad before = new AdGenerator().PropertiesSetter(new Domain.Concrete.Ad());

            bool result = handler.Add(before, true);

            Domain.Concrete.Ad after = handler.Find(before);

            Assert.IsTrue(result);
            Assert.IsNotNull(after);
        }
        [TestMethod]
        public void GetPlaylist_SuccesfullyGet()
        {
            Playlist before = new BaseGenerator<Playlist>().PropertiesSetter(new Playlist());

            bool result = handler.Add(before, true);

            Playlist after = handler.Find(before);

            Assert.IsTrue(result);
            Assert.IsNotNull(after);
        }
        [TestMethod]
        public void GetClient_SuccesfullyGet()
        {
            Client before = new ClientGenerator().PropertiesSetter(new Client());

            bool result = handler.Add(before, true);

            Client after = handler.Find(before);

            Assert.IsTrue(result);
            Assert.IsNotNull(after);
        }
        #endregion
        #region Update
        [TestMethod]
        public void UpdateAd_SuccesfullyUpdate()
        {
            Domain.Concrete.Ad before = new AdGenerator()
                .PropertiesSetter(new Domain.Concrete.Ad());

            bool result = handler.Add(before, true);

            Domain.Concrete.Ad after = handler.Find(before);

            after.Description = "ImAnAd";

            bool updateResult = handler.Update(after);

            Domain.Concrete.Ad updated = handler.Find(after);

            Assert.IsTrue(result);
            Assert.IsNotNull(after);
            Assert.IsTrue(updateResult);
            Assert.IsNotNull(updated);
            AreEqual(after, updated);
        }
        [TestMethod]
        public void UpdatePlaylist_SuccesfullyUpdate()
        {
            Playlist before = new BaseGenerator<Playlist>().PropertiesSetter(new Playlist());

            bool result = handler.Add(before, true);

            Playlist after = handler.Find(before);

            after.Description = "ImAPlaylist";

            bool updateResult = handler.Update(after);

            Playlist updated = handler.Find(after);

            Assert.IsTrue(result);
            Assert.IsNotNull(after);
            Assert.IsTrue(updateResult);
            Assert.IsNotNull(updated);
            AreEqual(after, updated);
        }
        [TestMethod]
        public void UpdateClient_SuccesfullyUpdate()
        {
            Client before = new ClientGenerator().PropertiesSetter(new Client());

            bool result = handler.Add(before, true);

            Client after = handler.Find(before);

            after.Description = "ImAClient";

            bool updateResult = handler.Update(after);

            Client updated = handler.Find(after);

            Assert.IsTrue(result);
            Assert.IsNotNull(after);
            Assert.IsTrue(updateResult);
            Assert.IsNotNull(updated);
            AreEqual(after, updated);
        }
        #endregion
        #region Delete 
        [TestMethod]
        public void DeleteAd_SuccesfullyDelete()
        {
            Domain.Concrete.Ad before = new AdGenerator().PropertiesSetter(new Domain.Concrete.Ad());

            bool result = handler.Add(before, true);

            Domain.Concrete.Ad after = handler.Find(before);

            bool deleteResult = handler.Delete(after);

            Assert.IsTrue(result);
            Assert.IsNotNull(after);
            Assert.IsTrue(deleteResult);
        }
        [TestMethod]
        public void DeletePlaylist_SuccesfullyDelete()
        {
            Playlist before = new BaseGenerator<Playlist>().PropertiesSetter(new Playlist());

            bool result = handler.Add(before, true);

            Playlist after = handler.Find(before);

            bool deleteResult = handler.Delete(after);

            Assert.IsTrue(result);
            Assert.IsNotNull(after);
            Assert.IsTrue(deleteResult);
        }
        [TestMethod]
        public void DeleteClient_SuccesfullyDelete()
        {
            Client before = new ClientGenerator().PropertiesSetter(new Client());

            bool result = handler.Add(before, true);

            Client after = handler.Find(before);

            bool deleteResult = handler.Delete(after);

            Assert.IsTrue(result);
            Assert.IsNotNull(after);
            Assert.IsTrue(deleteResult);
        }
        #endregion 
        #endregion

        #region Unique

        [TestMethod]
        public void OnlyAllowUniqueOneCreateAd_FailedAdd()
        {
            Domain.Concrete.Ad before = new AdGenerator().PropertiesSetter(new Domain.Concrete.Ad());

            bool result1 = handler.Add(before, true);
            bool result2 = handler.Add(before, true);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }
        [TestMethod]
        public void OnlyAllowUniqueOneCreatePlaylist_FailedAdd()
        {
            Playlist before = new BaseGenerator<Playlist>().PropertiesSetter(new Playlist());

            bool result1 = handler.Add(before, true);
            bool result2 = handler.Add(before, true);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }
        [TestMethod]
        public void OnlyAllowUniqueOneCreateClient_FailedAdd()
        {
            Client before = new ClientGenerator().PropertiesSetter(new Client());

            bool result1 = handler.Add(before, true);
            bool result2 = handler.Add(before, true);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        } 
        #endregion
    }
}
