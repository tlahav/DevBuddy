using DevBuddy.Model;
using DevBuddy.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;

namespace DevBuddy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public string fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DevBuddy.xml");
        public string fileName;
        public string settingsFileName = System.IO.Path.Combine("c:\\DevBuddySettings.xml");
        public List<ProjectModel> liProjects = new List<ProjectModel>();
        public Settings settings = new Settings();
        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists(settingsFileName))
            {
                settings = ObjectXMLSerializer<Settings>.Load(settingsFileName);
                if (settings != null)
                {
                    bool success = InitializeSettings();
                    if(!success)
                        System.Windows.MessageBox.Show("Settings File Not Found. Unable to load settings", "Settings File Not Found!");


                }
            }
            if (txtWorkPortfolioFilePath.Text != "")
            {
                fileName = txtWorkPortfolioFilePath.Text;
                btnRefresh_Click(new object(),new RoutedEventArgs());
            }
            else
            {
                lblTitle.Content = "Please select a Work Portfolio File first";
            }

            if(File.Exists(fileName))
            {
                liProjects = ObjectXMLSerializer<List<ProjectModel>>.Load(fileName);
                InitiateGrid();
            }
        
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //demo data . overrights real data so ahh do not use :D 
        {
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    //lblLabel.Content = fileName.ToString();
                    ProjectModel x = new ProjectModel("Monkey" + i.ToString());

                    //fake data:
                    Asset assetNew = new Asset("asset1");
                    assetNew.Command = CMDCommand.copy;
                    assetNew.strDevEnvFile = "c:\\asdf";
                    assetNew.strStageEnvFile = "d:\\asdf";
                    assetNew.isEnabled = false;
                    x.AddAsset(assetNew);

                    Asset assetNew2 = new Asset("asset2");
                    assetNew2.Command = CMDCommand.delete;
                    assetNew2.strDevEnvFile = "c:\\asdf34234";
                    assetNew2.strStageEnvFile = "d:\\asdf234234";
                    assetNew2.isEnabled = true;
                    x.AddAsset(assetNew2);

                    liProjects.Add(x);
                }
            }
            catch (Exception ex)
            {
                lblLabel.Content = ex.Message;
                throw;
            }


        }
        private void InitiateGrid()
        { 
            btnRefresh_Click(new object(), new RoutedEventArgs());
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    //List<ProjectModel> liProjects = new List<ProjectModel>();
                    liProjects = ObjectXMLSerializer<List<ProjectModel>>.Load(fileName);


                    dataGridTest.ItemsSource = liProjects;

                    lblLabel.Content = liProjects.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                lblLabel.Content = ex.Message;
                throw;
            }
            
        }



        private void dataGridTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count < 0)
                {
                    if (e.AddedItems[0].GetType().Name == "ProjectModel")
                    {
                        ProjectModel project = (ProjectModel)e.AddedItems[0];
                        if (project.isVisible)
                            ((System.Windows.Controls.DataGrid)(sender)).RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
                        else
                            ((System.Windows.Controls.DataGrid)(sender)).RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                lblLabel.Content = ex.Message;
                throw;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ObjectXMLSerializer<List<ProjectModel>>.Save(liProjects, fileName);
            }
            catch (Exception ex)
            {
                lblLabel.Content = ex.Message;
                throw;
            }
        }

        private void dataGridTest_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            ProjectModel x = (ProjectModel)e.Row.Item;
          
                //foreach(Asset asset in x.AttachedAssets)
                //{
                //    asset.isEnabled = x.isEnabled;
                //}

                try
                {
                    ObjectXMLSerializer<List<ProjectModel>>.Save(liProjects, fileName);
                }
                catch (Exception ex)
                {
                    lblLabel.Content = ex.Message;
                    throw;
                }
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            foreach (ProjectModel project in liProjects)
            {
                if (project.isEnabled)
                {
                    foreach (Asset asset in project.AttachedAssets)
                    {
                        if (asset.isEnabled)
                        {
                            string command = asset.Command.ToString();
                            string strFileFrom = asset.strDevEnvFile;
                            string strFileTo = asset.strStageEnvFile;

                            //lblLabel.Content = @"/C " + command + " " + strFileFrom + " " + strFileTo;
                            var logItem = new ListBoxItem();
                            logItem.Content = @"/C " + command + " " + strFileFrom + " " + strFileTo;
                            LogList.Items.Add(logItem);

                            Process.Start("cmd.exe", @"/C " + command + " \"" + strFileFrom + "\" \"" + strFileTo+"\"");
                            ProcessStartInfo startinfo = new ProcessStartInfo();
                            
                        }
                    }
                }
            }
        }

        private void dataGridTest_AutoGeneratedColumns(object sender, EventArgs e)
        {
            string hi;
            hi = "asdf";
        }

        private void DataGrid_AutoGeneratedColumns_1(object sender, EventArgs e)
        {
            string hi;
            hi = "afd:";
        }

        private void DataGrid_BeginningEdit_1(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                string filepath = "";
                Asset asset;

                //can fall here probably:
                if (e.Row.Item != null)
                    asset = (Asset)e.Row.Item;
                else
                    asset = new Asset();



                switch ((e.Column).DisplayIndex)
                {
                    case 3:
                        if ((asset.Command != CMDCommand.iisreset) || asset.Command == CMDCommand.md)
                        {
                            var fd = new System.Windows.Forms.OpenFileDialog();
                            //if (fd.ShowDialog() == true)
                            if (!string.IsNullOrEmpty(asset.strDevEnvFile))
                                fd.InitialDirectory = System.IO.Path.GetDirectoryName(asset.strDevEnvFile);
                            else if (!string.IsNullOrEmpty(settings.DefaultWorkFolder))
                                fd.InitialDirectory = settings.DefaultWorkFolder;
                            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {

                                filepath = fd.FileName;
                                asset.strDevEnvFile = filepath;
                            }
                        }
                        else
                        {
                            asset.strDevEnvFile = "";
                        }
                        //e.Cancel = true;
                        SaveAndRefresh();
                        break;
                    case 4:
                        if ((asset.Command != CMDCommand.iisreset))
                        {
                            FolderBrowserDialog dest = new FolderBrowserDialog();
                            if (!string.IsNullOrEmpty(asset.strStageEnvFile))
                                dest.SelectedPath = asset.strStageEnvFile;
                            else if (!string.IsNullOrEmpty(settings.DefaultWorkFolder))
                                dest.SelectedPath = System.IO.Path.GetDirectoryName(settings.DefaultWorkFolder);

                            if (dest.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                filepath = dest.SelectedPath;
                                asset.strStageEnvFile = filepath;
                            }
                        }
                        else
                        {
                            asset.strStageEnvFile = "";
                        }
                        SaveAndRefresh();
                        break;

                    default:
                        SaveAndRefresh();
                        break;


                }
            }
            catch (Exception ex)
            {
                lblLabel.Content = ex.Message;
                throw;
            }

        }
    private void SaveAndRefresh()
    {
        try
        {
            
            //dataGridTest.ItemsSource = liProjects;

            dataGridTest.CommitEdit(DataGridEditingUnit.Cell, true);
            //dataGridTest.Items.Refresh();
            //ObjectXMLSerializer<List<ProjectModel>>.Save(liProjects, fileName);
           
        }
        catch (Exception ex)
        {
            lblLabel.Content = ex.Message;
            throw;
        }
      
    }

    private void dataGridTest_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
        try
        {
            string filepath = "";
            ProjectModel project = (ProjectModel)e.Row.Item;

            switch (e.Column.DisplayIndex)
            {
                case 7:
                    FolderBrowserDialog dest = new FolderBrowserDialog();
                    if (!string.IsNullOrEmpty(settings.DefaultDocFolder))
                        dest.SelectedPath = System.IO.Path.GetDirectoryName(settings.DefaultDocFolder);

                    if (dest.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        filepath = dest.SelectedPath;
                        project.ProjectDocumentFolder = filepath;
                    }
                    break;

                case 8:
                    if (!string.IsNullOrEmpty(project.ProjectDocumentFolder))
                        Process.Start(@project.ProjectDocumentFolder);
                    e.Cancel = true;
                    break;
            }
        }
        catch (Exception ex)
        {
            lblLabel.Content = ex.Message;  
            throw;
        }
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        try
        {
            FolderBrowserDialog dest = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(txtDocFolder.Text))
            {
                dest.SelectedPath = txtDocFolder.Text;
            }
            if (dest.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                settings.DefaultDocFolder = dest.SelectedPath;
                txtDocFolder.Text = dest.SelectedPath;
                ObjectXMLSerializer<Settings>.Save(settings, settingsFileName);
            }

        }
        catch (Exception ex)
        {
            lblLabel.Content = ex.Message;
            throw;
        }


    }

    private bool InitializeSettings()
    {
        try
        {
            if (settings != null)
            {
                if (string.IsNullOrEmpty(settings.DefaultDocFolder))
                    txtDocFolder.Text = "";
                else
                    txtDocFolder.Text = settings.DefaultDocFolder;

                if (string.IsNullOrEmpty(settings.DefaultWorkFolder))
                    txtWorkFolder.Text = "";
                else
                    txtWorkFolder.Text = settings.DefaultWorkFolder;
                if (string.IsNullOrEmpty(settings.LastPortfolioFilePath))
                    txtWorkPortfolioFilePath.Text = "";
                else
                    txtWorkPortfolioFilePath.Text = settings.LastPortfolioFilePath;

                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            lblLabel.Content = ex.Message;
            return false;
        }
    }

    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
        try
        {
            FolderBrowserDialog dest = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(txtWorkFolder.Text))
            {
                dest.SelectedPath = txtWorkFolder.Text;
            }
            if (dest.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                settings.DefaultWorkFolder = dest.SelectedPath;
                txtWorkFolder.Text = dest.SelectedPath;
                ObjectXMLSerializer<Settings>.Save(settings, settingsFileName);
            }
        }
        catch (Exception ex)
        {
            lblLabel.Content = ex.Message;
            throw;
        }
    }

    private void btnOpenDocFolder_Click_1(object sender, RoutedEventArgs e)
    {
        try
        {
            string hi = e.ToString();
            lblLabel.Content = hi;
        }
        catch (Exception ex)
        {
            lblLabel.Content = ex.Message;
            throw;
        }
    }

    private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
    {
        try
        {
            Hyperlink hyper = (Hyperlink)sender;
            string strPath = hyper.NavigateUri.ToString();

            if (!string.IsNullOrEmpty(strPath))
                System.Diagnostics.Process.Start("explorer.exe", @hyper.NavigateUri.ToString());
            //Process.Start(@hyper.NavigateUri.AbsolutePath);
        }
        catch (Exception)
        {
            lblLabel.Content = "Error opening document path";
        }


    }
    private void OpenNetworkPath(string uncPath)
    {
        
    }

    private void Button_Click_4(object sender, RoutedEventArgs e)
    {

    }


    private void btnBrowsePortfolioFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var fd = new System.Windows.Forms.OpenFileDialog();
            //if (fd.ShowDialog() == true)


            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                txtWorkPortfolioFilePath.Text = fd.FileName;
                fileName = fd.FileName;
                liProjects = ObjectXMLSerializer<List<ProjectModel>>.Load(fileName);
                InitiateGrid();
                lblTitle.Content = "Portfolio file Loaded: " + fd.SafeFileName;
                settings.LastPortfolioFilePath = fileName;
                ObjectXMLSerializer<Settings>.Save(settings, settingsFileName);
            }
        }
        catch (Exception ex)
        {
            lblLabel.Content = ex.Message;
            throw;
        }
    }

    private void btnNewFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var fd = new System.Windows.Forms.SaveFileDialog();
            fd.Filter = "XML documents (.XML)|*.XML";
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(fd.FileName))
                {
                    File.Delete(fd.FileName);
                }
                var file = File.Create(fd.FileName);
                file.Close();
                file = null;

                List<ProjectModel> projNewEmpty = CreateEmptyProject();
                ObjectXMLSerializer<List<ProjectModel>>.Save(projNewEmpty, fd.FileName);
                txtWorkPortfolioFilePath.Text = fd.FileName;
                liProjects = projNewEmpty;
                fileName = fd.FileName;
                InitiateGrid();

            }
        }
        catch (Exception ex)
        {
            lblLabel.Content = ex.Message;
            throw;
        }
   
    }

    private List<ProjectModel> CreateEmptyProject()
    {
        try
        {
            var liNewList = new List<ProjectModel>();
            for (int i = 0; i < 2; i++)
            {
                //lblLabel.Content = fileName.ToString();
                ProjectModel x = new ProjectModel("New" + i.ToString());

                //fake data:
                Asset assetNew = new Asset("New Asset1");
                assetNew.Command = CMDCommand.copy;
                assetNew.strDevEnvFile = "c:\\asdf";
                assetNew.strStageEnvFile = "d:\\asdf";
                assetNew.isEnabled = false;
                x.AddAsset(assetNew);

                Asset assetNew2 = new Asset("New Asset2");
                assetNew2.Command = CMDCommand.delete;
                assetNew2.strDevEnvFile = "c:\\asdf34234";
                assetNew2.strStageEnvFile = "d:\\asdf234234";
                assetNew2.isEnabled = false;
                x.AddAsset(assetNew2);

                liNewList.Add(x);
            }
            return liNewList;
        }
        catch (Exception ex)
        {
            lblLabel.Content = ex.Message;
            throw;
        }
    }
    }


}
