using repair.service.data.entities;
using repair.service.service.model;
using repair.service.shared.model;
using System.Net.Http;
using System.Threading.Tasks;

namespace repair.service.service.abstracts
{
    public interface IRepairInvoiceService : IBaseService<RepairInvoiceModel, RepairInvoice>
    {
        Task<RepairInvoiceModel> FindOneAsync(int orderId,string accountId);

        Task<RepairInvoiceModel> InsertOneAsync(RepairInvoiceModel model, JwtInfo jwtInfo);

        Task<HttpResponseMessage> SaveOnPremAsync(RepairInvoiceModel model, JwtInfo jwtInfo);

    }
}
