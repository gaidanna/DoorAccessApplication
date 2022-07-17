namespace DoorAccessApplication.Ids.Interfaces
{
    public interface IRedirectService
    {
        string ExtractRedirectUriFromReturnUrl(string url);
    }
}
