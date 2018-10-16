using DAL.UnitOfWork;

namespace DAL.Factory
{
    public interface IFactory
    {
        IUnityOfWork GetUOF();
    }
}
