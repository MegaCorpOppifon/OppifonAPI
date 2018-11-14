using DAL.Data;
using DAL.Factory;
using DAL.UnitOfWork;

namespace OppifonAPI
{
    public class Factory : IFactory
    {
        public static string ConnectionString { get; set; }
        
        public IUnitOfWork GetUOF()
        {
            return new UnitOfWork(new Context(ConnectionString));
        }
    }
}
