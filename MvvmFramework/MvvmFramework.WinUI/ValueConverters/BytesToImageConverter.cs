using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

namespace MvvmFramework.WinUI;

public class BytesToImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is not byte[] bytes)
        {
            return value;
        }

        using var ms = new InMemoryRandomAccessStream();
        using var writer = new DataWriter(ms.GetOutputStreamAt(0));
        writer.WriteBytes(bytes);
        writer.StoreAsync().GetResults();

        var image = new BitmapImage();
        image.SetSource(ms);
        return image;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
