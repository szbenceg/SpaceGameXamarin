using beadXamarin.Droid.Persistence;
using beadXamarin.Model;
using beadXamarin.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDataAccess))]
namespace beadXamarin.Droid.Persistence
{
    public class AndroidDataAccess : IGameDataAccess
    {
        
        public async Task<SpaceWord> LoadAsync(String path)
        {
            try
            {

                var picker = new Windows.Storage.Pickers.FileOpenPicker();
                picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".txt");

                Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
                String fileContent = string.Empty; ;
                if (file != null) {
                    fileContent = await Windows.Storage.FileIO.ReadTextAsync(file);
                }

                return Newtonsoft.Json.JsonConvert.DeserializeObject<SpaceWord>(fileContent);
            }
            catch
            {
                throw new GameDataException();
            }
        }
        public async Task SaveAsync(SpaceWord spaceWord)
        {
            try
            {

                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                // Dropdown of file types the user can save the file as
                savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
                // Default file name if the user does not type one in or select a file to replace
                savePicker.SuggestedFileName = "New Document";

                Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(spaceWord);
                    await Windows.Storage.FileIO.WriteTextAsync(file, data);
                }

            }
            catch
            {
                throw new GameDataException();
            }

            //return null;

            //String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);

            //await Task.Run(() => File.WriteAllText(filePath, text));
        }

    }
}