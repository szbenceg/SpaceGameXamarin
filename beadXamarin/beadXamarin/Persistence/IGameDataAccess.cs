using beadXamarin.Model;
using System.Threading.Tasks;

namespace beadXamarin.Persistence
{
    public interface IGameDataAccess
    {
        Task<SpaceWord> LoadAsync(string path);
        Task SaveAsync(SpaceWord table);
    }
}