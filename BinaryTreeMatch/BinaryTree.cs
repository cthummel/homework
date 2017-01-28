using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeMatch
{
    class BinaryTreeMatch
    {
        static void Main(string[] args)
        {
            //var rejected = new List<Node>();
            //var success = new List<Node>();
            //var success = new List<int[]>();
            //var rejected = new List<int[]>();
            var successtree = new List<BinaryTree>();
            var rejectedtree = new List<BinaryTree>();
            var uncheckedtree = new List<BinaryTree>();


            //Formatting the initial line of input that is metadata.
            string line = Console.ReadLine();
            string[] TreeParameters = line.Split();

            //Kattis defined n and k values respectively.
            int TREECOUNT = Int32.Parse(TreeParameters[0]);
            int MAXELEMENTS = Int32.Parse(TreeParameters[1]);
            
            //used only for the first tree we look at.
            bool first = true;

            //used to skip steps as needed later.
            bool keepgoing = true;

            //Reads in lines from Kattis's input one at a time.
            while ((line = Console.ReadLine()) != null)
            {
                int[] values = new int[MAXELEMENTS];
                string[] temp = new string[MAXELEMENTS];
                temp = line.Split();

                //Parses line for integer values and stores them in an array.
                for (int i = 0; i < MAXELEMENTS; i++)
                {
                    int j = Int32.Parse(temp[i]);
                    values[i] = j;
                }

                //Creates the new test BinaryTree
                Node root = new Node(values[0]);
                BinaryTree tree = new BinaryTree();
                
                for (int i = 1; i < MAXELEMENTS; i++)
                {

                    root = tree.insert(root, values[i]);
                }

                //tree.Traverse(root);
                //Console.WriteLine(" ");
                uncheckedtree.Add(tree);
            }

            //Tree Checking time!
            BinaryTree treecheck = new BinaryTree();
            for(int i = 0; i < uncheckedtree.Count; i++)
            {
                //First tree is always unique so we add it to solutions.
                if (first == true)
                {
                    successtree.Add(uncheckedtree.ElementAt(0));
                    first = false;
                    keepgoing = false;
                }

                //Now we compare each tree to previously created shapes across the Solutions list and Rejected list.
                if (keepgoing == true)
                {

                    //First we compare current tree to Solutions to see if there is a shape match.

                    //This iteration of the checking is very costly because i recreate each solution tree again instead of 
                    //using a saved version from earlier.
                    for (int j = 0; j < successtree.Count; j++)
                    {
                        if (treecheck.ShapeCheck(successtree.ElementAt(j).root, uncheckedtree.ElementAt(i).root))
                        {
                            successtree.RemoveAt(j);
                            rejectedtree.Add(uncheckedtree.ElementAt(i));
                            keepgoing = false;
                            break;
                        }
                    }


                    //If current wasnt in Solutions list we then compare in rejected list.
                    if (keepgoing == true)
                    {
                        //Again a very inefficient checking method.
                        for (int k = 0; k < rejectedtree.Count; k++)
                        {
                            if (treecheck.ShapeCheck(rejectedtree.ElementAt(k).root, uncheckedtree.ElementAt(i).root))
                            {
                                keepgoing = false;
                                break;
                            }
                        }


                    }

                    //if current isnt in either list we add it to Solutions list.
                    if (keepgoing == true)
                    {
                        successtree.Add(uncheckedtree.ElementAt(i));
                    }

                }
            }

            ////Output the final answer.
            Console.WriteLine(successtree.Count);

        }
        

    }

    /// <summary>
    /// Initializes nodes for use in binary trees.
    /// </summary>
    class Node
    {
        public int value;
        public Node rchild;
        public Node lchild;
        public Node(int data)
        {
            this.value = data;
            lchild = null;
            rchild = null;

        }
    }

    class BinaryTree
    {
        public Node root;
        //public void RootSet(int value)
        //{
        //    root = new Node (value);
        //}
        public void Traverse(Node root)
        {
            if (root == null)
            {
                return;
            }
            
            Traverse(root.lchild);
            Traverse(root.rchild);
            Console.WriteLine(root.value);
        }
        /// <summary>
        /// Inserts a new node into a tree with value v.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public Node insert(Node newroot, int v)
        {
            
            if (newroot == null)
            {
                newroot = new Node(v);

            }
            else if (v < newroot.value)
            {

                newroot.lchild = insert(newroot.lchild, v);
            }
            else
            {

                newroot.rchild = insert(newroot.rchild, v);
            }

            return root = newroot;
        }
       

        /// <summary>
        /// Compares the shape of two tree node by node. Returns true if all nodes are the same shape. Returns false otherwise.
        /// </summary>
        /// <param name="master"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        public bool ShapeCheck(Node master, Node student)
        {
            bool result = true;
            //If Master has no children, check if student also has no children.
            if (master.lchild == null && master.rchild == null)
            {
                //If student has children then shape isnt the same.
                if (student.lchild != null || student.rchild != null)
                {
                    return false;
                }
                //If student doesnt have children then the shape is the same.
                else
                {
                    return true;
                }

            }
            //If Master has no right child but has a left child.
            if (master.lchild != null && master.rchild == null)
            {
                if (student.lchild == null || student.rchild != null)
                {
                    return false;
                }
                else
                {
                     result = ShapeCheck(master.lchild, student.lchild);
                }
            }
            //If master has a right child but no left child
            if (master.lchild == null && master.rchild != null)
            {
                if (student.lchild != null || student.rchild == null)
                {
                    return false;
                }
                else
                {
                     result = ShapeCheck(master.rchild, student.rchild);
                }
            }
            //If Master has both right and left children.
            if (master.lchild != null && master.rchild != null)
            {
                if (student.lchild == null || student.rchild == null)
                {
                    return false;
                }
                else
                {
                    result = ShapeCheck(master.lchild, student.lchild);

                    if(result == false)
                    {
                        return result;
                    }

                    result = ShapeCheck(master.rchild, student.rchild);
                }
            }
            return result;
        }
    }
}

