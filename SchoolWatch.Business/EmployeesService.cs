using SchoolWatch.Business.Interface;
using SchoolWatch.Data;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Business
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeesRepository EmployeesRepository;

        public EmployeesService(IEmployeesRepository employeesRepository)
        {
            EmployeesRepository = employeesRepository;
        }

        public EmployeeEntity[] GetAll()
        {
            return EmployeesRepository.GetAll();
        }
    }
}
