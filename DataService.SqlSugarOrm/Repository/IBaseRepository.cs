using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataService.SqlSugarOrm
{
    /// <summary>
    /// 基类接口,其他接口继承该接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 查询全部
        /// </summary>
        Task<List<TEntity>> GetListAsync();
        /// <summary>
        /// id查询
        /// </summary>
        Task<TEntity> GetByIdAsync(object objId);
        /// <summary>
        /// id查询
        /// </summary>
        Task<TEntity> GetByIdExpMapperAsync(object objId, Action<TEntity> mapper);
        /// <summary>
        /// 条件查询
        /// </summary>
        Task<List<TEntity>> GetByExpAsync(Expression<Func<TEntity, bool>> exp);
        /// <summary>
        /// 条件统计
        /// </summary>
        Task<int> CoutByExpAsync(Expression<Func<TEntity, bool>> exp);
        /// <summary>
        /// 根据条件查询指定列
        /// </summary>
        Task<List<T>> GetColumnByExpAsync<T>(Expression<Func<TEntity, bool>> wexp, Expression<Func<TEntity, T>> sexp);
        /// <summary>
        /// 条件导航查询
        /// </summary>
        Task<List<TEntity>> GetByExpMapperAsync(Expression<Func<TEntity, bool>> exp, Action<TEntity> mapper);
        /// <summary>
        /// 条件导航查询
        /// </summary>
        Task<List<TEntity>> GetByExpIncludAsync<TReturn1>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn1>> include);
        /// <summary>
        /// 条件导航查询2
        /// </summary>
        /// <typeparam name="TReturn1"></typeparam>
        /// <typeparam name="TReturn2"></typeparam>
        /// <param name="exp"></param>
        /// <param name="include1"></param>
        /// <param name="include2"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetByExpIncludAsync<TReturn1, TReturn2>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn1>> include1, Expression<Func<TReturn1, List<TReturn2>>> include2);
        /// <summary>
        /// 条件导航查询3
        /// </summary>
        /// <typeparam name="TReturn1"></typeparam>
        /// <typeparam name="TReturn2"></typeparam>
        /// <param name="exp"></param>
        /// <param name="include1"></param>
        /// <param name="include2"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetByExpIncludAsync<TReturn1, TReturn2, TReturn3>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn1>> include1, Expression<Func<TReturn1, List<TReturn2>>> include2, Expression<Func<TReturn2, TReturn3>> include3);
        /// <summary>
        /// 条件导航查询2
        /// </summary>
        Task<List<TEntity>> GetByExpInclud2Async<TReturn1, TReturn2>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn1>> include, Expression<Func<TEntity, TReturn2>> include2);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="exp">查询条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="page_index">起始页</param>
        /// <param name="page_size">每页数量</param>
        /// <returns></returns>
        Task<(List<TEntity>, int)> GetByPageAsync(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, object>> order, int page_index, int page_size);
        /// <summary>
        /// 分页导航查询
        /// </summary>
        /// <param name="exp">查询条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="mapper">关联数据</param>
        /// <param name="page_index">起始页</param>
        /// <param name="page_size">每页数量</param>
        /// <returns></returns>
        Task<(List<TEntity>, int)> GetByPageMapperAsync(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, object>> order, Action<TEntity> mapper, int page_index, int page_size);
        /// <summary>
        /// 分页导航查询
        /// </summary>
        /// <param name="exp">查询条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="mapper">关联数据</param>
        /// <param name="page_index">起始页</param>
        /// <param name="page_size">每页数量</param>
        /// <returns></returns>
        Task<(List<T>, int)> GetByPageMapperAsync<T>(Expression<Func<T, bool>> exp, Expression<Func<T, object>> order, Action<T> mapper, int page_index, int page_size);
        /// <summary>
        /// 添加
        /// </summary>
        Task<bool> AddAsync(TEntity model);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> AddReturnIdAsync(TEntity model);
        /// <summary>
        /// 批量添加
        /// </summary>
        Task<bool> AddManyAsync(List<TEntity> list);
        /// <summary>
        /// 修改
        /// </summary>
        Task<bool> UpdateAsync(TEntity model);        
        /// <summary>
        /// 修改部分字段
        /// </summary>
        Task<bool> UpdateColumnsAsync(TEntity model,string[] columns);
        /// <summary>
        /// 批量更新实体数据
        /// </summary>
        Task<bool> UpdateManyAsync(List<TEntity> list);
        /// <summary>
        /// 批量删除
        /// </summary>
        Task<bool> DeleteByIdsAsync(int[] ids);
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByExpAsync(Expression<Func<TEntity, bool>> exp);
        /// <summary>
        /// 删除
        /// </summary>
        Task<bool> DeleteByIdAsync(object id);
        /// <summary>
        /// 获取一个公共主键
        /// </summary>
        /// <returns></returns>
        int GetId(string seq);
        /// <summary>
        /// 获取一个公共主键
        /// </summary>
        Task<int> GetIdAsync(string seq);
        /// <summary>
        /// 根据条件逆序排序获取第一条
        /// </summary>
        Task<T> GetFirstAsync<T>(Expression<Func<T, bool>> exp, Expression<Func<T, object>> order);
        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<bool> Exist<T>(Expression<Func<T, bool>> exp);
    }
}
