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
using IEntity = Repository.Core.IEntity;

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

            // If it is a string then use the method 'IsNullOrEmpty'

            if (y.PropertyB.IsNullOrEmpty)
                query = query.Where(x => x.PropertyB == y.PropertyB);
            return query;
        }
        */

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
        private ICollection<Subscription> FindMultipleSubscriptions(ISubscription s)
        {
            var query = repo.YourDomainClassAsPlural.AsQueryable();
            query = BuildFindYourDomainClassQuery(y, query);

            return FindMultipleResults(query);
        }
        */

        
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
