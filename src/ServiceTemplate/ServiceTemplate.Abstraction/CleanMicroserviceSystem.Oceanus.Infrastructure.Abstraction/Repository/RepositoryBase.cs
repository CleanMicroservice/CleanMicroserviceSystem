﻿using System.Linq.Expressions;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
where TEntity : class
{
    protected readonly ILogger<RepositoryBase<TEntity>> logger;
    protected readonly DbContext dbContext;

    public RepositoryBase(
        ILogger<RepositoryBase<TEntity>> logger,
        DbContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.logger.LogDebug($"Create data service: {GetType().FullName} ({GetHashCode():X})");
    }

    #region Add

    /// <summary>
    /// Add entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        var result = await dbContext.Set<TEntity>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Add entities
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public virtual async Task<int> AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await dbContext.Set<TEntity>().AddRangeAsync(entities);
        var result = await dbContext.SaveChangesAsync();
        return result;
    }
    #endregion

    #region Update

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var result = dbContext.Set<TEntity>().Update(entity);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Update entities
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public virtual async Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        dbContext.Set<TEntity>().UpdateRange(entities);
        var results = await dbContext.SaveChangesAsync();
        return results;
    }
    #endregion

    #region Delete

    /// <summary>
    /// Remove entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<TEntity> RemoveAsync(TEntity entity)
    {
        var result = dbContext.Set<TEntity>().Remove(entity);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Remove entities
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public virtual async Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        dbContext.Set<TEntity>().RemoveRange(entities);
        var results = await dbContext.SaveChangesAsync();
        return results;
    }
    #endregion

    #region Load

    /// <summary>
    /// Load
    /// </summary>
    public virtual void Load()
        => dbContext.Set<TEntity>().Load();

    /// <summary>
    /// Load
    /// </summary>
    /// <returns></returns>
    public virtual Task LoadAsync()
        => dbContext.Set<TEntity>().LoadAsync();
    #endregion

    #region For each

    /// <summary>
    /// All
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<bool> AllAsync(Expression<Func<TEntity, bool>> expression)
        => dbContext.Set<TEntity>().AllAsync(expression);

    /// <summary>
    /// Any
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        => dbContext.Set<TEntity>().AnyAsync(expression);

    /// <summary>
    /// For each
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public virtual Task ForEachAsync(Action<TEntity> action)
        => dbContext.Set<TEntity>().ForEachAsync(action);
    #endregion

    #region Extremum

    /// <summary>
    /// Max
    /// </summary>
    /// <typeparam name="TResult">Type</typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> expression)
        => dbContext.Set<TEntity>().MaxAsync(expression);

    /// <summary>
    /// Min
    /// </summary>
    /// <typeparam name="TResult">Type</typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> expression)
        => dbContext.Set<TEntity>().MinAsync(expression);
    #endregion

    #region Count

    /// <summary>
    /// Count
    /// </summary>
    /// <returns></returns>
    public virtual Task<int> CountAsync()
        => dbContext.Set<TEntity>().CountAsync();

    /// <summary>
    /// Count
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
        => dbContext.Set<TEntity>().CountAsync(expression);
    #endregion

    #region Contain

    /// <summary>
    /// Contain
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual Task<bool> ContainsAsync(TEntity entity)
        => dbContext.Set<TEntity>().ContainsAsync(entity);
    #endregion

    #region Search by primary key

    /// <summary>
    /// Find
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    /// <remarks>Find method will try to search object in memory first, and search in database when not exist in memory</remarks>
    public virtual TEntity? Find(params object[] keys)
        => dbContext.Set<TEntity>().Find(keys);

    /// <summary>
    /// Find
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public virtual Task<TEntity?> FindAsync(params object[] keys)
        => dbContext.Set<TEntity>().FindAsync(keys).AsTask();
    #endregion

    #region First

    /// <summary>
    /// First
    /// </summary>
    /// <returns></returns>
    public virtual Task<TEntity> FirstAsync()
        => dbContext.Set<TEntity>().FirstAsync();

    /// <summary>
    /// First
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression)
        => dbContext.Set<TEntity>().FirstAsync(expression);

    /// <summary>
    /// First or default
    /// </summary>
    /// <returns></returns>
    public virtual Task<TEntity?> FirstOrDefaultAsync()
        => dbContext.Set<TEntity>().FirstOrDefaultAsync();

    /// <summary>
    /// First or default
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        => dbContext.Set<TEntity>().FirstOrDefaultAsync(expression);

    /// <summary>
    /// Single
    /// </summary>
    /// <returns></returns>
    public virtual Task<TEntity> SingleAsync()
        => dbContext.Set<TEntity>().SingleAsync();

    /// <summary>
    /// Single
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression)
        => dbContext.Set<TEntity>().SingleAsync(expression);

    /// <summary>
    /// Single or default
    /// </summary>
    /// <returns></returns>
    public virtual Task<TEntity?> SingleOrDefaultAsync()
        => dbContext.Set<TEntity>().SingleOrDefaultAsync();

    /// <summary>
    /// Single or default
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        => dbContext.Set<TEntity>().FirstOrDefaultAsync(expression);
    #endregion

    #region Collection cast

    /// <summary>
    /// Return IQueryable
    /// </summary>
    /// <returns></returns>
    public virtual IQueryable<TEntity> AsQueryable()
        => dbContext.Set<TEntity>().AsQueryable();

    /// <summary>
    /// Return IEnumerable
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<TEntity> AsEnumerable()
        => dbContext.Set<TEntity>().AsEnumerable();

    /// <summary>
    /// Return ParallelQuery
    /// </summary>
    /// <returns></returns>
    public virtual ParallelQuery<TEntity> AsParallel()
        => dbContext.Set<TEntity>().AsParallel();

    /// <summary>
    /// Return Array
    /// </summary>
    /// <returns></returns>
    public virtual Task<TEntity[]> ToArrayAsync()
        => dbContext.Set<TEntity>().ToArrayAsync();

    /// <summary>
    /// Return Dictionary
    /// </summary>
    /// <typeparam name="TKey">Type of Key</typeparam>
    /// <param name="keySelector"></param>
    /// <returns></returns>
    public virtual Task<Dictionary<TKey, TEntity>> ToDictionaryAsync<TKey>(Func<TEntity, TKey> keySelector)
        where TKey : notnull
        => dbContext.Set<TEntity>().ToDictionaryAsync(keySelector);

    /// <summary>
    /// Return List
    /// </summary>
    /// <returns></returns>
    public virtual Task<List<TEntity>> ToListAsync()
        => dbContext.Set<TEntity>().ToListAsync();
    #endregion

    #region Skip and take

    /// <summary>
    /// Skip
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public virtual IQueryable<TEntity> Skip(int count)
        => dbContext.Set<TEntity>().Skip(count);

    /// <summary>
    /// Take
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public virtual IQueryable<TEntity> Take(int count)
        => dbContext.Set<TEntity>().Take(count);
    #endregion

    #region Sum

    /// <summary>
    /// Sum
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> expression)
        => dbContext.Set<TEntity>().SumAsync(expression);

    /// <summary>
    /// Sum
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<decimal?> SumAsync(Expression<Func<TEntity, decimal?>> expression)
        => dbContext.Set<TEntity>().SumAsync(expression);

    /// <summary>
    /// Sum
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<double> SumAsync(Expression<Func<TEntity, double>> expression)
        => dbContext.Set<TEntity>().SumAsync(expression);

    /// <summary>
    /// Sum
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<double?> SumAsync(Expression<Func<TEntity, double?>> expression)
        => dbContext.Set<TEntity>().SumAsync(expression);

    /// <summary>
    /// Sum
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<float> SumAsync(Expression<Func<TEntity, float>> expression)
        => dbContext.Set<TEntity>().SumAsync(expression);

    /// <summary>
    /// Sum
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<float?> SumAsync(Expression<Func<TEntity, float?>> expression)
        => dbContext.Set<TEntity>().SumAsync(expression);

    /// <summary>
    /// Sum
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<long> SumAsync(Expression<Func<TEntity, long>> expression)
        => dbContext.Set<TEntity>().SumAsync(expression);

    /// <summary>
    /// Sum
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<long?> SumAsync(Expression<Func<TEntity, long?>> expression)
        => dbContext.Set<TEntity>().SumAsync(expression);

    /// <summary>
    /// Sum
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<int> SumAsync(Expression<Func<TEntity, int>> expression)
        => dbContext.Set<TEntity>().SumAsync(expression);

    /// <summary>
    /// Sum
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<int?> SumAsync(Expression<Func<TEntity, int?>> expression)
        => dbContext.Set<TEntity>().SumAsync(expression);
    #endregion

    #region Average

    /// <summary>
    /// Average
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<decimal> AverageAsync(Expression<Func<TEntity, decimal>> expression)
        => dbContext.Set<TEntity>().AverageAsync(expression);

    /// <summary>
    /// Average
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<decimal?> AverageAsync(Expression<Func<TEntity, decimal?>> expression)
        => dbContext.Set<TEntity>().AverageAsync(expression);

    /// <summary>
    /// Average
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<double> AverageAsync(Expression<Func<TEntity, double>> expression)
        => dbContext.Set<TEntity>().AverageAsync(expression);

    /// <summary>
    /// Average
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<double?> AverageAsync(Expression<Func<TEntity, double?>> expression)
        => dbContext.Set<TEntity>().AverageAsync(expression);

    /// <summary>
    /// Average
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<float> AverageAsync(Expression<Func<TEntity, float>> expression)
        => dbContext.Set<TEntity>().AverageAsync(expression);

    /// <summary>
    /// Average
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<float?> AverageAsync(Expression<Func<TEntity, float?>> expression)
        => dbContext.Set<TEntity>().AverageAsync(expression);

    /// <summary>
    /// Average
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<double> AverageAsync(Expression<Func<TEntity, long>> expression)
        => dbContext.Set<TEntity>().AverageAsync(expression);

    /// <summary>
    /// Average
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<double?> AverageAsync(Expression<Func<TEntity, long?>> expression)
        => dbContext.Set<TEntity>().AverageAsync(expression);

    /// <summary>
    /// Average
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<double> AverageAsync(Expression<Func<TEntity, int>> expression)
        => dbContext.Set<TEntity>().AverageAsync(expression);

    /// <summary>
    /// Average
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual Task<double?> AverageAsync(Expression<Func<TEntity, int?>> expression)
        => dbContext.Set<TEntity>().AverageAsync(expression);
    #endregion

    #region Save

    /// <summary>
    /// Save changes
    /// </summary>
    /// <returns></returns>
    public int SaveChanges()
        => dbContext.SaveChanges();

    /// <summary>
    /// Save changes
    /// </summary>
    /// <returns></returns>
    public Task<int> SaveChangesAsync()
        => dbContext.SaveChangesAsync();
    #endregion

    #region Transact

    /// <summary>
    /// Execute transact
    /// </summary>
    /// <typeparam name="TDelegate">Type of delegate</typeparam>
    /// <typeparam name="TResult">Type of result</typeparam>
    /// <param name="delegate"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public virtual TResult? Transact<TDelegate, TResult>(TDelegate @delegate, params object[] parameters)
        where TDelegate : Delegate
    {
        using var transaction = dbContext.Database.BeginTransaction();
        try
        {
            var result = (TResult?)@delegate.DynamicInvoke(parameters);

            dbContext.SaveChanges();
            transaction.Commit();

            return result;
        }
        catch
        {
            transaction.Rollback();

            throw;
        }
    }
    #endregion
}
