using AudioPlayerWCF.Responses;
using System.ServiceModel;


namespace AudioPlayerWCF
{
    // ПРИМЕЧАНИЕ. Можно использовать команду "Переименовать" в меню "Рефакторинг", чтобы изменить имя интерфейса "IAudioPlayerService" в коде и файле конфигурации.
    [ServiceContract]
    public interface IAudioPlayerService
    {
        [OperationContract]
        AudioResponse GetAudios(int page, int limit);

        [OperationContract]
        int GetSum(int page, int limit);
    }
}
