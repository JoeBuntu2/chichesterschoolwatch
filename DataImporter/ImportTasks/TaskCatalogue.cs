using System.Linq;

namespace DataImporter.ImportTasks
{
    public class TaskCatalogue
    {
        public ITask[] GetTasks()
        {
            return GetType().Assembly.DefinedTypes
                .Where(x => x.ImplementedInterfaces.Contains(typeof(ITask)))
                .Select(x => x.GetConstructor(System.Type.EmptyTypes).Invoke(null))
                .Cast<ITask>()
                .ToArray();
                
        }
    }
}
