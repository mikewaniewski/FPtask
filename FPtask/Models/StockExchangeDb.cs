using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FPtask.Models
{


    public interface IStockExchangeDb : IDisposable
    {
        //this interface defines methods that will be used for accessing and updating database
        IQueryable<T> Query<T>() where T : class;
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;



        ApplicationDbContext appDbContext { get; set; }



        void SaveChanges();
        Task<int> SaveChangesAsync();
    }




    public class StockExchangeDb : DbContext, IStockExchangeDb
    {   //this class implements IStockExchangeDb interface, and then 
        //provides access to data from database through DataSet objects
        //it's purpose is to allow unit testing if needed.

        public StockExchangeDb()
            : base("name=DefaultConnection")
        {

        }

  



        ApplicationDbContext x = new ApplicationDbContext();

        public ApplicationDbContext appDbContext
        {

            get
            {
                return x;
            }

            set { }

        }


        public DbSet<Shares> StockShares { get; set; }



        IQueryable<T> IStockExchangeDb.Query<T>()
        {
            return Set<T>();
        }

        void IStockExchangeDb.Add<T>(T entity)
        {
            Set<T>().Add(entity);
        }

        void IStockExchangeDb.Update<T>(T entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        void IStockExchangeDb.Remove<T>(T entity)
        {
            Set<T>().Remove(entity);
        }

        void IStockExchangeDb.SaveChanges()
        {
            SaveChanges();
        }

        Task<int> IStockExchangeDb.SaveChangesAsync()
        {

            return x.SaveChangesAsync();

        }

        public System.Data.Entity.DbSet<FPtask.Models.Operation> StockOperations { get; set; }

    }
}