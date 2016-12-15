using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Nameless
{
    class Filehandler
    {
        public SqlConnection conn;
        public List<string> knownFormats;
        public static List<string> watchedFolders;
        public string conString;
        public DataTable dt;
        public SqlCommand scomm;
        public SqlDataAdapter sda;
        
        public SqlCommandBuilder scb;
       
        public void connect()
        {

            conString = "Data Source=TEEZYT;Integrated Security=SSPI;Initial Catalog=NoName";
            conn = new SqlConnection(conString);
            watchedFolders = new List<string>();
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                
                throw;
            }
            
            conn.Close();
        }//first initialized when programm starts
       
        #region Methods for database
        public  void Select(string tableName)
        {
            dt = new DataTable();
            conn.Open();
            string Comm = "SELECT * From " + tableName;

             sda = new SqlDataAdapter(Comm, conn);
            sda.Fill(dt);
            conn.Close();
        }
        public void UpdateNamingConvention(string nameConventionSelection)
        {
            conn.Open();
            dt = new DataTable();
            string Comm = "SET NameConvention = ID  WHERE ID IN(SELECT NAME FROM tbl_NamingConventions WHERE Format= '" + nameConventionSelection + "')";
             scomm = new SqlCommand(Comm, conn);
            scomm.ExecuteNonQuery();
            conn.Close();
           
            
           

        }//for when the prefrances are set in order for user to decide his naiming convention.
        public void UpdateEpisodeCount(int episodeCount, string Name)
        {
            conn.Open();
            dt = new DataTable();
            string Comm = "SET EpisodeCount = " + episodeCount + " WHERE Name='" + Name + "'";
             scomm = new SqlCommand(Comm, conn);
            scomm.ExecuteNonQuery();
            conn.Close();




        }//will update episode count when change is made.
        public void InsertFolder(string name,int namingConvention,int eCount,string lastEp,string lastSeason,string path,string missinEp) 
        {
            conn.Open();
            string Comm = "sp_insertFolder";
            scomm = new SqlCommand(Comm, conn);
            scomm.CommandType = CommandType.StoredProcedure;
            scomm.Parameters.AddWithValue("@name", name);
            scomm.Parameters.AddWithValue("@namingConvention", namingConvention);
            scomm.Parameters.AddWithValue("@eCount", eCount);
            scomm.Parameters.AddWithValue("@lastEp", lastEp);
            scomm.Parameters.AddWithValue("@lastSeason", lastSeason);
            scomm.Parameters.AddWithValue("@path", path);
            scomm.Parameters.AddWithValue("@missinEp", missinEp);
            scomm.ExecuteNonQuery();
            conn.Close();
        }//inserts a new folder that should be watched.
        public  void InsertFormat(string format)
        {
             conn.Open();
             string Comm = "INSERT   (format)  VALUES( '"+format+"')";  
             scomm = new SqlCommand(Comm, conn);
             scomm.ExecuteNonQuery();
             conn.Close();
        }//if ser needs to add a format that isnt in the list.
        #endregion
        public void SetInnerName (string name,string newName)
        {

            conn.Open();

            scomm = new SqlCommand("sp_SetInnerName", conn);//stroed proc

            scomm.Parameters.AddWithValue("@name", name);
            scomm.Parameters.AddWithValue("@newName", newName);
            
            scomm.ExecuteNonQuery();
            conn.Close();
        }/// <summary>
        /// sets the filenames wthiin the folder
        /// </summary>
        public  void ListPopulation() 
        {
            Select("tbl_watchedFolders");
            watchedFolders.Add("Video Name\tEpisode Number");
            foreach (DataRow item in dt.Rows)
            {
                watchedFolders.Add(item["Name"].ToString()+"         \t"+item["LastEp"]);
            }

        }//will be run when program executes to watch each of the folders.
        public static void Stringcheck() 
        {
           //this method will check every string for the numbers within it.
           //then it will compile  a list of e numbers
            //take the highest //
            //compare the current insertes against the highes 
            //update the folder count
            //update the en
            //rename the content to prefared method
        }//this will check string against folder prefered format
        public void EpCount() 
        {
            //will update the episode count
        }//will check the number of episodes withing the folder
        public void AlreadyExistsDelete(string name)//set all to be updated 
        {
            conn.Open();
            string Comm = "DELETE watchedFolders  WHERE Name= '" + name + "'";
            scomm = new SqlCommand(Comm, conn);
            scomm.ExecuteNonQuery();
            conn.Close();
        }
        
        public void RenameFormat() 
        {

        }// if fileformat needs to be renamed then will rename it like szn to mkv
        public string ExistsCheck(string Name) 
        {
            string temp = "";

            string Comm = "sp_ExistsCheck";
            scomm = new SqlCommand(Comm,conn);
            scomm.Parameters.AddWithValue("@name", Name);
            scomm.CommandType = CommandType.StoredProcedure;
            
            if (temp!=null)
            {
                try 
	                {	        
		                  conn.Open();
                          return temp = (string)scomm.ExecuteScalar();
                          
	                }
	                catch (OleDbException)
	                {
		
		                throw;
	                }
                finally
                {
                    conn.Close();
                }
              
                              
            }
            else
            {
                return temp;
            }
           
                
           
           
          
           
        }
        public void GetFormats() 
        {
            knownFormats = new List<string>();
            Select("tbl_Formats");
            foreach (DataRow item in dt.Rows)
            {
                knownFormats.Add(item["format"].ToString());
            }
        }
        public void GetWatchedFolders() 
        {
            Select("watchedFolders");
        }
        public void GetMaxEpMax() { }//comming soon
        public  void ResetWtachedFolderTable()//will fix once i work
        {
            conn.Open();

            

            string Comm = "delete from   watchedFolders alter table TableName alter column ID alter table TableName alter column ID AUTOINCREMENT  ";
           
            scomm = new SqlCommand(Comm, conn);
            scomm.ExecuteNonQuery();
            conn.Close();
        }
        
        
    }
}
