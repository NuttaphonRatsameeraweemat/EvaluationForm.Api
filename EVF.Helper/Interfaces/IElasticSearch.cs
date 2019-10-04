using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Helper.Interfaces
{
    public interface IElasticSearch<T> where T : class
    {
        /// <summary>
        /// Get elastic search client.
        /// </summary>
        /// <returns></returns>
        ElasticClient GetClient();
        /// <summary>
        /// Get data from elastic search.
        /// </summary>
        /// <param name="searchFunc">The search descriptor and query container for filter.</param>
        /// <returns></returns>
        IEnumerable<T> SearchFilter(Func<SearchDescriptor<T>, ISearchRequest> searchFunc);
        /// <summary>
        /// Insert to elastic search.
        /// </summary>
        /// <param name="model">The infomation data.</param>
        /// <param name="index">The index elastic search.</param>
        /// <param name="type">The type elastic search.</param>
        /// <returns></returns>
        string Insert(T model, string index, string type);
        /// <summary>
        /// Update to elastic search.
        /// </summary>
        /// <param name="model">The infomation data.</param>
        /// <param name="index">The index elastic search.</param>
        /// <param name="type">The type elastic search.</param>
        /// <returns></returns>
        string Update(T model, string index, string type);
        /// <summary>
        /// Delete item in elastic search.
        /// </summary>
        /// <param name="id">The identity key.</param>
        /// <param name="index">The index elastic search.</param>
        /// <param name="type">The type elastic search.</param>
        /// <returns></returns>
        string Delete(int id, string index, string type);
        /// <summary>
        /// Delete index in elastic search.
        /// </summary>
        /// <param name="index">The identity index.</param>
        /// <param name="type">The type elastic search.</param>
        /// <returns></returns>
        string DeleteAll(string index, string type);
        /// <summary>
        /// Bulk to elastic search.
        /// </summary>
        /// <param name="modelList">The Generic list data.</param>
        /// <param name="index">The identity index.</param>
        /// <param name="type">The type elastic search.</param>
        /// <returns></returns>
        string Bulk(List<T> modelList, string index, string type);
    }
}
