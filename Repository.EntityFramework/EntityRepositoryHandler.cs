using Domain.Core;
using Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Domain.Concrete;
using Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using DateTime = System.DateTime;

namespace Repository.EntityFramework
{
    public class EntityRepositoryHandler : IRepository
    {
        public EntityRepository repo;
        private string connectionString = string.Empty;

        public EntityRepositoryHandler(string connectionString = null, bool useLazyLoading = true, bool ensureDeleted = false, bool ensureCreated = true, bool migrate = false)
        {
            if (connectionString == null)
                repo = new EntityRepository(useLazyLoading: useLazyLoading, ensureDeleted: ensureDeleted, ensureCreated: ensureCreated, migrate: migrate);
            else
            {
                repo = new EntityRepository(connectionString, useLazyLoading, ensureDeleted, ensureCreated, migrate);
                this.connectionString = connectionString;
            }    
        }

        public T Find<T>(T predicate) where T : class, IEntity
        {
            IEntity entity = null;
            switch (predicate)
            {
                // Create cases for all the different classes that should be retriable from the database

                // Example:
                // case IYourDomainClass y:
                //    entity = FindYourDomainClass(y);
                //    break;
                case IAd a:
                    entity = FindAd(a);
                    break;
                case IClient c:
                    entity = FindClient(c);
                    break;
                case ILogEntry l:
                    entity = FindLogEntry(l);
                    break;
                case IUser u:
                    entity = FindUser(u);
                    break;
                default :
                    throw new Exception("Nooooooooooooooo"); 
            }

            return entity as T;
        }

        public ICollection<T> FindMultiple<T>(T predicate) where T : class, IEntity
        {
            ICollection<T> entities = null;
            switch (predicate)
            {
                // Create cases for all the different classes that should be retriable from the database

                // Example:
                // case IYourDomainClass y:
                //    entities = FindMultipleYourDomainClassInPlural(y) as ICollection<T>;
                //    break;
                case IAd a:
                    entities = FindMultipleAds(a) as ICollection<T>;
                    break;
                case IClient c:
                    entities = FindMultipleClients(c) as ICollection<T>;
                    break;
                case ILogEntry l:
                    entities = FindMultipleLogEntries(l) as ICollection<T>;
                    break;
                case IUser u:
                    entities = FindMultipleUsers(u) as ICollection<T>;
                    break;
                default :
                    throw new Exception("Nooooooooooooooo"); 
            }

            return entities;
        }



        #region Find Query Builders

        // There should be one query for each case in either 'Find' or 'FindMultiple',
        // meaning if there is a case to find YourDomainClass in both methods,
        // then there should be one query builder, meant to build queries for YourDomainClass
        // as both find methods make use of the same query, only the amount of elements returned are diffent.
        /*
        private IQueryable<YourDomainClass> BuildFindYourDomainClassQuery(IYourDomainClass y, IQueryable<YourDomainClass> query)
        {
            Check whether or not a property has been set, if it has been set, add a where to the query including the property.

            // Example:
            if (y.PropertyA != default(PropertyAType))
                query = query.Where(x => x.PropertyA == y.PropertyA);

            // If it is a string then use the method 'IsNullOrEmpty' and the method 'Contains'

            if (!y.PropertyB.IsNullOrEmpty())
                query = query.Where(x => x.PropertyB.Contains(y.PropertyB));
            return query;
        }
        */

        private IQueryable<User> BuildFindUserQuery(IUser u, IQueryable<User> query)
        {
            if (!u.Email.IsNullOrEmpty())
                query = query.Where(x=>x.Email.Contains(u.Email));
            if (!u.Username.IsNullOrEmpty())
                query = query.Where(x => x.Username.Contains(u.Username));
            if (!u.Password.IsNullOrEmpty())
                query = query.Where(x => x.Password.Contains(u.Password));

            return query;
        }

        // Might need to expand the 'AdsPlayTime' to use Hours/Minutes/Seconds
        private IQueryable<Client> BuildFindClientQuery(IClient c, IQueryable<Client> query)
        {
            if (c.AdsCount != default(int))
                query = query.Where(x => x.AdsCount == c.AdsCount);
            if (c.AdsPlayCount != default(int))
                query = query.Where(x => x.AdsPlayCount == c.AdsPlayCount);

            if (!c.Ip.IsNullOrEmpty())
                query = query.Where(x => x.Ip.Contains(c.Ip));

            if (c.CreationDate != default(DateTime))
                query = query.Where(x => x.CreationDate.Date == c.CreationDate.Date);

            if (c.AdsPlayTime != default(TimeSpan))
                query = query.Where(x => x.AdsPlayTime.TotalHours == c.AdsPlayTime.TotalHours);

            return query;
        }

        // Might need to expand the 'TotalPlayTime' to use Hours/Minutes/Seconds
        private IQueryable<Ad> BuildFindAdQuery(IAd a, IQueryable<Ad> query)
        {
            if (a.ClientsCount != default(int))
                query = query.Where(x => x.ClientsCount == a.ClientsCount);
            if (a.TotalPlayCount != default(int))
                query = query.Where(x => x.TotalPlayCount == a.TotalPlayCount);

            if (!a.AdName.IsNullOrEmpty())
                query = query.Where(x => x.AdName.Contains(a.AdName));
            if (!a.FileExtension.IsNullOrEmpty())
                query = query.Where(x => x.FileExtension.Contains(a.FileExtension));
            if (!a.FileLocation.IsNullOrEmpty())
                query = query.Where(x => x.FileLocation.Contains(a.FileLocation));

            if (a.CreationDate != default(DateTime))
                query = query.Where(x => x.CreationDate.Date == a.CreationDate.Date);

            if (a.TotalPlayTime != default(TimeSpan))
                query = query.Where(x => x.TotalPlayTime.TotalHours == a.TotalPlayTime.TotalHours);

            return query;
        }

        private IQueryable<LogEntry> BuildFindLogEntryQuery(ILogEntry l, IQueryable<LogEntry> query)
        {
            if (!l.Message.IsNullOrEmpty())
                query = query.Where(x => x.Message.Contains(l.Message));
            if (!l.StackTrace.IsNullOrEmpty())
                query = query.Where(x => x.StackTrace.Contains(l.StackTrace));

            if (l.TimeStamp != default(DateTime))
                query = query.Where(x => x.TimeStamp.Date == l.TimeStamp.Date);

            return query;
        }

        #endregion

        #region Find Multiple Methods

        private ICollection<T> FindMultipleResults<T>(IQueryable<T> query) where T : class, IEntity
        {
            var result = query.ToList().Distinct();
            if (result.Count() > 0)
                return new List<T>(result);
            else
                throw new Exception(string.Format("Found no result for {0}", typeof(T).Name));
        }
        // Create methods for all the different classes, where you should be able to get multiple specific elements.

        // Example:
        /*
        private ICollection<YourDomainClass> FindMultipleSubscriptions(IYourDomainClass y)
        {
            var query = repo.YourDomainClassInPlural.AsQueryable();
            query = BuildFindYourDomainClassQuery(y, query);

            return FindMultipleResults(query);
        }
        */

        private ICollection<User> FindMultipleUsers(IUser u)
        {
            var query = repo.Users.AsQueryable();
            query = BuildFindUserQuery(u, query);

            return FindMultipleResults(query);
        }

        private ICollection<LogEntry> FindMultipleLogEntries(ILogEntry l)
        {
            var query = repo.LogEntries.AsQueryable();
            query = BuildFindLogEntryQuery(l, query);

            return FindMultipleResults(query);
        }

        private ICollection<Client> FindMultipleClients(IClient c)
        {
            var query = repo.Clients.AsQueryable();
            query = BuildFindClientQuery(c, query);

            return FindMultipleResults(query);
        }

        private ICollection<Ad> FindMultipleAds(IAd a)
        {
            var query = repo.Ads.AsQueryable();
            query = BuildFindAdQuery(a, query);

            return FindMultipleResults(query);
        }

        #endregion

        #region Find Single Methods

        private T FindAResult<T>(IQueryable<T> query) where T : class, IEntity
        {
            var result = query.ToList().Distinct();
            if (result.Count() == 1)
                return result.First();
            else if (result.Count() > 1)
                throw new Exception(string.Format("More than 1 result found when searching for a {0}", typeof(T).Name));
            else
                throw new Exception(string.Format("No results found when searching for a {0}", typeof(T).Name));
        }
        // Create methods for all the different classes, where you should be able to get one specific element.

        // Example:
        /*
        private IYourDomainClass FindYourDomainClass(IYourDomainClass y)
        {
            var query = repo.YourDomainClassAsPlural.AsQueryable();
            query = BuildFindYourDomainClassQuery(y, query);

            return FindAResult(query);
        }
        */

        private IAd FindAd(IAd a)
        {
            var query = repo.Ads.AsQueryable();
            query = BuildFindAdQuery(a, query);

            return FindAResult(query);
        }

        private IClient FindClient(IClient c)
        {
            var query = repo.Clients.AsQueryable();
            query = BuildFindClientQuery(c, query);

            return FindAResult(query);
        }

        private IUser FindUser(IUser u)
        {
            var query = repo.Users.AsQueryable();
            query = BuildFindUserQuery(u, query);

            return FindAResult(query);
        }

        private ILogEntry FindLogEntry(ILogEntry l)
        {
            var query = repo.LogEntries.AsQueryable();
            query = BuildFindLogEntryQuery(l, query);

            return FindAResult(query);
        }



        #endregion

        public bool Delete<T>(T predicate, bool resetRepo = true, bool autoSave = true) where T : class, IEntity
        {
            if (predicate.Id == 0)
                throw new Exception(string.Format("I need an Id to figure out what to remove"), new ArgumentException("Id of predicate can not be 0"));
            EntityState state = EntityState.Unchanged;
            EntityEntry entry = null;

            switch (predicate)
            {
                //Create cases for all the different classes that should be updateable in the database.

                // Example:
                // case IYourDomainClass y:
                //    entry = repo.Remove(y);
                //    break;
                case IAd a:
                    entry = repo.Remove(a);
                    break;
                case IClient c:
                    entry = repo.Remove(c);
                    break;
                case IUser u:
                    entry = repo.Remove(u);
                    break;
                case ILogEntry l:
                    entry = repo.Remove(l);
                    break;
                default :
                    throw new Exception("Nooooooooooooooo"); 
            }

            if (entry != null)
                state = CheckEntryState(state, entry);

            if (state == EntityState.Deleted)
            {
                if (autoSave == true)
                    Save();
                if (resetRepo == true)
                    ResetRepo();
                return true;
            }
            else return false;
        }

        public bool Update<T>(T predicate, bool resetRepo = true, bool autoSave = true) where T : class, IEntity
        {
            if (predicate.Id == 0)
                throw new Exception(string.Format("I need an Id to figure out what to update"), new ArgumentException("Id of predicate can not be 0"));
            EntityState state = EntityState.Unchanged;
            EntityEntry entry = null;

            switch (predicate)
            {
                //Create cases for all the differnet classes that should be updateable in the database.

                // Example:
                // case IYourDomainClass y:
                //    entry = repo.Update(y);
                //    break;
                case IAd a:
                    entry = repo.Update(a);
                    break;
                case IClient c:
                    entry = repo.Update(c);
                    break;
                case IUser u:
                    entry = repo.Update(u);
                    break;
                case ILogEntry l:
                    entry = repo.Update(l);
                    break;
                default :
                    throw new Exception("Nooooooooooooooo"); 
            }

            if (entry != null)
                state = CheckEntryState(state, entry);

            if (state == EntityState.Modified)
            {
                if (autoSave == true)
                    Save();
                if (resetRepo == true)
                    ResetRepo();
                return true;
            }
            else return false;


        }

        public bool Add<T>(T element, bool autoSave = false) where T : class, IEntity
        {
            EntityState state = EntityState.Unchanged;
            EntityEntry entry = null;

            switch (element)
            {
                // Create cases for all the different classes that should be addable to the database.
                // Remember to make us of the method 'AddIfNotExist' from Helpers, as it insures no duplicate data is added.
                // Make use of ValueTuples as it' predicate to have it check multiple properties of the inputes element.

                // Example:
                // case IYourDomainClass y:
                //    entry = repo.YourDomainClassInPlural.AddIfNotExists(y as YourDomainClass, x => (x.PropertyA, x.PropertyB, ...));
                //    break;
                case IAd a:
                    entry = repo.Ads.AddIfNotExists(a as Ad, x => (x.AdName, x.FileExtension, x.FileLocation));
                    break;
                case IClient c:
                    entry = repo.Clients.AddIfNotExists(c as Client, x => (x.Ip));
                    break;
                case ILogEntry l:
                    entry = repo.LogEntries.AddIfNotExists(l as LogEntry, x => (x.TimeStamp));
                    break;
                case IUser u:
                    entry = repo.Users.AddIfNotExists(u as User, x => (x.Username, x.Email, x.Password));
                    break;
                default:
                    throw new Exception("Nooooooooooooooo");
            }

            if (entry != null)
                state = CheckEntryState(state, entry);

            if (state == EntityState.Added)
            {
                if (autoSave == true)
                    Save();
                return true;
            }
            else
                return false;
        }

        public string AddRange<T>(ICollection<T> elements, bool autoSave = true) where T : class, IEntity
        {
            List<bool> results = new List<bool>();

            foreach (var element in elements)
            {
                results.Add(Add(element));
            }

            if(autoSave == true)
                Save();

            return string.Format("Added {0} out of {1}.", results.Where(b => b).Count(), elements.Count);
        }

        private static EntityState CheckEntryState(EntityState state, EntityEntry entry)
        {
            if (entry != null)
                state = entry.State;
            return state;
        }

        public void Save()
        {
            repo.SaveChanges();
        }

        public void ResetRepo()
        {
            repo.Dispose();
            repo = null;
            if (connectionString == string.Empty)
                repo = new EntityRepository();
            else
                repo = new EntityRepository(connectionString);
        }
    }
}
