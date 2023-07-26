using AutoMapper;
using MongoDB.Bson.Serialization;
using repair.service.data.entities;
using repair.service.service.model;
using repair.service.shared.paging;
using System.Collections.Generic;

namespace repair.service.service.mapping
{
    public class DomainToModelMappingProfile : Profile
    {
        public DomainToModelMappingProfile()
        {
            CreateMap<TDocument, TDocumentModel>();          
            CreateMap<RepairRequest, RepairRequestModel>();
            CreateMap<Notification, NotificationModel>();
            CreateMap<Detail, DetailModel>();
            CreateMap<Comment, CommentModel>();
            CreateMap<Section, SectionModel>();
            CreateMap<RepairInvoice, RepairInvoiceModel>();
            CreateMap<RepairInvoiceStage, RepairInvoiceStageModel>();
            CreateMap<RepairInvoiceSection, RepairInvoiceSectionModel>();
            CreateMap<Line, LineModel>();
            CreateMap<LineItem, LineItemModel>();
            CreateMap<PagedResponseList<RepairRequest>, PagedResponseList<RepairRequestModel>>();
            CreateMap<PagingInfo<RepairRequest>, PagingInfo<RepairRequestModel>>();
            CreateMap<RepairRequestStage, RepairRequestStageModel>()
                .ForMember(dest => dest.ResponseData, experssion => experssion.MapFrom(src => BsonSerializer.Deserialize<object>(src.ResponseData,null)));
            CreateMap<PagedResponseList<RepairInvoiceStage>, PagedResponseList<RepairInvoiceStageModel>>();
            CreateMap<PagingInfo<RepairInvoiceStage>, PagingInfo<RepairInvoiceStageModel>>();

        }
    }
}
