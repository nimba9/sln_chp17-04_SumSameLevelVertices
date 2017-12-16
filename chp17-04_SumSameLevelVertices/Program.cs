using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace LevelSums
{
    public class BinaryTreeNode
    {
        private int value;
        private bool hasParent;
        private BinaryTreeNode leftChild;
        private BinaryTreeNode rightChild;
        public BinaryTreeNode(int value, BinaryTreeNode leftChild, BinaryTreeNode rightChild)
        {
            this.value = value;
            this.LeftChild = leftChild;
            this.RightChild = rightChild;
        }

        public BinaryTreeNode(int value) : this(value, null, null)
        {
        }

        public bool HasParent
        {
            get
            { return this.hasParent; }
        }

        public int Value
        {
            get
            { return this.value; }

            set
            { this.value = value; }
        }

        public BinaryTreeNode LeftChild
        {
            get
            { return this.leftChild; }

            set
            {
                if (value == null)
                { return; }

                if (value.hasParent)
                { throw new ArgumentException("The node already has a parent!"); }
                
                value.hasParent = true;
                this.leftChild = value;
            }
        }

        public BinaryTreeNode RightChild
        {
            get
            { return this.rightChild; }
            
            set
            {
                if (value == null)
                { return; }

                if (value.hasParent)
                { throw new ArgumentException("The node already has a parent!"); }
                
                value.hasParent = true;
                this.rightChild = value;
            }
        }
    }

    public class BinaryTree
    {
        private BinaryTreeNode root;
        public BinaryTree(int value, BinaryTree leftChild, BinaryTree rightChild)
        {
            BinaryTreeNode leftChildNode = leftChild != null ? leftChild.root : null;
            BinaryTreeNode rightChildNode = rightChild != null ? rightChild.root : null;
            this.root = new BinaryTreeNode(value, leftChildNode, rightChildNode);
        }

        public BinaryTree(BinaryTreeNode value)
        {
            if (value == null)
            { throw new ArgumentNullException("Cannot insert null value!");}

            this.root = value;
        }

        public BinaryTree(int value) : this(value, null, null)
        {
        }

        public BinaryTreeNode Root
        {
            get
            { return this.root;}
        }

        public void PrintLevelSums()
        {
            Queue<BinaryTreeNode> currentLevelNodes = new Queue<BinaryTreeNode>();
            Queue<BinaryTreeNode> nextLevelNodes = new Queue<BinaryTreeNode>();
            List<long> sums = new List<long>();
            currentLevelNodes.Enqueue(this.root);
            while (currentLevelNodes.Count > 0)
            {
                long nextSum = 0;
                while (currentLevelNodes.Count > 0)
                {
                    BinaryTreeNode currentNode = currentLevelNodes.Dequeue();
                    nextSum += currentNode.Value;

                    if (currentNode.LeftChild != null)
                    { nextLevelNodes.Enqueue(currentNode.LeftChild);}

                    if (currentNode.RightChild != null)
                    { nextLevelNodes.Enqueue(currentNode.RightChild);}
                }

                sums.Add(nextSum);
                while (nextLevelNodes.Count > 0)
                {
                    BinaryTreeNode currentChild = nextLevelNodes.Dequeue();
                    currentLevelNodes.Enqueue(currentChild);
                }
            }

            PrintLevelSums(sums);
        }

        public void PrintLevelSums(List<long> sums)
        {
            long sumCounter = 0;
            foreach (var sum in sums)
            {
                Console.WriteLine("Level {0} -> {1}", sumCounter, sum);
                sumCounter++;
            }
        }
    }

    public class LevelSums
    {
        private static List<string> GetChildren(string successors)
        {
            List<string> children = new List<string>();
            int openBrackets = 0;
            StringBuilder currentChildTree = new StringBuilder();
            for (int i = 0; i < successors.Length; i++)
            {
                if (openBrackets == 0 && successors[i] != '(')
                {
                    StringBuilder currentLeaf = new StringBuilder();
                    while (i < successors.Length && (Char.IsDigit(successors[i]) || successors[i] == 'x'))
                    {
                        currentLeaf.Append(successors[i]);
                        i++;
                    }

                    children.Add(currentLeaf.ToString());
                    continue;
                }

                if (successors[i] == ')')
                { openBrackets--;}

                else if (successors[i] == '(')
                { openBrackets++;}

                if (openBrackets == 0)
                {
                    currentChildTree.Append(')');
                    children.Add(currentChildTree.ToString());
                    currentChildTree.Clear();
                    i++;
                }
                else
                { currentChildTree.Append(successors[i]);}
            }

            return children;
        }

        public static BinaryTreeNode ParseTree(string tree)
        {
            if (tree.Contains("->") == false)
            {
                int currentValue = int.Parse(tree);
                return new BinaryTreeNode(currentValue);
            }

            string cleanTree = tree.Substring(1, tree.Length - 2);
            string[] currentNodes = cleanTree.Split(new string[] { "->" }, 2, StringSplitOptions.RemoveEmptyEntries);
            BinaryTreeNode currentNode = new BinaryTreeNode(int.Parse(currentNodes[0]));
            if (currentNodes[1].Contains("->") == true)
            {
                List<string> children = GetChildren(currentNodes[1]);
                if (children[0] != "x")
                {
                    currentNode.LeftChild = ParseTree(children[0]);
                }

                if (children[1] != "x")
                {
                    currentNode.RightChild = ParseTree(children[1]);
                }
            }
            else
            {
                string[] leafs = currentNodes[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (leafs[0] != "x")
                {
                    int leafValue = int.Parse(leafs[0]);
                    currentNode.LeftChild = new BinaryTreeNode(leafValue);
                }
                
                if (leafs[1] != "x")
                {
                    int leafValue = int.Parse(leafs[1]);
                    currentNode.RightChild = new BinaryTreeNode(leafValue);
                }
            }

            return currentNode;
        }

        public static void Main(string[] args)
        {
            string rawTree = Console.ReadLine();
            BinaryTreeNode root = ParseTree(rawTree);
            BinaryTree tree = new BinaryTree(root);
            tree.PrintLevelSums();
        }
    }
}