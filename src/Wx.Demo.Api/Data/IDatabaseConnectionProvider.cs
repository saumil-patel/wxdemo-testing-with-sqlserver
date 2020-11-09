using System.Data;
using System.Threading.Tasks;

namespace Wx.Demo.Api.Data
{
    public interface IDatabaseConnectionProvider
    {
        Task<IDbConnection> GetOpenConnection();
    }
}