using Core.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using RestSharp.Deserializers;
using System.Text;
using System.Web.Configuration;
using System.ComponentModel;

namespace Core.Resources
{
    public class Languages
    {
        private readonly GeneralDb _db = new GeneralDb();
        private static RestClient _client = new RestClient("https://www.googleapis.com/language/translate/v2");
        private string _key;
        public bool? LargeQuery { get; set; }
        public bool? PrettyPrint { get; set; }

        public string Translate(string phrase, string langF, string lanfT)
        {
            try
            {
                if (langF.ToUpper().Equals(lanfT.ToUpper()))
                    return phrase;
                var word = GetfromDb(phrase, lanfT);
                if (!string.IsNullOrEmpty(word)) return word;
                return GetfromTr(phrase, langF, lanfT);
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        private string GetfromDb(string phrase, string lanfT)
        {
            try
            {
                var tb = ExistsTable(lanfT);
                if (!tb) return "";
                var query = @"select top (1) a.Lang_Translation as TTranslation from " + lanfT + @" as a
                            where a.Lang_Key = '" + phrase + "'";
                var exOne = _db.Database.SqlQuery<string>(query).SingleOrDefault();
                return exOne;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        private string GetfromTr(string phrase, string langF, string lanfT)
        {
            try
            {
                _key = WebConfigurationManager.AppSettings["GoogleTkey"];
                var result = FinalTranslate(FindLanguage(langF), FindLanguage(lanfT), phrase);
                SaveInDb(phrase, result.FirstOrDefault().TranslatedText, lanfT);
                return result.FirstOrDefault().TranslatedText;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        private GLanguages FindLanguage(string lang)
        {
            if (lang == "en") return GLanguages.English;
            var type = typeof(GLanguages);
            string filter = "";
            foreach (var item in Enum.GetNames(type))
            {
                var memInfo = type.GetMember(item.ToString());
                var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description = ((DescriptionAttribute)attributes[0]).Description;
                if (lang.Equals(description))
                {
                    filter = item;
                    break;
                }
            }
            if (string.IsNullOrEmpty(filter))
                return GLanguages.English;
            var rtn = Enum.GetValues(typeof(GLanguages)).Cast<GLanguages>().Where(x => x.ToString() == filter).FirstOrDefault();
            if (rtn == null)
                return GLanguages.English;
            return rtn;
        }

        private bool ExistsTable(string dbName)
        {
            try
            {
                var queryDb = @"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = '" + dbName + "')) BEGIN select 'true' END else BEGIN select 'false' END";
                var exOne = _db.Database.SqlQuery<string>(queryDb).SingleOrDefault();
                if (Convert.ToBoolean(exOne)) return true;
                queryDb = "CREATE TABLE " + dbName + @" (
                           LangDescr_id int identity primary key,
                           Lang_Key nvarchar(1500) not null,
                           Lang_Translation nvarchar(1500) not null);";
                _db.Database.ExecuteSqlCommand(queryDb);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void SaveInDb(string phrase, string translated, string lanfT)
        {
            try
            {
                var queryDb = "INSERT INTO " + lanfT + @" VALUES ('" + phrase + "','" + translated + "');";
                _db.Database.ExecuteSqlCommand(queryDb);
            }
            catch (Exception e)
            {
                //
            }
        }

        private RestRequest CreateRequest(string function)
        {
            RestRequest request;
            if (LargeQuery.HasValue && LargeQuery.Value)
            {
                request = new RestRequest(function, Method.POST);
                request.AddHeader("X-HTTP-Method-Override", "GET");
            }
            else request = new RestRequest(function, Method.GET);
            request.AddParameter("key", _key);
            if (PrettyPrint.HasValue)
                request.AddParameter("prettyprint", PrettyPrint.ToString().ToLower());
            return request;
        }

        public List<Translation> FinalTranslate(GLanguages sourceLanguage, GLanguages destinationLanaguage, params string[] text)
        {
            var request = CreateRequest(string.Empty);
            CheckRequest(text);
            foreach (string q in text) request.AddParameter("q", q);
            request.AddParameter("target", destinationLanaguage.GetStringValue());
            request.AddParameter("source", sourceLanguage.GetStringValue());
            var results = GetResponse<TranslateResult>(request);
            return results.Data.Translations;
        }

        private void CheckRequest(IEnumerable<string> requestContent)
        {
            var sum = requestContent.Sum(item => item.Length);

            if (((LargeQuery.HasValue && !LargeQuery.Value) || !LargeQuery.HasValue) && sum >= 2000)
                throw new ArgumentException("Your text content is larger than 2000 characters. Set LargeQuery to 'true' to enable support up to 5000 characters.");
            if (sum > 5000) throw new ArgumentException("Your text content is larger than 5000 characters. Google Translate only allow up to 5000 characters");
        }

        private T GetResponse<T>(RestRequest request)
        {
            var response = (RestResponse)_client.Execute(request);
            var deserializer = new JsonDeserializer();
            var results = deserializer.Deserialize<T>(response);
            var errorResponse = deserializer.Deserialize<ErrorResponse>(response);
            if (errorResponse.Error != null) throw new Exception(GetErrorText(errorResponse.Error));
            return results;
        }

        private string GetErrorText(GError error)
        {
            if (error != null)
            {
                var sb = new StringBuilder();
                sb.Append(error.Message);
                if (error.Errors.Count >= 1)
                {
                    ErrorData errorData = error.Errors.First();
                    sb.Append("Reason: " + errorData.Reason);
                }
                return sb.ToString();
            }
            return "There was an error. Unable to determine the cause.";
        }

        public void UpdateLanguages()
        {
            var query = "exec sp_GetLanguages";
            var lst = _db.Database.SqlQuery<LotusViewModels.General.Languages>(query).ToList();
            var type = typeof(GLanguages);
            foreach (var item in Enum.GetNames(type))
            {
                var memInfo = type.GetMember(item.ToString());
                var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description = ((DescriptionAttribute)attributes[0]).Description;
                if (!lst.Any(x => x.LanguageShort.ToUpper().Equals(description.ToUpper())))
                {
                    /*_db.languages.Add(new languages
                    {
                        lang_description = item,
                        lang_short = description
                    });
                    _db.SaveChanges();*/
                }
            }
        }
    }

    public enum GLanguages
    {
        [StringValue("af")]
        [Description("af")]
        Afrikaans,
        [StringValue("sq")]
        [Description("sq")]
        Albanian,
        [StringValue("ar")]
        [Description("ar")]
        Arabic,
        [StringValue("be")]
        [Description("be")]
        Belarusian,
        [StringValue("bg")]
        [Description("bg")]
        Bulgarian,
        [StringValue("ca")]
        [Description("ca")]
        Catalan,
        [StringValue("zh")]
        [Description("zh")]
        ChineseSimplified,
        [StringValue("zh-TW")]
        [Description("zh-TW")]
        ChineseTraditional,
        [StringValue("hr")]
        [Description("hr")]
        Croatian,
        [StringValue("cs")]
        [Description("cs")]
        Czech,
        [StringValue("da")]
        [Description("da")]
        Danish,
        [StringValue("nl")]
        [Description("nl")]
        Dutch,
        [StringValue("en")]
        [Description("en")]
        English,
        [StringValue("eo")]
        [Description("eo")]
        Esperanto,
        [StringValue("et")]
        [Description("et")]
        Estonian,
        [StringValue("tl")]
        [Description("tl")]
        Filipino,
        [StringValue("fi")]
        [Description("fi")]
        Finnish,
        [StringValue("fr")]
        [Description("fr")]
        French,
        [StringValue("gl")]
        [Description("gl")]
        Galician,
        [StringValue("de")]
        [Description("de")]
        German,
        [StringValue("el")]
        [Description("el")]
        Greek,
        [StringValue("ht")]
        [Description("ht")]
        HaitianCreole,
        [StringValue("iw")]
        [Description("iw")]
        Hebrew,
        [StringValue("hi")]
        [Description("hi")]
        Hindi,
        [StringValue("hu")]
        [Description("hu")]
        Hungarian,
        [StringValue("is")]
        [Description("is")]
        Icelandic,
        [StringValue("id")]
        [Description("id")]
        Indonesian,
        [StringValue("ga")]
        [Description("ga")]
        Irish,
        [StringValue("it")]
        [Description("it")]
        Italian,
        [StringValue("ja")]
        [Description("ja")]
        Japanese,
        [StringValue("ko")]
        [Description("ko")]
        Korean,
        [StringValue("lv")]
        [Description("lv")]
        Latvian,
        [StringValue("lt")]
        [Description("lt")]
        Lithuanian,
        [StringValue("mk")]
        [Description("mk")]
        Macedonian,
        [StringValue("ms")]
        [Description("ms")]
        Malay,
        [StringValue("mt")]
        [Description("mt")]
        Maltese,
        [StringValue("no")]
        [Description("no")]
        Norwegian,
        [StringValue("fa")]
        [Description("fa")]
        Persian,
        [StringValue("pl")]
        [Description("pl")]
        Polish,
        [StringValue("pt")]
        [Description("pt")]
        Portuguese,
        [StringValue("ro")]
        [Description("ro")]
        Romanian,
        [StringValue("ru")]
        [Description("ru")]
        Russian,
        [StringValue("sr")]
        [Description("sr")]
        Serbian,
        [StringValue("sk")]
        [Description("sk")]
        Slovak,
        [StringValue("sl")]
        [Description("sl")]
        Slovenian,
        [StringValue("es")]
        [Description("es")]
        Spanish,
        [StringValue("sw")]
        [Description("sw")]
        Swahili,
        [StringValue("sv")]
        [Description("sv")]
        Swedish,
        [StringValue("th")]
        [Description("th")]
        Thai,
        [StringValue("tr")]
        [Description("tr")]
        Turkish,
        [StringValue("uk")]
        [Description("uk")]
        Ukrainian,
        [StringValue("vi")]
        [Description("vi")]
        Vietnamese,
        [StringValue("cy")]
        [Description("cy")]
        Welsh,
        [StringValue("yi")]
        [Description("yi")]
        Yiddish
    }

    public class StringValueAttribute : Attribute
    {
        public string StringValue { get; private set; }

        public StringValueAttribute(string value)
        {
            StringValue = value;
        }
    }

    public class Translation
    {
        public string TranslatedText { get; set; }
        public string DetectedSourceLanguage { get; set; }
    }

    public static class ExtensionMethods
    {
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            var fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
    }

    public class TranslateResult
    {
        public TranslationData Data { get; set; }
    }

    public class ErrorResponse
    {
        public GError Error { get; set; }
    }

    public class GError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<ErrorData> Errors { get; set; }
    }

    public class ErrorData
    {
        public string Domain { get; set; }
        public string Reason { get; set; }
        public string Message { get; set; }
        public string LocationType { get; set; }
        public string Location { get; set; }
    }

    public class TranslationData
    {
        public List<Translation> Translations { get; set; }
    }
}