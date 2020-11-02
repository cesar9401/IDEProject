using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media.TextFormatting;

namespace IDEProject
{
    class TreeNode 
    { 

        public String data { get; set; }
        public int id { get; set; }
        public TreeNode father { get; set; }

        public TreeNode()
        {
        }
     
        public TreeNode(String data, int id)
        {
            this.data = data;
            this.id = id;
        }

        public TreeNode(TreeNode father, String data, int id)
        {
            this.father = father;
            this.data = data;
            this.id = id;
        }
    }
}
