namespace XmasDev.Loader.Common
{
    public class ConfigurationHelper
    {
        public static string ConnectionString
        {
            get
            {
                return "AuthType=Office365;Username=santaclaus@xmasdev.onmicrosoft.com;Password=@1Giancarlo1@;Url=https://xmasdev.crm4.dynamics.com";
            }
        }

        public static string StorageConnection
        {
            get
            {
                return "DefaultEndpointsProtocol=https;AccountName=xmasdevmltnjw7m4tl2x5ost;AccountKey=xQpJZqrtuHsH7WJaC41fOAKH7koYcEL8ts9izRQwER2i1bKPbnZtUKeT2AoV/xxXI5iGOJAbonhFoCI5q7fNhQ==;EndpointSuffix=core.windows.net";
            }
        }

        public static string ServiceBusConnection
        {
            get
            {
                return "Endpoint=sb://xmasdev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=U4tQszrzYD3hNmMFOnFF+axlRONi04yI/iLE8no+kt4=";
            }
        }
    }
}
