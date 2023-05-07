using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FoodCourt.Core.ExtensionMethods
{
    public enum MessageType
    {
        Success,
        Error,
        Warning,
        Info
    }
    public static class TempDataExtensionMethods
    {

        public static void SetMessage(this ITempDataDictionary tempData, MessageType type, string message, string? title = null)
        {
            switch(type)
            {
                case MessageType.Success:
                    tempData["Success"] = message;
                    break;
                case MessageType.Error:
                    tempData["Error"] = message;
                    break;
                case MessageType.Warning:
                    tempData["Warning"] = message;
                    break;
                case MessageType.Info:
                    tempData["Info"] = message;
                    break;
                default:
                    break;
            }
            if(title != null) 
            {
                tempData["Title"] = title;
            }
        }


    }
}
