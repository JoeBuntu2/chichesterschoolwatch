using System.Collections.Generic;
using System.Linq;
using SchoolWatch.Business.DTO.Revenue;
using SchoolWatch.Business.Interface;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Business
{
    public class RevenueCodesService : IRevenueCodesService
    {
        private readonly IRevenuesRepository RevenuesRepository;

        public RevenueCodesService(IRevenuesRepository revenuesRepository)
        {
            RevenuesRepository = revenuesRepository;
        }

        public AllRevenueCodesDto GetAll()
        {
            var revenueCodeEntities = RevenuesRepository.GetAll();

            var functionCodes = revenueCodeEntities.Select(x => new RevenueCodeDto
            {
                RevenueCodeId = x.RevenueId,
                Level = x.Level,
                Description = x.Description
            }).ToDictionary(x => x.RevenueCodeId);

            var dto = new AllRevenueCodesDto
            {
                FunctionCodes = functionCodes,
                Levels = new Dictionary<string, string>
                {
                    { "L", "Local" },
                    { "S", "State" },
                    { "F", "Federal" },
                    { "O", "Other" }
                }
            };

            return dto;
        }
    }
}