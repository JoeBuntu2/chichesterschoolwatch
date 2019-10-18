namespace SchoolWatch.Business.Interface
{
    public interface IDistrictsService
    {
        DistrictsDto[] GetAll();
        DistrictsDto Get(int districtId);
    }
}