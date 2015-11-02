using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSearch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            int amountOfSearchedFiles = 0;
            string path = @"c:\";//default directory
            string searchPattern = textBox2.Text;
            if (searchPattern == string.Empty) searchPattern = "*.txt"; //default file template
            string searchText = textBox1.Text;
           
            FolderBrowserDialog FBD = new FolderBrowserDialog(); //select search directory 
            FBD.ShowNewFolderButton = false;
            if (FBD.ShowDialog() == DialogResult.OK)
                {
                    path = FBD.SelectedPath;
                }
            
            

            DirectoryInfo dir = new DirectoryInfo(path);

            FileInfo[] files = dir.GetFiles(searchPattern, SearchOption.AllDirectories);// get files from selected directory and all it's sub-sirectories
              foreach (FileInfo f in files)
              {
                  listBox1.Items.Clear();
                  listBox1.Items.Add(f.Name);//show the name of currently processed file
                  StreamReader str = new StreamReader(f.FullName, Encoding.Default);
                  while (!str.EndOfStream)
                  {
                      amountOfSearchedFiles++;
                      string st = str.ReadLine();
                      if (st.Contains(searchText))
                      {                         
                          GenerateTree(f.FullName, path); 
                          break;
                      }
                  }
                  
              }
              listBox1.Items.Clear();//Clear the listbox after the search is over
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void GenerateTree(string input, string StartDir)
        {
            input = input.Substring(input.IndexOf(StartDir) + StartDir.Length);
            if (input.IndexOf("/") == -1)
            {
                treeView1.Nodes.Add(input, input); // treeView1.Nodes.Add( string key, string text) used instead of treeView1.Nodes.Add(string text), to use key for searching
            }
            else
            {
                string top = input.Substring(0, input.IndexOf("/"));
                TreeNode topNode;
                input = input.Substring(input.IndexOf("/") + 1);

                if (treeView1.Nodes.ContainsKey(top)) //chek if such node already exists(search key)
                    topNode = treeView1.Nodes[treeView1.Nodes.IndexOfKey(top)];//if node exists, 
                else topNode = treeView1.Nodes.Add(top, top);  //if node doesn't exist, create new
                while (true)
                {
                    if (input.IndexOf("/") == -1)
                    {
                        topNode.Nodes.Add(input, input);
                        break;
                    }
                    top = input.Substring(0, input.IndexOf("/"));
                    input = input.Substring(input.IndexOf("/") + 1);
                    if (topNode.Nodes.ContainsKey(top)) //chek if such node already exists(search key)
                        topNode = topNode.Nodes[topNode.Nodes.IndexOfKey(top)];//if node exists, 
                    else topNode = topNode.Nodes.Add(top, top);  //if node doesn't exist, create new
                }
            }
        }

    }
}
