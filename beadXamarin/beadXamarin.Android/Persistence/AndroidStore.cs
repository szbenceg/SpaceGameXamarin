using beadXamarin.Droid.Persistence;
using beadXamarin.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidStore))]
namespace beadXamarin.Droid.Persistence
{
    /// <summary>
    /// J�t�k t�rol� megval�s�t�sa Android platformra.
    /// </summary>
    public class AndroidStore : IStore
    {
        /// <summary>
        /// F�jlok lek�rdez�se.
        /// </summary>
        /// <returns>A f�jlok list�ja.</returns>
        public async Task<IEnumerable<String>> GetFiles()
        {
            return await Task.Run(() => Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).Select(file => Path.GetFileName(file)));
        }

        /// <summary>
        /// M�dos�t�s idej�nek lekr�dez�se.
        /// </summary>
        /// <param name="name">A f�jl neve.</param>
        /// <returns>Az utols� m�dos�t�s ideje.</returns>
        public async Task<DateTime> GetModifiedTime(String name)
        {
            FileInfo info = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), name));

            return await Task.Run(() => info.LastWriteTime);
        }
    }
}