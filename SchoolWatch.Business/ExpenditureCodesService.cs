using System.Linq;
using SchoolWatch.Business.DTO;
using SchoolWatch.Business.Interface;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Business
{
    public class ExpenditureCodesService : IExpenditureCodesService
    {
        private readonly IExpenditureCodesRepository ExpenditureCodesRepository;

        public ExpenditureCodesService(IExpenditureCodesRepository expenditureCodesRepository)
        {
            ExpenditureCodesRepository = expenditureCodesRepository;
        }

        public AllExpenditureCodesDto GetAll()
        {
            var top = GetTopLevelCodes();
            var mid = GetMidLevelCodes();
            var code = GetCodeLevelCodes();

            return new AllExpenditureCodesDto
            {
                TopLevelCodes = top.ToDictionary(x => x.Code),
                MidLevelCodes = mid.ToDictionary(x => x.Code),
                CodeLevelCodes = code.ToDictionary(x => x.Code)
            };
        }

        public BudgetCodeDto[] GetTopLevelCodes()
        {
            return ExpenditureCodesRepository.GetTopLevelCodes()
                .Select(x => new BudgetCodeDto {Level = "Top", Code = x.TopLevelId, Name = x.Description})
                .ToArray();
        }

        public BudgetCodeDto[] GetMidLevelCodes()
        {
            return ExpenditureCodesRepository.GetMidLevelCodes()
                .Select(x => new BudgetCodeDto {Level = "Mid", Code = x.MidLevelId, Name = x.Description})
                .ToArray();
        }

        public BudgetCodeDto[] GetCodeLevelCodes()
        {
            return ExpenditureCodesRepository.GetCodeLevelCodes()
                .Select(x => new BudgetCodeDto {Level = "Top", Code = x.Code, Name = x.Description})
                .ToArray();
        }
    }
}