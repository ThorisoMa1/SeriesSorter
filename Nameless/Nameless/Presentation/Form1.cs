using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Threading;
using System.IO;
using System.Collections;
using Microsoft.VisualBasic;
using System.Web.RegularExpressions;
using System.Text.RegularExpressions;

namespace Nameless
{
    //changes that were made now.
    //trying to do recursie check then adding video paths to tobemoved folder..
    //will then check for others 2 at the same time.
    //will then create the season folders beforem moving and deleting  and other folder
    //will then move every video and other to the seasons and the other folder.
    //will then delete sub folders.
  

    public partial class Form1 : Form
    {
        string minEp = "0";
        public string currMaxSeason;
        public string maxSeason;
        public string missinEp = "";
        public int supposedToBeEpisode=00;//for mising ep
        public static ManualResetEvent recieveDone = new ManualResetEvent(false);
        bool seasonConstant = false;
        int epCount = 0;
        public int root = 0;
        public Thread starter;
        public string season = "";
        public string episode = "";
        
        public List<string> tobeMoved;
        public List<string> Locations;
        public List<string> innerFileNames; //to be  checked for Folders
        public List<string> fileNames;
        public List<string> toBeMovedToOther;
        public bool skipItteration = false;
        public string[] addedFilePaths;

        
        Filehandler fa;

        public Form1()
        {

            fa = new Filehandler();
            fa.connect();
           // fa.GetWatchedFolders();
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            tobeMoved = new List<string>();

            ComboBoxPopulation();//populates combobox


            fa.ListPopulation();

            listBox1.DataSource = Filehandler.watchedFolders;
            fa.GetFormats();

            this.AllowDrop = true;
            this.DragEnter += Form1_DragEnter;
          











        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;

            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)//activated once you drag
        {
            //this wiil get the list of dragged folders and add them to the list of things that should  be watched
            addedFilePaths = (string[])e.Data.GetData(DataFormats.FileDrop,false);

            ExtenstionCheck();
            MessageBox.Show("Done");

            // need to check if the dropped iitem contains videos.
        }
        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            fa.ResetWtachedFolderTable();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            string result = Interaction.InputBox("Please input the new  episode name  ", "new name", null, -1, -1);
            if (result != null)
            {
                if (checkBox1.CheckState == CheckState.Checked)
                {
                    foreach (string item in listBox1.Items)
                    {
                        fa.SetInnerName(item.ToString(), result);

                    }

                }
                else
                {

                    fa.SetInnerName(listBox1.SelectedItem.ToString().Substring(0, listBox1.SelectedItem.ToString().Length - 4), result);

                }
                MessageBox.Show("That will be the new name from now on");
            }


        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
      
        
        
        
        private void   ExtenstionCheck()
        {

            
            Locations = new List<string>();
            DirectoryInfo g;
            bool ContainsJustVideos = false;
            List<int> num = new List<int>();
            List<string> resolutions = new List<string>();
            string numz;
            string maxEp = "0";
            minEp = "";
           


            fa.GetFormats();

            #region Grouping

            foreach (string item in addedFilePaths)//works
            {
                root = addedFilePaths.ToList().IndexOf(item);
                maxEp = "0";
                currMaxSeason = "0";
                innerFileNames = Directory.GetDirectories(item).ToList();
                tobeMoved = new List<string>();
                toBeMovedToOther = new List<string>();
                directoryInsideDirectory(item);///needs testing
                                               ///
                directorMove(item);//works
                deleteFolders(innerFileNames);//deletes remaining folders
               


               



                #endregion





                g = new DirectoryInfo(item);
                epCount = 0;
                maxSeason = "00";


                if (File.Exists(item))
                {
                    //this will check for the file path
                }
                else if (Directory.Exists(item))
                {

                    fileNames = Directory.GetFiles(item).ToList();//this will check for the folder that contains it

                    if (fileNames != null)
                    {
                        foreach (string name in fileNames)
                        {

                            episode = "0";
                            season = "0";

                            numz = "";


                            string wow = Path.GetExtension(name).ToLower().Substring(1);




                            ContainsJustVideos = fa.knownFormats.Contains(Path.GetExtension(name).ToLower().Substring(1));//makes sure that it just has videos



                            RegexOrganiser(name);//this will set the ep numbers and season
                            

















                            #region Resolution Switch
                            switch (ResolutionCheck(numz))//must create regex matc system for it
                            {
                                case 0:
                                    resolutions.Add("360");
                                    break;
                                case 1:
                                    resolutions.Add("360");
                                    break;
                                case 2:
                                    resolutions.Add("720");
                                    break;
                                case 3:
                                    resolutions.Add("1080");
                                    break;
                                default:
                                    break;
                            }

                            #endregion//need to put into own  method
                            maxEp = MaxEpAndSeasonCheck(season, episode, maxEp, currMaxSeason,minEp);
                            //adds max season and episode

                        }
                    }



                   

                        PhaseOne(fileNames.Count, maxEp, item);
                   




                    



                }





                seasonConstant = false;

            }
        }
        private void   ComboBoxPopulation()
        {
            //will change tto stored proc
            fa.Select("tbl_NamingConvention");

            foreach (DataRow item in fa.dt.Rows)
            {
                comboBox1.Items.Add(item["format"]);
            }

        }
        private string MaxEpAndSeasonCheck(string season, string episode, string currMaxEpisode, string maxSe, string minEp)
        {
            if (minEp != "")
            {
                if (int.Parse(episode) < int.Parse(minEp))
                {
                    minEp = episode;//this will set the ranges before the missing episodes are added 
                }

            }
            else
            {
                minEp = episode;
            }
            if (int.Parse(season) > int.Parse(maxSe))
            {
                currMaxSeason = season;
            }
            if (int.Parse(episode) > int.Parse(currMaxEpisode))
            {
                currMaxEpisode = maxSe;
            }

            return currMaxEpisode;

        }//this one gets the max number 
        private void   MissingEpisodeCheck(string episode)
        {
            //this method has a more detailed fix under way
        }
        private int    ResolutionCheck(string item) //checks vid quality
        {
            int temp = 5;

            if (item.Length > 3)//this is just to make sure that thier is a resolution in the video
            {
                if (item.IndexOf("360") != null)
                {
                    temp = 1;
                }
                else if (item.IndexOf("720") != null)
                {
                    temp = 2;
                }
                else if (item.IndexOf("1080") != null)
                {
                    temp = 3;
                }
                else
                {
                    temp = 0;
                }
            }

            return temp;

        }
       
        private void   set_Click(object sender, EventArgs e)
        {
           

            foreach (var item in listBox1.Items)
            {
                //now we set the organization
            }
        }
        private void   listbUpdate()
        {

            fa.ListPopulation();
            listBox1.DataSource = null;
            listBox1.DataSource = Filehandler.watchedFolders;

        }
        private void   directoryInsideDirectory(string DirectoryName) //this will be a recursive method to take out and place files into seasons
        {

            foreach (string item2 in Directory.GetFiles(addedFilePaths[root]))
            {
                if (fa.knownFormats.Contains(Path.GetExtension(item2).Substring(1).ToString()) != true)
                {
                    if (toBeMovedToOther.Contains(item2)==true)
                    {
                        
                    }
                    else
                    {
                        toBeMovedToOther.Add(item2);
                    }
                    //should probably change varible to false.
                }
            }
            foreach (string item in Directory.GetDirectories(DirectoryName))//gets database known directories
            {
                foreach (string item2 in Directory.GetFiles(item))
                {
                    string s = Path.GetExtension(item2);

                    if (fa.knownFormats.Contains(Path.GetExtension(item2).Substring(1).ToString()) != true)
                    {
                        toBeMovedToOther.Add(item2);
                    }


                }
                foreach (string format in fa.knownFormats)
                {
                    
                    foreach (string file in Directory.GetFiles(item, "*." + format))//gets a;; videos with that format
                    {
                        
                            tobeMoved.Add(file);
                       
                           
                        
                        //adds paths of videos to be moved
                    }
                }
                directoryInsideDirectory(item);

            }

        }
        private void   directorMove(string path)//moves the item from one location to the next
        {

            if (toBeMovedToOther.Count>0)
            {
                extrasMove();
                toBeMovedToOther = new List<string>();
            }

            foreach (string item in tobeMoved)
            {
                string FileName = Path.GetFileName(item);

                try
                { 
                    
                    if (File.Exists(Path.Combine(path, FileName)))
                    {
                        //do nothing
                    }
                    else
                    {
                        
                        File.Move(item,Path.Combine(path, FileName));
                    }

                }
                catch (IOException f)
                {

                    MessageBox.Show(f.ToString());
                }



                //check for duplicates 


            }
        }//moves the video to root folder [proper]
        private void   extrasMove()
        {
            if (Directory.Exists(addedFilePaths[root] + @"\Other"))
            {

            }
            else
            {
                Directory.CreateDirectory(addedFilePaths[root] + @"\Other");

            }

            foreach (string item in toBeMovedToOther)
            {
                string FileName = Path.GetFileName(item);
                string directoryName = Path.GetDirectoryName(item) + @"\Other";

                try
                {

                    if (File.Exists(Path.Combine(item, FileName)))
                    {
                        //do nothing
                    }
                    else
                    {

                        File.Move(item, Path.Combine(addedFilePaths[root] + @"\Other", FileName));
                    }

                }
                catch (IOException f)
                {

                    MessageBox.Show(f.ToString());
                }



            }
        }
        private void   deleteFolders(List<string> innerFileNames)
        {
            foreach (string delpath in innerFileNames)
            {

                if (Path.GetFileName(delpath) != "Other")
                {
                    Directory.Delete(delpath, true);
                }
            }
        }
        
        private void   RegexMissingEpisodeList(string seriesName)
        {
       
        }//comming soon
        private void   RegexSeasonPlacement(string seriesName)//working on
        {
            string name = Path.GetFileName(seriesName);
            string currentLocation= Path.GetDirectoryName(seriesName);
            Regex regex = new Regex(@"S(?<season>\d{1,2})E(?<episode>\d{1,2})", RegexOptions.IgnoreCase);//works

            Match match = regex.Match(seriesName);

            if (match.Success)
            {

                season = match.Groups["season"].Value;
                episode = match.Groups["episode"].Value;


            }
            else
            {
                regex = new Regex(@"S(?<season>\d{1,2})[E-_](?<episode>\d{1,2})", RegexOptions.IgnoreCase);
                match = regex.Match(seriesName);
                if (match.Success)
                {
                    season = match.Groups["season"].Value;

                    episode = match.Groups["episode"].Value;


                }
                else
                {
                    regex = new Regex(@"\d{3,4}", RegexOptions.IgnoreCase);//works

                    match = regex.Match(seriesName);

                    if (match.Success)
                    {
                        regex = new Regex(@"[0-9]{1,4}");//works

                        match = regex.Match(match.Value);
                        if (match.Success)
                        {
                            if (match.Length > 3)
                            {

                                season = match.Value.ToString().Substring(0, 2);
                                episode = match.Value.ToString().Substring(2, match.Length - 2);


                            }
                            else
                            {
                                
                                    season = match.Value.ToString().Substring(0, 1);
                                   episode = match.Value.ToString().Substring(1, 2);




                            }
                        }
                        else
                        {
                            regex = new Regex(@"([a-zA-Z])(?<episode>\d{1,2})", RegexOptions.IgnoreCase);
                            match = regex.Match(seriesName);

                            if (match.Success)
                            {
                                seasonConstant = true;

                                episode = match.Groups["episode"].Value;
                              
                                season = string.Format("{0:00}", int.Parse(seasonSpec.SeasonNumber.ToString()));



                            }
                            //more combinations
                        }


                    }
                }
            }

          

            if (season != "")
            {
                season = string.Format("{0:00}", int.Parse(season));

                /*MissingEpisodeCheck(episode);*///will add misssing ep but still in progress 

                if (Directory.Exists(currentLocation+@"Season"+ season.ToString())==false)
                {
                    Directory.CreateDirectory(currentLocation + @"\Season " + season.ToString());
                }
                //will actully try to send the information to a list now
                File.Move(seriesName, Path.Combine(currentLocation + @"\Season " + season.ToString(), name));

            }
           


        }
        private void   RegexOrganiser(string seriesName)
        {
            Regex regex = new Regex(@"S(?<season>\d{1,2})E(?<episode>\d{1,2})", RegexOptions.IgnoreCase);//works

            Match match = regex.Match(seriesName);

            if (match.Success)
            {

                season = match.Groups["season"].Value;
                episode=match.Groups["episode"].Value;
            }
            else
            {
                regex = new Regex(@"S(?<season>\d{1,2})[E-_](?<episode>\d{1,2})", RegexOptions.IgnoreCase);
                match = regex.Match(seriesName);

                if (match.Success)
                {
                    season = match.Groups["season"].Value;
                    episode = match.Groups["episode"].Value;
                    
                }
                else
                {
                    regex = new Regex(@"\d{3,4}", RegexOptions.IgnoreCase);//works

                    match = regex.Match(seriesName);

                    if (match.Success)
                    {
                        regex = new Regex(@"[0-9]{1,4}");//works

                        match = regex.Match(match.Value);
                        if (match.Success)
                        {
                            if (match.Length > 3)
                            {

                                season = match.Value.ToString().Substring(0, 2);

                                episode = match.Value.ToString().Substring(2, match.Length - 2);
                            }
                            else
                            {
                                
                                    season = match.Value.ToString().Substring(0, 1);
                                    episode = match.Value.ToString().Substring(1, 2);

                            
                            }

                        }


                    }
                    else
                    {
                        regex = new Regex(@"([a-zA-Z])(?<episode>\d{1,2})", RegexOptions.IgnoreCase);
                        match = regex.Match(seriesName);

                        if (match.Success)
                        {
                            
                            
                            episode = match.Groups["episode"].Value;

                            if (seasonConstant != true)
                            {

                                SeasonForm();
                                seasonConstant = true;
                               
                                //RecieveDone.WaitOne();



                            }
                           season= string.Format("{0:00}", int.Parse(seasonSpec.SeasonNumber.ToString()));
                            





                        }
                        //more combinations
                    }
                }
               

            }


            #region final Touch
            if (season!="")
            {
                season = string.Format("{0:00}", int.Parse(season));
            }
            if (episode!="")
            {
                episode= string.Format("{0:00}", int.Parse(season));
            }
            #endregion


        }
       
       
        private void   PhaseOne (int epCount,string maxEp,string item)
        {
            
            string actualName = Path.GetFileNameWithoutExtension(item);
            string d = fa.ExistsCheck(actualName);//remove soon

            foreach (string paths in Directory.GetFiles(addedFilePaths[root]))
            {
               
                MissingEpisodeCheck(episode);//will be impimented later
                RegexSeasonPlacement(paths);//places into correct seaeson
            }
            if (fa.ExistsCheck(actualName) == null)
            {
                
                fa.InsertFolder(actualName, 1, epCount, maxEp, currMaxSeason, item,missinEp);//places info into databse

               
                listbUpdate();

                //the watcher will then be started
            }
            else
            {
                //supposed to bbe an update method
               //needs to update everything
                
            }
            for (int i = 0; i < int.Parse(currMaxSeason); i++)//creates season folders
            {
                Directory.CreateDirectory(addedFilePaths[root] + @"\Season " + (string.Format("{0:00}", (i + 1)).ToString()));//still need to round to teo decimals

            }
            


            supposedToBeEpisode = 00;

        }//uesed as last part of phase 1
        private void   SeasonForm()
        {
            MessageBox.Show("The episodes contained in this folder do not have a season number");
            
            seasonSpec sp = new seasonSpec();
            this.Hide();
            var dialog = sp.ShowDialog();
            this.Show();

            //will bring up a messagebox for the user;
        }//will activate the form
       
       
    }
}
