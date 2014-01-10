using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace DevBuddy.Model
{
    public class ProjectModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }

        //[XmlAttributeAttribute(DataType = "date", AttributeName="ProjectStart")]
        public DateTime ProjectStart{ get; set; }

        //[XmlAttributeAttribute(  DataType = "date" , AttributeName="Projectend")]
        public DateTime ProjectEnd { get; set; }
        
        
        public bool isEnabled { get; set; }
        public bool isVisible { get; set; }

       
        public List<Asset> AttachedAssets { get; set; }
        public string AssociatedTask { get; set; }

        public string ProjectDocumentFolder { get; set; }

        public ProjectModel() { AttachedAssets = new List<Asset>(); }
        
        public ProjectModel(string strProjectName)
        {
            var rndMizer = new Random(3);

            ProjectId = rndMizer.Next(100);
            ProjectName = strProjectName;
            ProjectStart = DateTime.Now;
            isEnabled = true;
            isVisible = true;
            AttachedAssets = new List<Asset>();
            AssociatedTask = "";
            ProjectDocumentFolder = "";
        }

        public bool SaveProject()
        {

            try
            {
                StreamWriter x = new StreamWriter("c:\\save.txt", true);
                x.WriteLine(this.ProjectName);
                x.WriteLine(this.ProjectStart.ToString());
                x.WriteLine(this.ProjectEnd.ToString());
                x.WriteLine(this.isEnabled.ToString());
                x.WriteLine(this.isVisible.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public List<string> GetAttributesAsStrings()
        {
            var liAttributes = new List<string>();
            liAttributes.Add(this.ProjectId.ToString());
            liAttributes.Add(this.ProjectName);
            liAttributes.Add(this.ProjectStart.ToString());
            liAttributes.Add(this.ProjectEnd.ToString());
            liAttributes.Add(this.isEnabled.ToString());
            liAttributes.Add(this.isVisible.ToString());
            return liAttributes;
        }

        public Dictionary<string, string> GetAttributesDictionary()
        {
            var liAttributes = new Dictionary<string,string>();
            liAttributes.Add("ProjectId",this.ProjectId.ToString());
            liAttributes.Add("ProjectName", this.ProjectName);
            liAttributes.Add("ProjectStart",this.ProjectStart.ToString());
            liAttributes.Add("ProjectEnd" ,this.ProjectEnd.ToString());
            liAttributes.Add("isEnabled",this.isEnabled.ToString());
            liAttributes.Add("isVisible", this.isVisible.ToString());
            return liAttributes;
        }

        public void AddAsset(Asset assetNew)
        {
            this.AttachedAssets.Add(assetNew);
        }

        public List<Dictionary<string,string>> GetAssetsDictionariessAsList()
        {
            var dicAssetAttributes = new Dictionary<string,string>();
            var liAssetsDictionary = new List<Dictionary<string, string>>();
            foreach (var x in this.AttachedAssets)
            {
                dicAssetAttributes = x.GetAttributesDictionary();
                liAssetsDictionary.Add(dicAssetAttributes);
            }
            return liAssetsDictionary;
        }
    }
}
