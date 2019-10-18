using SchoolWatch.Data.Entities;

namespace SchoolWatch.Business.Interface
{
    public interface IEmployeesService
    {
        EmployeeEntity[] GetAll();
    }
}
