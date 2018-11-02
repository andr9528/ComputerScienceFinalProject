using System.Collections.Generic;

namespace Repository.Core
{
    public interface IRepository
    {
        T Find<T>(T predicate) where T : class, IEntity;
        ICollection<T> FindMultiple<T>(T predicate) where T : class, IEntity;
        bool Update<T>(T predicate, bool resetRepo = true, bool autoSave = true) where T : class, IEntity;
        bool Delete<T>(T predicate, bool resetRepo = true, bool autoSave = true) where T : class, IEntity;
        bool Add<T>(T element, bool autoSave = false) where T : class, IEntity;
        string AddRange<T>(ICollection<T> elements, bool autoSave = true) where T : class, IEntity;
        void Save();
        void ResetRepo();
    }
}