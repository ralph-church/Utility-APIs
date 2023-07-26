using AutoMapper;

namespace repair.service.service.mapping
{
    public class AutoMapperConfiguration
    {
        public static IMapper Configure()
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DomainToModelMappingProfile>();
                cfg.AddProfile<ModelToDomainMappingProfile>();
            });

            return config.CreateMapper();
        }
    }
}
