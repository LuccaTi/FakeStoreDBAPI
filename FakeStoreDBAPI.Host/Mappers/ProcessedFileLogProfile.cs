using AutoMapper;
using FakeStoreDBAPI.Host.DTO.ProcessedFileLog;
using FakeStoreDBAPI.Host.Models;

namespace FakeStoreDBAPI.Host.Mappers
{
    public class ProcessedFileLogProfile : Profile
    {
        public ProcessedFileLogProfile()
        {
            CreateMap<CreateProcessedFileLogDto, ProcessedFileLog>();
            CreateMap<ProcessedFileLog, ProcessedFileLogDto>();
        }
    }
}
