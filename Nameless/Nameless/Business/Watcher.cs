using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Nameless
{
    class Watcher
    {
      public static FileSystemWatcher watcher;

      public void start(string path) 
      {
          watcher=new FileSystemWatcher();
          watcher.Path = path;
          watcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.DirectoryName | NotifyFilters.FileName|NotifyFilters.LastWrite|NotifyFilters.LastAccess|NotifyFilters.CreationTime;
         //works
          watcher.Created += new FileSystemEventHandler(watcher_Changed);
          watcher.Changed += new FileSystemEventHandler(watcher_Changed);
          watcher.EnableRaisingEvents=true;

      }

      void watcher_Changed(object sender, FileSystemEventArgs e)
      {
          if (e.ChangeType==WatcherChangeTypes.Created)
          {
              MessageBox.Show(e.Name.ToString());
              MessageBox.Show("the name got changed then organised  {0}",e.ChangeType.ToString());
             //check filename
              //check ep number
              //if season and ep mathch then deny
              //if season doent match move it to correct season folder
              //if string fromat doesnt match then rename according to fromat


              //ultimatly it will start the application that will rename  all  of this this 
              //what must happen once the directory is changed
  
          }
      }
  
        
       
        
    }
   
    
}
