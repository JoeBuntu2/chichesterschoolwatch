using System.Linq;
using SchoolWatch.Business.Interface;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Business
{
    public class DistrictsService : IDistrictsService
    { 
        private readonly IDistrictsRepository DistrictsRepository;

        public DistrictsService(IDistrictsRepository districtsRepository)
        {
            DistrictsRepository = districtsRepository;
        }

        public DistrictsDto Get(int districtId)
        {
            var entity = DistrictsRepository.Get(districtId);

            return new DistrictsDto
            {
                DistrictId = entity.DistrictId,
                Name = entity.Name
            };
        }

        public DistrictsDto[] GetAll()
        {
            return DistrictsRepository.GetAll().Select(x => new DistrictsDto
            {
                DistrictId = x.DistrictId,
                Name = x.Name
            }).ToArray();
        }
    }
}