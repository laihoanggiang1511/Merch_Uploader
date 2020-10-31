using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace Common.Update
{
    public class UpdateHelper
    {
        private UpdateModel updateModel;
        public UpdateModel UpdateModel { get; set; }

        public Version LocalVersion { get; set; }

        public UpdateHelper(string localVersion)
        {
            Version version = null;
            Version.TryParse(localVersion, out version);
            LocalVersion = version;
        }

        public void GetUpdate()
        {
            UpdateModel result = null;
            string url = string.Format("");
            var client = new RestClient(url);
            client.Timeout = 5000;
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JsonDeserializer deserializer = new JsonDeserializer();
                result = deserializer.Deserialize<UpdateModel>(response);
            }

            UpdateModel = result;
        }
        public bool IsThereNewUpdate()
        {
            if (UpdateModel != null && UpdateModel.ServerVersion != null)
            {
                Version serverVersion;
                if (Version.TryParse(UpdateModel.ServerVersion, out serverVersion))
                {
                    return serverVersion.CompareTo(LocalVersion) > 0;
                }
            }
            return false;
        }
        public bool DownloadUpdate()
        {
            return true;
        }
    }
}
