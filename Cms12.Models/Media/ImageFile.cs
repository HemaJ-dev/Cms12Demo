using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace Cms12.Models.Media
{
    [ContentType(DisplayName = "ImageFile", GUID = "d8aefbbd-49d6-4ee0-9930-b6e1b4568d6e")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png")]
    public class ImageFile : ImageData
    {
    }
}
