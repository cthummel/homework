﻿using System;
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
            var success = new List<int[]>();
            var rejected = new List<int[]>();
            //var successtree = new List<BinaryTree>();
            //var rejectedtree = new List<BinaryTree>();


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
                Node root = null;
                BinaryTree tree = new BinaryTree();
                
                for (int i = 0; i < MAXELEMENTS; i++)
                {
                    tree.insert(root, values[i]);
                }

                //First tree is always unique so we add it to solutions.
                if (first == true)
                {
                    success.Add(values);
                    first = false;
                    keepgoing = false;
                }

                //Now we compare each tree to previously created shapes across the Solutions list and Rejected list.
                if (keepgoing == true)
                {

                    //First we compare current tree to Solutions to see if there is a shape match.

                    //This iteration of the checking is very costly because i recreate each solution tree again instead of 
                    //using a saved version from earlier.
                    for (int i = 0; i < success.Count; i++)
                    {
                        BinaryTree successcheck = new BinaryTree();
                        Node successnode = null;
                        int[] successvalues = new int[MAXELEMENTS];
                        successvalues = success.ElementAt(i);
                        for (int j = 0; j < MAXELEMENTS; j++)
                        {
                            successcheck.insert(successnode, successvalues[j]);
                        }

                        if (successcheck.ShapeCheck(successnode, root) == true)
                        {
                            success.RemoveAt(i);
                            rejected.Add(successvalues);
                            keepgoing = false;
                            break;
                        }
                    }


                    //If current wasnt in Solutions list we then compare in rejected list.
                    if (keepgoing == true)
                    {
                        //Again a very inefficient checking method.
                        for (int i = 0; i < rejected.Count; i++)
                        {
                            BinaryTree rejectedcheck = new BinaryTree();
                            Node rejectednode = null;
                            int[] rejectedvalues = new int[MAXELEMENTS];
                            rejectedvalues = rejected.ElementAt(i);
                            for (int j = 0; j < MAXELEMENTS; j++)
                            {
                                rejectedcheck.insert(rejectednode, rejectedvalues[j]);
                            }

                            if (rejectedcheck.ShapeCheck(rejectednode, root) == true)
                            {
                                keepgoing = false;
                                break;
                            }
                        }


                    }

                    //if current isnt in either list we add it to Solutions list.
                    if (keepgoing == true)
                    {
                        success.Add(values);
                    }

                }
                keepgoing = true;
            }
            //Output the final answer.
            Console.WriteLine(success.Count);

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
        private bool result = true;
        /// <summary>
        /// Inserts a new node into a tree with value v.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public Node insert(Node root, int v)
        {

            if (root == null)
            {
                root = new Node(v);
                //root.value = v;

            }
            else if (v < root.value)
            {

                root.lchild = insert(root.lchild, v);
            }
            else
            {

                root.rchild = insert(root.rchild, v);
            }

            return root;
        }
        public void Traverse(Node root)
        {
            


        }

        /// <summary>
        /// Compares the shape of two tree node by node. Returns true if all nodes are the same shape. Returns false otherwise.
        /// </summary>
        /// <param name="master"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        public bool ShapeCheck(Node master, Node student)
        {
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

