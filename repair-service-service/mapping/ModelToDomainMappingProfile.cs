using AutoMapper;
using MongoDB.Bson;
using repair.service.data.entities;
using repair.service.service.model;

namespace repair.service.service.mapping
{
    public class ModelToDomainMappingProfile : Profile
    {
        public ModelToDomainMappingProfile()
        {
            CreateMap<TDocumentModel, TDocument>();           
            CreateMap<RepairRequestModel, RepairRequest>();
            CreateMap<NotificationModel, Notification>();
            CreateMap<DetailModel, Detail>();
            CreateMap<CommentModel, Comment>();
            CreateMap<SectionModel, Section>();
            CreateMap<RepairInvoiceModel, RepairInvoice>();
            CreateMap<RepairInvoiceStageModel, RepairInvoiceStage>();
            CreateMap<RepairInvoiceSectionModel, RepairInvoiceSection>();
            CreateMap<LineModel, Line>();
            CreateMap<LineItemModel, LineItem>();
            CreateMap<RepairRequestStageModel, RepairRequestStage>()            
                .ForMember(dest => dest.ResponseData, experssion => experssion.MapFrom(src => BsonDocument.Parse(src.ResponseData.ToString())));
        }
    }
}
