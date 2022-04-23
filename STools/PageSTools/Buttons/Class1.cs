using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

class Class1
    {
    static void Main(string[] args)
    {
        string path = @"C:\testfolder\utskrift.txt";
        var fileContent = File.ReadAllText(path);

        var array = fileContent.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
        MessageBox.Show("Hei");

    }

        
    }
    

