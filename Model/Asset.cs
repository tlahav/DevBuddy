using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DevBuddy.Model
{
    public enum CMDCommand
    {
        copy,
        delete,
        md,
        iisreset
    }
    public class Asset
    {
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public CMDCommand Command { get; set; }
        public string strDevEnvFile { get; set; }
        public string strStageEnvFile { get; set; }
        public bool isEnabled { get; set; }

        public Asset() 
        {
            this.AssetId = 0;
            this.AssetName = "";
            this.Command = CMDCommand.copy;
            this.strDevEnvFile = "";
            this.strStageEnvFile = "";
            this.isEnabled = true;

        }
        public Asset(string AssetName)
        {

            this.AssetId = 0;
            this.AssetName = AssetName;
            
        }

        public Dictionary<string, string> GetAttributesDictionary()
        {
            var dicAttributes = new Dictionary<string, string>();
            dicAttributes.Add("AssetId", this.AssetId.ToString());
            dicAttributes.Add("AssetName", this.AssetName);
            dicAttributes.Add("DevEnvFile", this.strDevEnvFile);
            dicAttributes.Add("StageEnvFile", this.strStageEnvFile);
            dicAttributes.Add("StageEnvFile", this.strStageEnvFile);
            return dicAttributes;
        }
    }
}
