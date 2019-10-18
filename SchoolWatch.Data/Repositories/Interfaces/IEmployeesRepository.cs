using SchoolWatch.Data.Entities;

namespace SchoolWatch.Data.Repositories.Interfaces
{
    public interface IEmployeesRepository
    {
        EmployeeEntity[] GetAll();
    }
}