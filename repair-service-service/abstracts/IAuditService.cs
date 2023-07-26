using repair.service.service.model;
using repair.service.shared.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace repair.service.service.abstracts
{
    public interface IAuditService
    {
         Task<HttpResponseMessage> PostAuditAsync(RepairRequestModel repairRequestModel, JwtInfo jwtInfo);
    }
}
