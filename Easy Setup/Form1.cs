using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Easy_Setup
{
    public partial class Form1 : Form
    {
        string temp = Path.GetTempPath(); //IMPORTANT, DON'T CHANGE!!!
        string win_path = Environment.GetFolderPath(Environment.SpecialFolder.Windows); //IMPORTANT, DON'T CHANGE!!!
        string msifile = "setup.msi"; //here put the msi package name
        string soft_name = "WinZip Self-Extractor"; // here put the software name
        string author = "WinZip"; //here put the author name of the software
        string version = "v4.0"; //here put the version of the software
        string software_website = "https://winzip.com"; //here put the website of the software (if one)
        bool require_admin = true; //set true if the installation requires admin permissions or false if not

        public Form1() //IMPORTANT, DON'T CHANGE!!!!
        {
            if (require_admin == true) //if the installer requires admin permission
            {
                try
                {
                    WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                    bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);
                    if (!hasAdministrativeRight) //invoke uac if the installer is run without admin privileges
                    {
                        this.Hide();
                        string fileName = Assembly.GetExecutingAssembly().Location;
                        ProcessStartInfo processInfo = new ProcessStartInfo();
                        processInfo.Verb = "runas";
                        processInfo.FileName = fileName;

                        try
                        {
                            Process.Start(processInfo);
                            Environment.Exit(0);
                        }
                        catch
                        {
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        InitializeComponent();
                    }
                }
                catch { }
            }
            else
            {
                InitializeComponent();
            }
        }
        public static void Extract(string nameSpace, string outDirectory, string internalFilePath, string resourceName)
        {
            //Important.DO NOT CHANGE!!!

            Assembly assembly = Assembly.GetCallingAssembly();

            using (Stream s = assembly.GetManifestResourceStream(nameSpace + "." + (internalFilePath == "" ? "" : internalFilePath + ".") + resourceName))
            using (BinaryReader r = new BinaryReader(s))
            using (FileStream fs = new FileStream(outDirectory + "\\" + resourceName, FileMode.OpenOrCreate))
            using (BinaryWriter w = new BinaryWriter(fs))
                w.Write(r.ReadBytes((int)s.Length));
        }

        private void button2_Click(object sender, EventArgs e) //cancel the installation
        {
            Environment.Exit(0); //exit the program
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = soft_name + " - Easy Setup";
            program_name.Text = soft_name;
            program_author.Text = author;
            program_version.Text = version;
        }

        private void program_name_Click(object sender, EventArgs e)
        {
            if (software_website != null)
            {
                try
                {
                    Process.Start(software_website);
                }
                catch
                {

                }
            }
        }

        private void button1_Click(object sender, EventArgs e) //user pressed install button
        {
            try
            {
                this.Hide();
                Extract("Easy_Setup",temp,"files",msifile);
                ProcessStartInfo install = new ProcessStartInfo();
                install.FileName = win_path + @"\System32\msiexec.exe";
                install.Arguments = $"/i {temp + @"\" + msifile} /qb /norestart";
                var install2 = Process.Start(install);
                install2.WaitForExit();
                File.Delete(temp + @"\" +msifile);
                Environment.Exit(0);
            }
            catch
            {

            }
        }
    }
}
