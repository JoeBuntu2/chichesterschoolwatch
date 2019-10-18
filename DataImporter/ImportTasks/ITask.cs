using System; 

namespace DataImporter.ImportTasks
{
    public interface ITask
    {
        void Run(IServiceProvider service);
    }
}
