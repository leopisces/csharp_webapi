using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataService.SqlSugarOrm
{
    public abstract class BaseRepository<TEntity> : SugarDbContext, IBaseRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseRepository(IOptions<SugarOption> options, ILogger logger) : base(options, logger)
        {
        }

        #region 查找

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListAsync()
        {
            using var dbContext = Instance;
            return await dbContext.Queryable<TEntity>().ToListAsync();
        }


        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public async Task<TEntity> GetByIdAsync(object objId)
        {
            using var dbContext = Instance;
            return await dbContext.Queryable<TEntity>().InSingleAsync(objId);
        }

        public async Task<TEntity> GetByIdExpMapperAsync(object objId, Action<TEntity> mapper)
        {
            using var dbContext = Instance;
            return await dbContext.Queryable<TEntity>()
                                  .Mapper(mapper)
                                  .InSingleAsync(objId);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        public async Task<List<TEntity>> GetByExpAsync(Expression<Func<TEntity, bool>> exp)
        {
            using var dbContext = Instance;
            return await dbContext.Queryable<TEntity>().Where(exp).ToListAsync();
        }

        /// <summary>
        /// 根据条件统计
        /// </summary>
        public async Task<int> CoutByExpAsync(Expression<Func<TEntity, bool>> exp)
        {
            using var dbContext = Instance;
            return await dbContext.Queryable<TEntity>().Where(exp).CountAsync();
        }

        /// <summary>
        /// 根据条件查询指定列
        /// </summary>
        public async Task<List<T>> GetColumnByExpAsync<T>(
            Expression<Func<TEntity, bool>> wexp, Expression<Func<TEntity, T>> sexp)
        {
            using var dbContext = Instance;
            return await dbContext.Queryable<TEntity>().Where(wexp).Select(sexp).ToListAsync();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        public async Task<List<TEntity>> GetByExpMapperAsync(Expression<Func<TEntity, bool>> exp, Action<TEntity> mapper)
        {
            using var dbContext = Instance;
            return await dbContext.Queryable<TEntity>()
                            .WhereIF(exp != null, exp)
                            .Mapper(mapper)
                            .ToListAsync();
        }

        /// <summary>
        /// 根据条件查询1
        /// </summary>
        public async Task<List<TEntity>> GetByExpIncludAsync<TReturn1>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn1>> include)
        {
            using var dbContext = Instance;
            return await dbContext.Queryable<TEntity>()
                            .WhereIF(exp != null, exp)
                            .Includes(include)
                            .ToListAsync();
        }

        /// <summary>
        /// 根据条件查询2
        /// </summary>
        public async Task<List<TEntity>> GetByExpIncludAsync<TReturn1, TReturn2>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn1>> include1, Expression<Func<TReturn1, List<TReturn2>>> include2)
        {
            using var dbContext = Instance;
            return await dbContext.Queryable<TEntity>()
                            .WhereIF(exp != null, exp)
                            .Includes(include1, include2)
                            .ToListAsync();
        }

        public async Task<List<TEntity>> GetByExpIncludAsync<TReturn1, TReturn2, TReturn3>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn1>> include1, Expression<Func<TReturn1, List<TReturn2>>> include2, Expression<Func<TReturn2, TReturn3>> include3)
        {
            using var dbContext = Instance;
            return await dbContext.Queryable<TEntity>()
                            .WhereIF(exp != null, exp)
                            .Includes(include1, include2, include3)
                            .ToListAsync();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        public async Task<List<TEntity>> GetByExpInclud2Async<TReturn1, TReturn2>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn1>> include, Expression<Func<TEntity, TReturn2>> include2)
        {
            using var dbContext = Instance;
            return await dbContext.Queryable<TEntity>()
                            .WhereIF(exp != null, exp)
                            .Includes(include)
                            .Includes(include2)
                            .ToListAsync();
        }

        public async Task<(List<TEntity>, int)> GetByPageAsync(
            Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, object>> order, int page_index, int page_size)
        {
            using var dbContext = Instance;
            RefAsync<int> totalCount = 0;
            var query = await dbContext.Queryable<TEntity>()
                                .WhereIF(exp != null, exp)
                                .OrderByIF(order != null, order, OrderByType.Desc)
                                .ToPageListAsync(page_index, page_size, totalCount);
            return (query, totalCount);
        }

        public async Task<(List<TEntity>, int)> GetByPageMapperAsync(
            Expression<Func<TEntity, bool>> exp,
            Expression<Func<TEntity, object>> order,
            Action<TEntity> mapper,
            int page_index, int page_size)
        {
            using var dbContext = Instance;
            RefAsync<int> totalCount = 0;
            var query = await dbContext.Queryable<TEntity>()
                                .WhereIF(exp != null, exp)
                                .Mapper(mapper)
                                .OrderByIF(order != null, order, OrderByType.Desc)
                                .ToPageListAsync(page_index, page_size, totalCount);
            return (query, totalCount);
        }

        /// <summary>
        /// 分页导航查询
        /// </summary>
        /// <param name="exp">查询条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="mapper">关联数据</param>
        /// <param name="page_index">起始页</param>
        /// <param name="page_size">每页数量</param>
        /// <returns></returns>
        public async Task<(List<T>, int)> GetByPageMapperAsync<T>(
            Expression<Func<T, bool>> exp, Expression<Func<T, object>> order, Action<T> mapper, int page_index, int page_size)
        {
            using var dbContext = Instance;
            RefAsync<int> totalCount = 0;
            var query = await dbContext.Queryable<T>()
                                .WhereIF(exp != null, exp)
                                .Mapper(mapper)
                                .OrderByIF(order != null, order, OrderByType.Desc)
                                .ToPageListAsync(page_index, page_size, totalCount);
            return (query, totalCount);
        }

        /// <summary>
        /// 逆序排序获取第一条
        /// </summary>
        /// <param name="exp">查询条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="mapper">关联数据</param>
        /// <returns></returns>
        public async Task<T> GetFirstAsync<T>(
            Expression<Func<T, bool>> exp, Expression<Func<T, object>> order)
        {
            using var dbContext = Instance;
            RefAsync<int> totalCount = 0;
            var info = await dbContext.Queryable<T>()
                                .WhereIF(exp != null, exp)
                                .OrderByIF(order != null, order, OrderByType.Desc)
                                .FirstAsync();
            return info;
        }

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<bool> Exist<T>(Expression<Func<T, bool>> exp)
        {
            using var dbContext = Instance;
            var exist = await dbContext.Queryable<T>()
                                .WhereIF(exp != null, exp)
                                .AnyAsync();
            return exist;
        }

        /// <summary>
        /// 获取一个公共主键
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="seq"></param>
        /// <returns></returns>
        public int GetId(string seq)
        {
            using var dbContext = Instance;
            var i = dbContext.Ado.SqlQuerySingle<int>($"SELECT {seq}.nextval FROM DUAL");
            return i;
        }

        /// <summary>
        /// 获取一个公共主键
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="seq"></param>
        /// <returns></returns>
        public async Task<int> GetIdAsync(string seq)
        {
            using var dbContext = Instance;
            var i = await dbContext.Ado.SqlQuerySingleAsync<int>($"SELECT {seq}.nextval FROM DUAL");
            return i;
        }

        #endregion


        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(TEntity model)
        {
            using var dbContext = Instance;
            var i = await dbContext.Insertable(model).ExecuteReturnIdentityAsync();
            //返回的i是long类型,这里你可以根据你的业务需要进行处理
            return i > 0;
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> AddReturnIdAsync(TEntity model)
        {
            using var dbContext = Instance;
            var i = await dbContext.Insertable(model).ExecuteReturnIdentityAsync();
            //返回的i是long类型,这里你可以根据你的业务需要进行处理
            return i;
        }

        /// <summary>
        /// 批量写入实体数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddManyAsync(List<TEntity> list)
        {
            using var dbContext = Instance;
            var i = await dbContext.Insertable(list).ExecuteReturnBigIdentityAsync();
            //返回的i是long类型,这里你可以根据你的业务需要进行处理
            return i > 0;
        }

        /// <summary>
        /// 根据ID批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(int[] ids)
        {
            using var dbContext = Instance;
            var i = await dbContext.Deleteable<TEntity>().In(ids).ExecuteCommandAsync();
            return i > 0;
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdAsync(object id)
        {
            using var dbContext = Instance;
            var i = await dbContext.Deleteable<TEntity>().In(id).ExecuteCommandAsync();
            return i > 0;
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByExpAsync(Expression<Func<TEntity, bool>> exp)
        {
            using var dbContext = Instance;
            var i = await dbContext.Deleteable<TEntity>()
                            .Where(exp)
                            .ExecuteCommandAsync();
            return i > 0;
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(TEntity model)
        {
            using var dbContext = Instance;
            //这种方式会以主键为条件
            var i = await dbContext.Updateable(model).ExecuteCommandAsync();
            return i > 0;
        }

        /// <summary>
        /// 批量更新实体数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateManyAsync(List<TEntity> list)
        {
            using var dbContext = Instance;
            var i = await dbContext.Updateable(list).ExecuteCommandAsync();
            //返回的i是long类型,这里你可以根据你的业务需要进行处理
            return i > 0;
        }

        /// <summary>
        /// 更新字段
        /// </summary>
        /// <param name="model"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public async Task<bool> UpdateColumnsAsync(TEntity model, string[] columns)
        {
            using var dbContext = Instance;
            var i = await dbContext.Updateable(model).UpdateColumnsIF(columns.Length > 0, columns).ExecuteCommandAsync();
            return i > 0;
        }
    }
}
