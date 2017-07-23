using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.General
{
    [DataContract]
    public class ParametrosAplicacion
    {
        [DataMember]
        public Guid AppKey { get; set; }
        [DataMember]
        public string AppName { get; set; }
        [DataMember]
        public string UrlLogo { get; set; }
        [DataMember]
        public string UrlContrato { get; set; }
        [DataMember]
        public string UrlCarnet { get; set; }
        [DataMember]
        public string UrlZendesk { get; set; }
        [DataMember]
        public string ColorMarco { get; set; }
        [DataMember]
        public string ApiFace { get; set; }
        [DataMember]
        public string VersionFace { get; set; }
        [DataMember]
        public string AppKeyStorm { get; set; }
        [DataMember]
        public string AppSecretStorm { get; set; }
        [DataMember]
        public string UrlImgCarnet { get; set; }
        [DataMember]
        public Nullable<int> CodPlan { get; set; }
        [DataMember]
        public string CodigoVendedor { get; set; }
        [DataMember]
        public string UserZendesk { get; set; }
        [DataMember]
        public string PasswordZendesk { get; set; }
        [DataMember]
        public string ApiTokenZendesk { get; set; }
        [DataMember]
        public string AuthorIdZendesk { get; set; }
        [DataMember]
        public string OrganizationIdZendesk { get; set; }
        [DataMember]
        public string GroupIdZendesk { get; set; }
        [DataMember]
        public string CustomFieldIdZendesk { get; set; }
    }
}
