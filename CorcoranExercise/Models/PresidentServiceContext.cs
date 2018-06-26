using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Web.Hosting;

namespace CorcoranExercise.Models
{
    public class PresidentServiceContext
    {
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET";

        static string ClientSecret = HostingEnvironment.ApplicationPhysicalPath + "bin\\Utilities\\client_secret.json";

        static readonly string SpreedsheetId = "1i2qbKeasPptIrY1PkFVjbHSrLtKEPIIwES6m2l2Mdd8";
        const string Range = "Sheet1!A2:E45";

        public ValueRange result = null;

        public PresidentServiceContext()
        {
            var credential = GetSheetCredentials();

            var service = GetService(credential);

            result = GetData(service, Range, SpreedsheetId);
        }

        private static UserCredential GetSheetCredentials()
        {
            using (var stream = new FileStream(ClientSecret, FileMode.Open, FileAccess.Read))
            {
                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore("sheetsCredentials")).Result;
            }
        }

        private static SheetsService GetService(UserCredential credential)
        {
            return new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }

        private static ValueRange GetData(SheetsService service, string range, string spreedsheetId)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreedsheetId, range);
            ValueRange response = request.Execute();

            return response;
        }
    }
}