using OpcClientUa.Models;
using System.Threading.Tasks;

namespace OpcClientUa.SignalR
{
    public interface IOpcClient
    {
        /// <summary>
        /// OPC数据更新
        /// </summary>
        /// <param name="opcItem"></param>
        /// <returns></returns>
        Task OpcDataUpdate(OpcItem opcItem);
    }
}
